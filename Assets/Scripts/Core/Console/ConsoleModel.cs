using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Ambition;
using UFlow;

namespace Core
{
	public enum ConsoleStyle
	{
		Log,
		Warning,
		Error,
		Input	
	};

	public class ConsoleModel : Model
	{
		private static ConsoleModel _instance; // shady
		ConsoleView _view;

		Dictionary<string, ConsoleCommand> _commands;
		// relatively static entities
		Dictionary<string, IConsoleEntity> _entities;

		Dictionary<string, IConsoleEntity> ActiveEntities()
		{
			var transientEntities = _entities.ToDictionary(entry => entry.Key,
                                               entry => entry.Value);

			// Add entities that are likely to change here, like currently active UFlow machines

			var ufs = AmbitionApp.GetService<UFlowSvc>();

			foreach (var machine in ufs.GetAllMachines())
			{
				if (machine != null)
				{
					transientEntities[ "machine."+machine.MachineID ] = machine;
				}
			}

			return transientEntities;
		}

		public ConsoleModel ()
		{
			_instance = this;
			Debug.Log("ConsoleModel()");
         	ConsoleView[] views = GameObject.FindObjectsOfType(typeof(ConsoleView)) as ConsoleView[];
			if ((views != null) && (views.Length > 0))
			{
				_view = views[0];
				configureView();
				// Force scroll view to fill so version appears at bottom instead of in the middle :P
				for (int i = 0; i < 60; i++)
				{
					log("");
				}
				log("Version "+ ConfigurationModel.Config.Version.ToString() );
			}
			else
			{
				Debug.Log("ConsoleModel has no view");
			}

			_commands = new Dictionary<string,ConsoleCommand>();
			_entities = new Dictionary<string,IConsoleEntity>();

			NewCommand( "invoke", InvokeEntity, "trigger an entity" );
			NewCommand( "incident", StartIncident, "start an incident", CollectIncidents );
			NewCommand( "list", ListEntities, "list known entities", CollectEntities );
			NewCommand( "dump", DumpEntity, "display information about an entity" );
			NewCommand( "help", Help, "get information on commands" );
			NewCommand( "send", SendMessage, "send message" );
			NewCommand( "reward", GrantReward, "grant a reward" );
		}

		void configureView()
		{
			_view.Configure();
		}

		public static void log( string text )
		{
			_instance._view?.Add(text);
		}

		public static void log(string format, params object[] args)
		{
			log(string.Format(format,args));
		}

		public static void warn( string text )
		{
			_instance._view?.Add(text,ConsoleStyle.Warning);
		}

		public static void warn(string format, params object[] args)
		{
			warn(string.Format(format,args));
		}

		public static void error( string text )
		{
			_instance._view?.Add(text,ConsoleStyle.Error);
		}

		public static void error(string format, params object[] args)
		{
			error(string.Format(format,args));
		}

		public static void logInput( string text )
		{
			_instance._view?.Add(text,ConsoleStyle.Input);
		}

		public static void logInput(string format, params object[] args)
		{
			logInput(string.Format(format,args));
		}

		public void ParseInput(string input)
		{
			logInput("> " +input);

			// tokenize into command and parameters
			var sep = new char[] { ',',' ' };

            var tokens = input.Split( sep );
			var commandPrefix = tokens[0].ToLower();
			// look up command
			var matchingCommands = Lookup<ConsoleCommand>( commandPrefix, _commands );
			if (matchingCommands.Count == 0)
			{
				error("Unrecognized command '{0}'.", commandPrefix );
				return;
			}
			if (matchingCommands.Count > 1)
			{
				warn("Ambiguous command '{0}'. Potential matches:", commandPrefix);
				foreach (var mc in matchingCommands)
				{
					log("  "+mc);
				}
				return;
			}

			// handle command
			_commands[matchingCommands[0]].handler(tokens);
		}

		bool PrefixMatch( string prefix, string candidate )
		{
			return candidate.ToLower().StartsWith( prefix.ToLower() );
		}

