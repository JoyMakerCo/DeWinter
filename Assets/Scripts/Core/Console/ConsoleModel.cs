#if DEBUG
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
        Dictionary<string, ConsoleEntity> _entities;

        // After each entity matching attempt, we populate a table of
        // aliases, e.g. "@" matches the last single hit, "@0-@9" matches 
        // previous multi hits, etc.
        ConsoleEntity[] _aliases;

        public ConsoleModel()
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
                log("Version " + Application.version + "                              ");
            }
            else
            {
                Debug.Log("ConsoleModel has no view");
            }

            _commands = new Dictionary<string, ConsoleCommand>();
            _entities = new Dictionary<string, ConsoleEntity>();
            _aliases = new ConsoleEntity[0];

            NewCommand("voke", InvokeEntity, "trigger an entity");
            NewCommand("incident", StartIncident, "start an incident", CollectIncidents);
            NewCommand("end", EndIncident, "Ends the current incident");
            NewCommand("list", ListEntities, "list known entities", CollectEntities);
            NewCommand("dump", DumpEntity, "display information about an entity");
            NewCommand("help", Help, "get information on commands");
            NewCommand("send", SendMessage, "send message");
            NewCommand("reward", GrantReward, "grant a reward");
            NewCommand("check", CheckRequirement, "check a requirement");
            NewCommand("set", SetCommodity, "set a commodity value");
            NewCommand("shortcuts", ListShortcuts, "show current shortcut table");
            NewCommand("dialog", OpenDialog, "open a popup dialog by ID");

            // handy quick use tools
            NewCommand("inventory", DumpInventory, "dump inventory");
            NewCommand("gossip", UnlockGossip, "unlock gossip features for testing");
            NewCommand("clear", ClearSaves, "clear all saved games");
            NewCommand("sempai", MakeDateable, "make the named romance option dateable");
            NewCommand("discover", Discover, "reveal a location in Paris");
            NewCommand("explore", Explore, "mark a location as explored");
            NewCommand("date", Date, "set the current date to MMDDYYYY");
            NewCommand("party", GenerateParty, "Generates a random party");
            NewCommand("dehumidify", ResetSteam, "Resets Steam User Data");
            ConsoleUtilities.TestLookup();
        }

        void configureView()
        {
            _view.Configure();
        }

        public static void log(string text)
        {
            _instance._view?.Add(text);
        }

        public static void log(string format, params object[] args)
        {
            log(string.Format(format, args));
        }

        public static void warn(string text)
        {
            _instance._view?.Add(text, ConsoleStyle.Warning);
        }

        public static void warn(string format, params object[] args)
        {
            warn(string.Format(format, args));
        }

        public static void error(string text)
        {
            _instance._view?.Add(text, ConsoleStyle.Error);
        }

        public static void error(string format, params object[] args)
        {
            error(string.Format(format, args));
        }

        public static void logInput(string text)
        {
            _instance._view?.Add(text, ConsoleStyle.Input);
        }

        public static void logInput(string format, params object[] args)
        {
            logInput(string.Format(format, args));
        }

        Dictionary<string, ConsoleEntity> ActiveEntities()
        {
            var transientEntities = _entities.ToDictionary(entry => entry.Key,
                                               entry => entry.Value);

            // Add entities that are likely to change here, like currently active UFlow machines

            // Live UFlow machines
            var ufs = AmbitionApp.GetService<UFlowSvc>();
            foreach (var machine in ufs.GetAllFlows())
            {
                if (machine != null)
                {
                    transientEntities["machine." + machine.FlowID] = new ConsoleEntity(machine);
                }
            }

            // Servants, on staff or otherwise
            foreach (var servant in AmbitionApp.GetModel<ServantModel>().GetAllServants())
            {
                transientEntities[servant.ID] = new ConsoleEntity(servant);
            }

            // Characters

            foreach (var ckv in AmbitionApp.GetModel<CharacterModel>().Characters)
            {
                transientEntities["character." + ckv.Value.ID] = new ConsoleEntity(ckv.Value);
            }

            // party, if live
            var pm = AmbitionApp.GetModel<PartyModel>();
            if (pm != null)
            {
                transientEntities["party"] = new ConsoleEntity(pm);
            }

            return transientEntities;
        }

        public void ParseInput(string input)
        {
            logInput("> " + input);

            // tokenize into command and parameters
            var sep = new char[] { ',', ' ' };

            var tokens = input.Split(sep);
            var commandPrefix = tokens[0].ToLower();
            // look up command
            var matchingCommands = ConsoleUtilities.Lookup(commandPrefix, _commands);
            if (matchingCommands.Length == 0)
            {
                error("Unrecognized command '{0}'.", commandPrefix);
                return;
            }
            if (matchingCommands.Length > 1)
            {
                warn("Ambiguous command '{0}'. Potential matches:", commandPrefix);
                foreach (var mc in matchingCommands)
                {
                    log("  " + mc);
                }
                return;
            }

            // handle command
            _commands[matchingCommands[0]].handler(tokens);
        }

        void NewCommand(string n, ConsoleCommandHandler cch, string help, Action setup = null)
        {
            RegisterCommand(new ConsoleCommand(n, cch, help, setup));
        }

        void RegisterCommand(ConsoleCommand cmd)
        {
            if (_commands.ContainsKey(cmd.name))
            {
                Debug.LogErrorFormat("'{0}' already registered in console commands", cmd.name);
                return;
            }

            _commands[cmd.name] = cmd;
        }

        // commands

        void Help(string[] args)
        {
            if (args.Length < 2)
            {
                log("Registered commands:");
                foreach (var kv in _commands)
                {
                    log("  " + kv.Key);
                }

            }
            else if (args.Length == 2)
            {
                var matchingCommands = ConsoleUtilities.Lookup(args[1], _commands);
                if (matchingCommands.Length == 0)
                {
                    error("Unrecognized command '{0}'.", args[1]);
                    return;
                }
                if (matchingCommands.Length > 1)
                {
                    warn("Ambiguous command '{0}'. Potential matches:", args[1]);
                    foreach (var mc in matchingCommands)
                    {
                        log("  " + mc);
                    }
                    return;
                }

                // handle command
                log(_commands[matchingCommands[0]].help);
            }
        }

        ConsoleEntity SingleActiveEntity(string ident)
        {
            if (ident[0] == '@')
            {
                int index = 0;
                if (ident.Length > 1) int.TryParse(ident.Substring(1), out index);
                if (index < _aliases.Length) return _aliases[index];
                error("Invalid shortcut '{0}'.", ident);
                return null;
            }

            Dictionary<string, ConsoleEntity> ents = ActiveEntities();
            if (!ents.TryGetValue(ident, out ConsoleEntity ent) || ent == null)
            {
                error("Unrecognized entity '{0}'.", ident);
                return null;
            }

            string[] matchingEnts = ConsoleUtilities.Lookup(ident, ents);
            if (matchingEnts.Length > 1)
            {
                warn("Ambiguous entity '{0}'. Potential invokable matches:", ident);
                foreach (string me in matchingEnts)
                {
                    log("  " + me);
                }
                return null;
            }

            ConsoleEntity match = ents[matchingEnts[0]];
            if (match?.Entity != null) return match;

            warn("Entity '{0}' is null", matchingEnts[0]);
            return null;
        }

        void InvokeEntity(string[] args)
        {
            if (args.Length < 2)
            {
                error("Error: voke <target>");
            }
            else
            {
                ConsoleEntity match = SingleActiveEntity(args[1]);
                match?.Invoke(args);
            }
        }

        void CollectIncidents()
        {
            log("Loading incidents...");
            List<string> incidents = new List<string>();
            IncidentConfig[] configs = Resources.FindObjectsOfTypeAll<IncidentConfig>();
            PartyConfig[] parties = Resources.LoadAll<PartyConfig>("Parties");
            LocationConfig[] locations = Resources.FindObjectsOfTypeAll<LocationConfig>();
            PlayerConfig[] players = Resources.FindObjectsOfTypeAll<PlayerConfig>();

            incidents.AddRange(configs.Select(c => c.name));
            foreach (PartyConfig party in parties)
            {
                if (party.IntroIncident != null) incidents.Add(party.IntroIncident.name);
                if (party.ExitIncident != null) incidents.Add(party.ExitIncident.name);
                incidents.AddRange(party.RequiredIncidents.Select(p => p?.name).Where(c => c != null));
                incidents.AddRange(party.SupplementalIncidents.Select(p => p.name));
            }
            foreach (LocationConfig location in locations)
            {
                if (location.IntroIncidentConfig != null) incidents.Add(location.IntroIncidentConfig.name);
                incidents.AddRange(location.StoryIncidentConfigs.Select(i => i.name));
            }
            foreach (PlayerConfig player in players)
            {
                incidents.AddRange(player.Incidents.Select(i => i.name));
            }

            foreach (string incident in incidents)
            {
                var id = "incident." + incident;
                _entities[id] = new ConsoleEntity(incident.ToString(), (args) => { InvokeIncident(incident, args); });
            }
        }

        void CollectEntities()
        {
            // Incidents already taken care of
            FactionModel factions = AmbitionApp.GetModel<FactionModel>();
            _entities["factions"] = new ConsoleEntity(factions);
            _entities["game"] = new ConsoleEntity(AmbitionApp.GetModel<GameModel>());
            _entities["calendar"] = new ConsoleEntity(AmbitionApp.GetModel<CalendarModel>());
            _entities["inventory"] = new ConsoleEntity(AmbitionApp.GetModel<InventoryModel>());

            foreach (FactionVO faction in factions.Factions.Values)
            {
                _entities["faction." + faction.Type.ToString().ToLower()] =
                    new ConsoleEntity(faction, (args) => InvokeFaction(faction, args));
            }

            _entities["uflow"] = new ConsoleEntity(AmbitionApp.GetService<UFlowSvc>());
            _entities["servants"] = new ConsoleEntity(AmbitionApp.GetModel<ServantModel>());
            _entities["characters"] = new ConsoleEntity(AmbitionApp.GetModel<CharacterModel>());
        }

        void InvokeIncident(string name, string[] args)
        {
            IncidentModel model = AmbitionApp.GetModel<IncidentModel>();
            IncidentVO incident;
            if (!model.Incidents.TryGetValue(name, out incident))
            {
                PlayerConfig[] players = Resources.LoadAll<PlayerConfig>("PlayerConfigs");
                foreach (PlayerConfig player in players)
                {
                    foreach (IncidentConfig config in player.Incidents)
                    {
                        if (config?.name == name) incident = config.GetIncident();
                    }
                    if (incident != null) break;
                }
                if (incident == null)
                {
                    LocationConfig[] locations = Resources.FindObjectsOfTypeAll<LocationConfig>();
                    foreach (LocationConfig location in locations)
                    {
                        if (location.IntroIncidentConfig?.name == name) incident = location.IntroIncidentConfig.GetIncident();
                        if (incident == null && location.StoryIncidentConfigs != null)
                        {
                            foreach (IncidentConfig config in location.StoryIncidentConfigs)
                            {
                                if (config?.name == name) incident = config.GetIncident();
                            }
                        }
                        if (incident != null) break;
                    }
                }
                if (incident == null)
                {
                    PartyConfig[] configs = Resources.LoadAll<PartyConfig>("Parties");
                    foreach (PartyConfig party in configs)
                    {
                        if (party.IntroIncident?.name == name) incident = party.IntroIncident.GetIncident();
                        else if (party.ExitIncident?.name == name) incident = party.ExitIncident.GetIncident();
                        else foreach (IncidentConfig config in party.SupplementalIncidents)
                            {
                                if (config.name == name) incident = config.GetIncident();
                            }
                        if (incident == null) foreach (IncidentConfig config in party.RequiredIncidents)
                            {
                                if (config?.name == name) incident = config.GetIncident();
                            }
                        if (incident != null) break;
                    }
                }
                if (incident == null)
                {
                    IncidentConfig[] configs = Resources.LoadAll<IncidentConfig>("Incidents");
                    incident = Array.Find(configs, i => i.name == name)?.GetIncident();
                }
                if (incident != null) model.Incidents[name] = incident;
            }
            if (model.SetTestIncident(name))
            {
                UFlowSvc uflow = _entities["uflow"].Entity as UFlowSvc;
                uflow.Reset();
                uflow.Register<TestIncidentFlow>("TestFlow");
                uflow.Invoke("TestFlow");
            }
            else
            {
                error("Error: no incident called " + name);
            }
        }

        void InvokeFaction(FactionVO fac, string[] args)
        {
            warn("NYI invoke faction {0}", fac.Type.ToString().ToLower());
        }

        void StartIncident(string[] args)
        {
            if (args.Length < 2)
            {
                error("Error: incident <incident id>");
            }
            else
            {
                InvokeEntity(new string[] { "voke", "incident." + args[1] });
            }
        }

        void EndIncident(string[] args)
        {
            AmbitionApp.Story.CompleteCurrentIncident();
        }

        void UpdateAliases(ConsoleEntity[] matches)
        {
            // Update shortcuts dict with result of this match
            _aliases = new ConsoleEntity[matches?.Length ?? 0];
            Array.Copy(matches, _aliases, _aliases.Length);
        }

        void ListEntities(string[] args)
        {
            var ents = ActiveEntities();

            ConsoleEntity[] matchingEnts;
            if (args.Length < 2) matchingEnts = ents.Select(k => k.Value).ToArray();
            else
            {
                string[] lookup = ConsoleUtilities.Lookup(args[1], ents);
                matchingEnts = ents.Where(k => Array.IndexOf(lookup, k.Key) >= 0).Select(k => k.Value).ToArray();
            }

            UpdateAliases(matchingEnts);

            log("Known entities: ");
            Array.ForEach(matchingEnts, e => log(" " + e));
        }

        void ListShortcuts(string[] args)
        {
            int len = _aliases?.Length ?? 0;
            for (int i=0; i<len; ++i)
                log("@{0}: {1}", i, _aliases[i]);
        }

        void DumpEntity(string[] args)
        {
            if (args.Length < 2)
            {
                error("Error: dump <target>");
            }
            else
            {
                var match = SingleActiveEntity(args[1]);
                if (match != null)
                {
                    string[] lines = match.Entity.ToString().Split('\n');
                    log("entity '{0}':", args[1]);
                    Array.ForEach(lines, l => log(" " + l));
                    log("");
                }
            }
        }


        void GrantReward(string[] args)
        {
            CommodityType cType = CommodityType.Livre;
            int val = 0;
            string id = "";

            if (args.Length > 1)
            {
                if (!Enum.TryParse<CommodityType>(args[1], ignoreCase: true, out cType))
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

            if (cType == CommodityType.Favor)
            {
                CharacterVO character = AmbitionApp.GetModel<CharacterModel>().GetCharacter(id);
                if (character == null)
                {
                    error("Character '{0}' not found.", id);
                    return;
                }
            }

            AmbitionApp.SendMessage(new CommodityVO(cType, id, val));
        }

        void CheckRequirement(string[] args)
        {
            if (args.Length <= 1)
            {
                error("'check' requires a type argument.");
            }

            if (!Enum.TryParse<CommodityType>(args[1], ignoreCase: true, out CommodityType type))
            {
                error("Can't interpret {0} as a CommodityType.", args[1]);
            }

            RequirementVO req = new RequirementVO() { Type = type };

            if (args.Length > 2)
                if (!int.TryParse(args[2], out req.Value))
                    req.ID = args[2];
            if (args.Length > 3)
                if (!int.TryParse(args[3], out req.Value))
                    req.ID = args[3];

            log(AmbitionApp.CheckRequirement(req)
                ? "passed with default operator"
                : "failed with default operator");
        }

        void SetCommodity(string[] args)
        {
            CommodityType cType = CommodityType.Livre;
            GameModel game = AmbitionApp.GetModel<GameModel>();

            int val = 0;
            string id = "";

            if (args.Length > 1)
            {
                if (!Enum.TryParse<CommodityType>(args[1], ignoreCase: true, out cType))
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


            FactionType factionType;
            FactionVO faction;

            switch (cType)
            {
                case CommodityType.Livre:
                    AmbitionApp.Game.Livre = val;
                    AmbitionApp.SendMessage(GameConsts.LIVRE, val);
                    break;

                case CommodityType.Exhaustion:
                    AmbitionApp.Game.Exhaustion = val;
                    break;

                case CommodityType.Credibility:
                    AmbitionApp.Game.Credibility = val;
                    AmbitionApp.SendMessage(GameConsts.CREDIBILITY, val);
                    break;

                case CommodityType.Peril:
                    AmbitionApp.Game.Peril = val;
                    AmbitionApp.SendMessage(GameConsts.PERIL, val);
                    break;

                case CommodityType.Power:
                    if (id != null && Enum.TryParse<FactionType>(id, true, out factionType))
                    {
                        AmbitionApp.Politics.Factions.TryGetValue(factionType, out faction);
                        if (faction != null) faction.Power = val;
                    }
                    else
                    {
                        Debug.LogWarning("SetCommodityCmd with FactionPower for unidentified faction ");
                    }
                    break;

                case CommodityType.Allegiance:
                    if (id != null && Enum.TryParse<FactionType>(id, true, out factionType))
                    {
                        AmbitionApp.Politics.Factions.TryGetValue(factionType, out faction);
                        if (faction != null) faction.Allegiance = val;
                    }
                    else
                    {
                        Debug.LogWarning("SetCommodityCmd with FactionAllegiance for unidentified faction ");
                    }
                    break;

                case CommodityType.Favor:
                    CharacterVO character = AmbitionApp.GetModel<CharacterModel>().GetCharacter(id);
                    if (character == null)
                    {
                        Debug.LogErrorFormat("Unrecognized character '{0}'", id);
                    }
                    else
                    {
                        character.Favor = val;
                        if (character.Favor < 0) character.Favor = 0;
                        else if (character.Favor > 100) character.Favor = 100;
                    }
                    break;

                default:
                    Debug.LogWarningFormat("Don't know how to set commodity type '{0}'", cType);
                    break;
            }
        }

        void SendMessage(string[] args)
        {
            switch (args[0])
            {
                case IncidentMessages.END_INCIDENT:
                    string id = AmbitionApp.Story.Incident?.ID;
                    AmbitionApp.SendMessage(IncidentMessages.END_INCIDENT, id);
                    break;

                default:
                    SendOtherMessage(args);
                    break;

            }
        }

        void SendOtherMessage(string[] args)
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
                if (int.TryParse(args[2], out intArg))
                {
                    AmbitionApp.SendMessage(args[1], intArg);

                }
                else
                {
                    AmbitionApp.SendMessage(args[1], args[2]);
                }
            }
            else
            {
                error("'send' requires a message ID.");
            }
        }

        // inventory shortcut
        void DumpInventory(string[] args)
        {
            DumpEntity(new string[] { "dump", "inventory" });
        }

        void UnlockGossip(string[] args)
        {
            AmbitionApp.SendMessage(new CommodityVO(CommodityType.Location, "La Trompette du Peuple", 1));

            int count = 3;
            if (args.Length > 1)
            {
                int.TryParse(args[1], out count);
            }
            for (int i = 0; i < count; i++)
            {
                var fac = Util.RNG.TakeRandom(new string[] { "Crown", "Church", "Military", "Bourgeoisie", "Revolution" });
                AmbitionApp.SendMessage(new CommodityVO(CommodityType.Gossip, fac, i % 3));
            }
        }

        void ClearSaves(string[] args)
        {
            string path = System.IO.Path.Combine(UnityEngine.Application.persistentDataPath, Filepath.SAVE_FILE);
            System.IO.File.Delete(path);
            path = System.IO.Path.Combine(UnityEngine.Application.persistentDataPath, Filepath.AUTOSAVE);
            System.IO.File.Delete(path);
            string[] files = System.IO.Directory.GetFiles(UnityEngine.Application.persistentDataPath, "*.jpg");
            foreach (string file in files)
            {
                System.IO.File.Delete(file);
            }
        }

        void MakeDateable(string[] args)
        {
            if (args.Length <= 1)
            {
                error("usage: 'sempai <character_id>' where character_id is a romance option");
            }
            else
            {
                CharacterModel model = AmbitionApp.GetModel<CharacterModel>();
                CharacterVO character = model.GetCharacter(args[1]);
                if (character != null)
                {
                    character.Favor = 100;
                    character.IsDateable = true;
                    character.Acquainted = true;
                    log("{0} is now dateable.", args[1]);
                }
                else
                {
                    error("error: could not find character '{0}'", args[1]);
                }
            }
        }

        void Discover(string[] args)
        {
            if (args.Length > 1)
            {
                CommodityVO location = new CommodityVO()
                {
                    Type = CommodityType.Location,
                    ID = args[1]
                };
                AmbitionApp.Execute<LocationReward, CommodityVO>(location);
            }
            else
            {
                error("usage: 'discover <location_id>' where location_id is a location on the Paris Map");
            }
        }

        void Explore(string[] args)
        {
            if (args.Length > 1)
            {
                LocationVO location = AmbitionApp.Paris.GetLocation(args[1]);
                if (location != null)
                {
                    if (location.IsOneShot)
                    {
                        if (!AmbitionApp.Paris.Completed.Contains(location.ID))
                            AmbitionApp.Paris.Completed.Add(location.ID);
                        AmbitionApp.Paris.Exploration.Remove(location.ID);
                        AmbitionApp.Paris.Rendezvous.Remove(location.ID);
                    }
                    else if (location.IsRendezvous)
                    {
                        AmbitionApp.Paris.Exploration.Remove(location.ID);
                        if (!AmbitionApp.Paris.Rendezvous.Contains(location.ID))
                            AmbitionApp.Paris.Rendezvous.Add(location.ID);
                    }
                    else
                    {
                        if (!AmbitionApp.Paris.Exploration.Contains(location.ID))
                            AmbitionApp.Paris.Exploration.Add(location.ID);
                    }
                }
            }
            else
            {
                error("usage: 'explore <location_id>' where location_id is a location on the Paris Map");
            }
        }

        void Date(string[] args)
        {
            if (AmbitionApp.Game == null || string.IsNullOrEmpty(AmbitionApp.Game.playerID))
            {
                error("Please start a game before setting the date");
            }
            else if (args.Length <= 1 || args[1].Length != 8)
            {
                error("Usage: date <YYYYMMDD>");
            }
            else
            {
                string[] substrings = new string[]
                {
                    args[1].Substring(0, 4),
                    args[1].Substring(4, 2),
                    args[1].Substring(6)
                };
                if (!int.TryParse(substrings[0], out int year)
                    || !int.TryParse(substrings[1], out int month)
                    || !int.TryParse(substrings[2], out int day))
                {
                    error("Usage: date <YYYYMMDD>");
                }
                else
                {
                    CharacterModel characters = AmbitionApp.GetModel<CharacterModel>();
                    CalendarModel calendar = AmbitionApp.Calendar;
                    int currDay = calendar.Day;
                    RendezVO[] rendezs;
                    DateTime date = new DateTime(year <= 0 ? 1789 : year, month, day);
                    calendar.Day = day = date.Subtract(AmbitionApp.Calendar.StartDate).Days;
                    foreach (KeyValuePair<string, CharacterVO> kvp in characters.Characters)
                    {
                        if (kvp.Value.LiaisonDay >= 0 && kvp.Value.LiaisonDay < day)
                        {
                            rendezs = AmbitionApp.Calendar.GetOccasions<RendezVO>(kvp.Value.LiaisonDay);
                            characters.Characters[kvp.Key].LiaisonDay = -1;
                            Array.ForEach(rendezs, r => r.RSVP = RSVP.Declined);
                        }
                    }
                    AmbitionApp.Story.Update(true);
                    AmbitionApp.UFlow.Reset();
                    AmbitionApp.UFlow.Invoke(FlowConsts.DAY_FLOW_CONTROLLER);
                }
            }
        }

        void GenerateParty(string[] args)
        {
            if (AmbitionApp.GetEvent() != null)
            {
                error("Error: Event already scheduled for today");
                return;
            }
            PartyVO party = new PartyVO()
            {
                RSVP = RSVP.Accepted
            };
            party.Day = AmbitionApp.Calendar.Day;
            for (int i = args.Length - 1; i > 0; --i)
            {
                if (int.TryParse(args[i], out int irsvp) && Enum.IsDefined(typeof(RSVP), irsvp))
                {
                    party.RSVP = (RSVP)irsvp;
                }
                else Enum.TryParse<FactionType>(args[i], true, out party.Faction);
            }
            AmbitionApp.Execute<InitPartyCmd, PartyVO>(party);
            log(AmbitionApp.GetModel<LocalizationModel>().GetPartyName(party) + " successfully created.");
        }

        void ResetSteam(string[] args)
        {
            Steamworks.SteamUserStats.ResetAllStats(true);
        }

        void OpenDialog(string[] args)
        {
            AmbitionApp.OpenDialog(args[1]);
        }
    }
}
#endif