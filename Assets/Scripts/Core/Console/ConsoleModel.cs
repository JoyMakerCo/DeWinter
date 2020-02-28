using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Ambition;

namespace Core
{
	public class ConsoleModel : Model
	{
		ConsoleView _view;

		Dictionary<string, ConsoleCommand> _commands;
		Dictionary<string, IConsoleEntity> _entities;

		public ConsoleModel ()
		{
			Debug.Log("ConsoleModel()");
         	ConsoleView[] views = GameObject.FindObjectsOfType(typeof(ConsoleView)) as ConsoleView[];
			if ((views != null) && (views.Length > 0))
			{
				_view = views[0];
				configureView();
				_view.Add("Version "+ ConfigurationModel.Config.Version.ToString() );
			}
			else
			{
				Debug.Log("ConsoleModel has no view");
			}

			_commands = new Dictionary<string,ConsoleCommand>();
			_entities = new Dictionary<string,IConsoleEntity>();

			NewCommand( "invoke", InvokeEntity, "trigger an entity" );
			NewCommand( "incident", StartIncident, "start an incident", CollectIncidents );
			NewCommand( "list", ListEntities, "list known entities" );
			NewCommand( "dump", DumpEntity, "display information about an entity" );
			NewCommand( "help", Help, "get information on commands" );
			NewCommand( "send", SendMessage, "send message" );
		}

		void configureView()
		{
			_view.Configure();
		}

		public void log( string text )
		{
			_view?.Add(text);
		}

		public void log(string format, params object[] args)
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
            foreach (UMachine machine in ufs.GetMachines())
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
			// tokenize into command and parameters
			var sep = new char[] { ',',' ' };

            var tokens = input.Split( sep );
			var commandPrefix = tokens[0].ToLower();
			// look up command
			var matchingCommands = Lookup<ConsoleCommand>( commandPrefix, _commands );
			if (matchingCommands.Count == 0)
			{
				log("Unrecognized command '{0}'.", commandPrefix );
				return;
			}
			if (matchingCommands.Count > 1)
			{
				log("Ambiguous command '{0}'. Potential matches:", commandPrefix);
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
		}

		void InvokeEntity( string[] args )
		{
			if (args.Length < 2)
			{
				log( "Error: invoke <target>");
			}
			else
			{
				var matchingEnts = Lookup<IConsoleEntity>( args[1], _entities );
				if (matchingEnts.Count == 0)
				{
					log("Unrecognized entity '{0}'.", args[1] );
					return;
				}
				if (matchingEnts.Count > 1)
				{
					log("Ambiguous entity '{0}'. Potential invokable matches:", args[1]);
					foreach (var me in matchingEnts)
					{
						log("  "+me);
					}
					return;
				}
					
				_entities[matchingEnts[0]].Invoke( args );
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

		void InvokeIncident( string name, string[] args )
		{
			log("NYI invoke incident {0}", name);
		}

		string[] DumpIncident( string name )
		{
			var ic = UnityEngine.Resources.Load("Incidents/"+name) as IncidentConfig;
			if (ic == null)
			{
				log("Error: couldn't load resource '{0}' as an IncidentConfig", name );
				return new string[] {};
			}
			//IncidentVO ivo = new IncidentVO(ic.Incident);
			return ic.GetIncident().Dump();
		}

		void StartIncident( string[] args )
		{
			if (args.Length < 2)
			{
				log( "Error: incident <incident id>");
			}
			else
			{
				InvokeEntity( new string[] { "incident." + args[1] } );
			}
		}

		void ListEntities( string[] args )
		{
			// merge invokable, dumpable and list them
			var ordered = _entities.Keys.OrderBy( k => k );
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
				log( "Error: dump <target>");
			}
			else
			{
				var matchingEnts = Lookup<IConsoleEntity>( args[1], _entities );
				if (matchingEnts.Count == 0)
				{
					log("Unrecognized entity '{0}'.", args[1] );
					return;
				}
				if (matchingEnts.Count > 1)
				{
					log("Ambiguous entity '{0}'. Potential matches:", args[1]);
					foreach (var me in matchingEnts)
					{
						log("  "+me);
					}
					return;
				}

				log("entity '{0}':", matchingEnts[0]);

				foreach (var line in _entities[matchingEnts[0]].Dump())
				{
					log(line);
				}
				log("");
			}
		}

		void SendMessage( string[] args )
		{
			switch (args[0])
			{
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
				// TODO can we deduce type of arg 2+?
				AmbitionApp.SendMessage(args[1],args[2]);
			}
			else
			{
				log("'send' requires a message ID.");
			}
		}
	}

}