		List<string> Lookup<T>( string commandPrefix, Dictionary<string,T> source )
		{
			List<string> result = new List<string>();
			foreach (var kv in source)
			{
				if (PrefixMatch(commandPrefix,kv.Key))
				{
					result.Add(kv.Key);
				}
			}

			return result;
		}


		void NewCommand( string n, ConsoleCommandHandler cch, string help, Action setup = null )
		{
			RegisterCommand( new ConsoleCommand(n,cch,help,setup) );
		}

		void RegisterCommand( ConsoleCommand cmd  )
		{
			if (_commands.ContainsKey(cmd.name))
			{
				Debug.LogErrorFormat("'{0}' already registered in console commands",cmd.name);
				return;
			}

			_commands[cmd.name] = cmd;
		}

		// commands

		void Help( string[] args )
		{
			if (args.Length < 2)
			{
				log("Registered commands:");
				foreach (var kv in _commands)
				{
					log("  "+kv.Key);
				}

			}
			else if (args.Length == 2)
			{
				var matchingCommands = Lookup<ConsoleCommand>( args[1], _commands );
				if (matchingCommands.Count == 0)
				{
					error("Unrecognized command '{0}'.", args[1] );
					return;
				}
				if (matchingCommands.Count > 1)
				{
					warn("Ambiguous command '{0}'. Potential matches:", args[1]);
					foreach (var mc in matchingCommands)
					{
						log("  "+mc);
					}
					return;
				}

				// handle command
				log( _commands[matchingCommands[0]].help );
			}
		}

		void InvokeEntity( string[] args )
		{
			if (args.Length < 2)
			{
				error( "Error: invoke <target>");
			}
			else
			{
				var ents = ActiveEntities();
				var matchingEnts = Lookup<IConsoleEntity>( args[1], ents );
				if (matchingEnts.Count == 0)
				{
					error("Unrecognized entity '{0}'.", args[1] );
					return;
				}
				if (matchingEnts.Count > 1)
				{
					warn("Ambiguous entity '{0}'. Potential invokable matches:", args[1]);
					foreach (var me in matchingEnts)
					{
						log("  "+me);
					}
					return;
				}
					
				ents[matchingEnts[0]].Invoke( args );
			}
		}

		string EntityKey( string name )
		{
			return name.Replace(" ","_");
		}
		void CollectIncidents()
		{
			log("Loading incidents...");

			var incidents = UnityEngine.Resources.LoadAll<IncidentConfig>("");

			foreach (var ic in incidents)
			{
				var localName = ic.name;
				// TODO deal with identical entity keys?
				var id = "incident." + EntityKey(localName);
				var ie = new ConsoleEntity( id, (args) => { InvokeIncident(localName,args); }, () => { return DumpIncident(localName); } );
				_entities[id] = ie;
			}
		}

		void CollectEntities()
		{
			// Incidents already taken care of
			
			_entities["game"] = AmbitionApp.GetModel<GameModel>();
			_entities["calendar"] = AmbitionApp.GetModel<CalendarModel>();
			_entities["inventory"] = AmbitionApp.GetModel<InventoryModel>();

			var factionModel = AmbitionApp.GetModel<FactionModel>();
			_entities["factions"] = factionModel;

			foreach (var faction in factionModel.Factions.Values)
			{
				var localFaction = faction;
				var id = "faction."+localFaction.Name;
				_entities[id] =
					new ConsoleEntity( id, (args) => { InvokeFaction(localFaction,args); }, () => { return DumpFaction(localFaction); } );
			}

			_entities["uflow"] = AmbitionApp.GetService<UFlowSvc>();

		}

		void InvokeIncident( string name, string[] args )
		{
			warn("NYI invoke incident {0}", name);
		}

		string[] DumpIncident( string name )
		{
			var ic = UnityEngine.Resources.Load("Incidents/"+name) as IncidentConfig;
			if (ic == null)
			{
				error("Error: couldn't load resource '{0}' as an IncidentConfig", name );
				return new string[] {};
			}
			//IncidentVO ivo = new IncidentVO(ic.Incident);
			return ic.GetIncident().Dump();
		}

		void InvokeFaction( FactionVO fac, string[] args )
		{
			warn("NYI invoke faction {0}", fac.Name);
		}

