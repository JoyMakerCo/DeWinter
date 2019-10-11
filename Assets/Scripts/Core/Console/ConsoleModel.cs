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

		// After each entity matching attempt, we populate a table of
		// aliases, e.g. "@" matches the last single hit, "@0-@9" matches 
		// previous multi hits, etc.
		Dictionary<string,string> _aliases;

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
				log("Version "+ ConfigurationModel.Config.Version.ToString() + "                              " );
			}
			else
			{
				Debug.Log("ConsoleModel has no view");
			}

			_commands = new Dictionary<string,ConsoleCommand>();
			_entities = new Dictionary<string,IConsoleEntity>();
			_aliases = new Dictionary<string,string>();

			NewCommand( "invoke", InvokeEntity, "trigger an entity" );
			NewCommand( "incident", StartIncident, "start an incident", CollectIncidents );
			NewCommand( "list", ListEntities, "list known entities", CollectEntities );
			NewCommand( "dump", DumpEntity, "display information about an entity" );
			NewCommand( "help", Help, "get information on commands" );
			NewCommand( "send", SendMessage, "send message" );
			NewCommand( "reward", GrantReward, "grant a reward" );
			NewCommand( "set", SetCommodity, "set a commodity value" );
			NewCommand( "shortcuts", ListShortcuts, "show current shortcut table" );

			ConsoleUtilities.TestLookup();
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

		Dictionary<string, IConsoleEntity> ActiveEntities()
		{
			var transientEntities = _entities.ToDictionary(entry => entry.Key,
                                               entry => entry.Value);

			// Add entities that are likely to change here, like currently active UFlow machines

			// Live UFlow machines
			var ufs = AmbitionApp.GetService<UFlowSvc>();
			foreach (var machine in ufs.GetAllMachines())
			{
				if (machine != null)
				{
					transientEntities[ "machine."+machine.MachineID ] = machine;
				}
			}

			// Servants, on staff or otherwise
			foreach (var servant in AmbitionApp.GetModel<ServantModel>().GetAllServants())
			{
				///transientEntities[ "servant."+servant.Name ] = servant;
				transientEntities[ servant.ID ] = servant;
			}

			// Characters

			foreach (var ckv in AmbitionApp.GetModel<CharacterModel>().Characters)
			{
				transientEntities["character."+ckv.Value.Name] = ckv.Value;
			}

			// party, if live
			var pm = AmbitionApp.GetModel<PartyModel>();
			if (pm != null)
			{
				transientEntities["party"] = pm;
			}

			return transientEntities;
		}

		public void ParseInput(string input)
		{
			logInput("> " +input);

			// tokenize into command and parameters
			var sep = new char[] { ',',' ' };

            var tokens = input.Split( sep );
			var commandPrefix = tokens[0].ToLower();
			// look up command
			var matchingCommands = ConsoleUtilities.Lookup( commandPrefix, _commands );
			if (matchingCommands.Length == 0)
			{
				error("Unrecognized command '{0}'.", commandPrefix );
				return;
			}
			if (matchingCommands.Length > 1)
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
				var matchingCommands = ConsoleUtilities.Lookup( args[1], _commands );
				if (matchingCommands.Length == 0)
				{
					error("Unrecognized command '{0}'.", args[1] );
					return;
				}
				if (matchingCommands.Length > 1)
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

		class EntityMatch
		{
			public string Ident;
			public IConsoleEntity Entity;

			public EntityMatch( string ident, IConsoleEntity entity )
			{
				Ident = ident;
				Entity = entity;
			}
		};

		EntityMatch SingleActiveEntity( string ident )
		{
			var ents = ActiveEntities();

			if (_aliases.ContainsKey(ident))
			{
				return new EntityMatch( _aliases[ident], ents[_aliases[ident]] );
			}

			var matchingEnts = ConsoleUtilities.Lookup( ident, ents );

			if (matchingEnts.Length == 0)
			{
				error("Unrecognized entity '{0}'.", ident );
				return null;
			}

			UpdateAliases(matchingEnts);

			if (matchingEnts.Length > 1)
			{
				warn("Ambiguous entity '{0}'. Potential invokable matches:", ident);
				foreach (var me in matchingEnts)
				{
					log("  "+me);
				}
				return null;
			}
				
			var e = ents[matchingEnts[0]];
			if (e != null)
			{
				return new EntityMatch(matchingEnts[0],e);
			}
			else
			{
				warn("Entity '{0}' is null",matchingEnts[0]);
			}			
			return null;
		}

		void InvokeEntity( string[] args )
		{
			if (args.Length < 2)
			{
				error( "Error: invoke <target>");
			}
			else
			{
				var match = SingleActiveEntity(args[1]);
				if (match != null)
				{
					match.Entity.Invoke( args );
				}
			}
		}

		void CollectIncidents()
		{
			log("Loading incidents...");

			var incidents = UnityEngine.Resources.LoadAll<IncidentConfig>("");

			foreach (var ic in incidents)
			{
				var localName = ic.name;
				// TODO deal with identical entity keys?
				var id = "incident." + localName;
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
			_entities["servants"] = AmbitionApp.GetModel<ServantModel>();
			_entities["characters"] = AmbitionApp.GetModel<CharacterModel>();
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

		void UpdateAliases( string[] matches )
		{
			// Update shortcuts dict with result of this match
			if (matches.Length == 1)
			{
				_aliases["@"] = matches[0];
			}
			if (matches.Length > 1)
			{
				_aliases = new Dictionary<string, string>();
				for (int i = 0; i < matches.Length; i++)
				{
					_aliases["@"+i] = matches[i];
				}
			}
		}

		void ListEntities( string[] args )
		{
			// TODO prefix restriction
			var ents = ActiveEntities();

			var matchingEnts = ents.Select( k => k.Key ).ToArray();
			if (args.Length == 2)
			{
				matchingEnts = ConsoleUtilities.Lookup( args[1], ents );
			}

			UpdateAliases( matchingEnts );

			log( "Known entities: ");
			foreach (var eid in matchingEnts)
			{
				log("  "+eid);
			}
		}

		void ListShortcuts( string[] args )
		{
			foreach (var k in _aliases.Keys)
			{
				log( "{0}: {1}", k, _aliases[k] );
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
				var match = SingleActiveEntity(args[1]);
				if (match != null)
				{
					log("entity '{0}':", match.Ident);

					foreach (var line in match.Entity.Dump())
					{
						log(line);
					}
					log("");
				}
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

			// HACKHACK for favor, try matching character ID
			if (cType == CommodityType.Favor)
			{
				var hits = AmbitionApp.GetModel<CharacterModel>().GetCharacters( id );
				if (hits.Length == 1)
				{
					id = hits[0].Name;
				}
			}

			AmbitionApp.SendMessage( new CommodityVO( cType, id, val ));
		}

		void SetCommodity( string[] args )
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
				error("'set' requires either type and value, or type, ID, and value.");
				return;
			}

			AmbitionApp.SendMessage( CommodityMessages.SET_COMMODITY, new CommodityVO( cType, id, val ));
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