		string[] DumpFaction( FactionVO fac )
		{
			return new string[] {
				"Faction "+fac.Name+":",
				"  modesty: " + fac.Modesty.ToString(),
				"  luxury: " + fac.Luxury.ToString(),
				"  steadfast: " + fac.Steadfast.ToString(),
				"  baroque: " + fac.Baroque[0].ToString() + "-" +fac.Baroque[1].ToString(),
				"  allegiance: " + fac.Allegiance.ToString(),
				"  power: " + fac.Power.ToString(),
				"  level: " + fac.Level.ToString(),
				"  reputation: " + fac.Reputation.ToString()
			};
		}

		void StartIncident( string[] args )
		{
			if (args.Length < 2)
			{
				error( "Error: incident <incident id>");
			}
			else
			{
				InvokeEntity( new string[] { "incident." + args[1] } );
			}
		}

		void ListEntities( string[] args )
		{
			// TODO prefix restriction
			var ents = ActiveEntities();

			var matchingEnts = ents.Select( k => k.Key );
			if (args.Length == 2)
			{
				matchingEnts = Lookup<IConsoleEntity>( args[1], ents );
			}
			var ordered = matchingEnts.OrderBy( k => k );
			log( "Known entities: ");
	
			foreach (var eid in ordered)
			{
				log("  "+eid);
			}
		}

		void DumpEntity( string[] args )
		{
			if (args.Length < 2)
			{
				error( "Error: dump <target>");
			}
			else
			{
				var ents = ActiveEntities();
				var matchingEnts = Lookup<IConsoleEntity>( args[1], ents );
				if (matchingEnts.Count == 0)
				{
					error("Unrecognized entity '{0}'.", args[1] );
					return;
				}
				if (matchingEnts.Count > 1)
				{
					warn("Ambiguous entity '{0}'. Potential matches:", args[1]);
					foreach (var me in matchingEnts)
					{
						log("  "+me);
					}
					return;
				}

				log("entity '{0}':", matchingEnts[0]);

				foreach (var line in ents[matchingEnts[0]].Dump())
				{
					log(line);
				}
				log("");
			}
		}


		void GrantReward( string[] args )
		{
			CommodityType cType = CommodityType.Livre;
			int val = 0;
			string id = "";

			if (args.Length > 1)
			{
				if (!Enum.TryParse<CommodityType>(args[1], ignoreCase:true, out cType))
				{
					error("Can't interpret {0} as a CommodityType.", args[1]);
				}
			}

			if (args.Length == 3)
			{
				// "reward CommodityType value"
				if (!int.TryParse(args[2], out val))
				{
					error("Can't interpret {0} as a number.", args[2]);
				}

			}
			else if (args.Length == 4)
			{
				id = args[2];
				// "reward CommodityType ID value"
				if (!int.TryParse(args[3], out val))
				{
					error("Can't interpret {0} as a number.", args[3]);
				}
			}
			else
			{
				error("'reward' requires either type and value, or type, ID, and value.");
				return;
			}

			AmbitionApp.SendMessage( new CommodityVO( cType, id, val ));
		}

		void SendMessage( string[] args )
		{
			switch (args[0])
			{
				case IncidentMessages.END_INCIDENT:
					AmbitionApp.SendMessage(IncidentMessages.END_INCIDENT, AmbitionApp.GetModel<IncidentModel>().Incident);
					break;

				default:
					SendOtherMessage(args);
					break;

			}
		}

		void SendOtherMessage( string[] args )
		{
			if (args.Length == 2)
			{
				AmbitionApp.SendMessage(args[1]);
			}
			else if (args.Length > 2)
			{
				int intArg;
				// TODO - infer the type of arg 2. 
				// If it parses as int, send it as int, otherwise as string
				if (int.TryParse(args[2],out intArg))
				{
					AmbitionApp.SendMessage(args[1],intArg);

				}
				else
				{
					AmbitionApp.SendMessage(args[1],args[2]);
				}
			}
			else
			{
				error("'send' requires a message ID.");
			}
		}

	}

}
