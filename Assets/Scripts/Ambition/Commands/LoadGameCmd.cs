using System;
using System.Collections.Generic;
using Core;
using UFlow;
using UnityEngine;
namespace Ambition
{
    public class LoadGameCmd : ICommand<string>
    {
        // Day Flow
        private static string MORNING_INCIDENT = "MorningIncident";
        private static string PARTY_STATE = "Party";
        private static string PARIS_STATE = "Paris";
        private static string EVENING_STATE = "Evening";
        private static string ESTATE_STATE = "Estate";
        private static string RENDEZVOUS_STATE = "Rendezvous";

        // Location Controller
        private static string PARIS_INCIDENT = "Location Incident";
        private static string PARIS_SCENE = "Location Scene";

        // Party Controller
        private static string PARTY_INTRO = "Intro"; // Moment != null; Turn < 0
        private static string PARTY_ROOM = "Conversation"; // Moment != null; Turn >= 0
        private static string PARTY_MAP = "Map"; // Moment == null;
        private static string PARTY_OUTTRO = "Exit Incident"; // Moment != null; Room = -1; Turn > 0
        private static string AFTER_PARTY = "AfterPartyInput"; // Moment != null; Room = -1; Turn > 0

        // Incident Controller
        private static string MOMENT = "OptionInput"; // Any Incident

        // Estate Controller
        private static string ESTATE_ENTER = "ShowEstate";

        // Rendezvous Controller
        private static string RENDEZVOUS_INCIDENT = "RendezvousIncident";

        public void Execute(string savedGameData)
        {
            GameModel game = AmbitionApp.Game;
            if (!game.Initialized) AmbitionApp.Execute<InitGameCmd>();
            UFlowSvc uflow = AmbitionApp.GetService<UFlowSvc>();
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            IncidentModel story = AmbitionApp.Story;
            ParisModel paris = AmbitionApp.Paris;
            PartyModel partyModel = AmbitionApp.GetModel<PartyModel>();
            AmbitionApp.GetService<ModelSvc>().Restore(savedGameData);
            PlayerConfig config = Resources.Load<PlayerConfig>(Filepath.PLAYERS + game.playerID);
            AmbitionApp.Execute<RestorePlayerCmd, PlayerConfig>(config);
            LocationVO location = paris.GetLocation(paris.Location);

            AmbitionApp.CloseAllDialogs();
            AmbitionApp.SendMessage(AudioMessages.STOP_AMBIENT);
            AmbitionApp.SendMessage(AudioMessages.STOP_MUSIC);

            uflow.Reset();
            story.RestoreIncident();
            foreach(string tutorialID in game.Tutorials)
            {
                AmbitionApp.Execute<TutorialReward, CommodityVO>(new CommodityVO()
                {
                    Type = CommodityType.Tutorial,
                    ID = tutorialID
                });
            }

            UMachine flow = calendar.Day == 0
                ? uflow.Instantiate(FlowConsts.GAME_CONTROLLER)
                : uflow.Instantiate(FlowConsts.DAY_FLOW_CONTROLLER);
            string sceneID = null;
            switch (game.Activity)
            {
                case ActivityType.Estate:
                    if (story.Moment == null)
                    {
                        flow = RestoreEstate(flow, out sceneID);
                    }
                    else flow = Restore(flow, MORNING_INCIDENT);
                    break;
                case ActivityType.Party:
                    flow = Restore(flow, PARTY_STATE);
                    if (story.Moment == null)
                    {
                        if (partyModel.TurnsLeft > 0)
                        {
                            Restore(flow, PARTY_MAP);
                            sceneID = SceneConsts.MAP_SCENE;
                        }
                        else
                        {
                            Restore(flow, AFTER_PARTY);
                            sceneID = SceneConsts.AFTER_PARTY_SCENE;
                        }
                    }
                    else if (partyModel.Turn < 0)
                        flow = Restore(flow, PARTY_INTRO);
                    else if (partyModel.TurnsLeft > 0)
                        flow = Restore(flow, PARTY_ROOM);
                    else
                        flow = Restore(flow, PARTY_OUTTRO);
                    break;
                case ActivityType.Evening:
                    flow = Restore(flow, EVENING_STATE);
                    break;
                case ActivityType.Paris:
                    flow = Restore(flow, PARIS_STATE);
                    if (story.Moment != null)
                    {
                        flow = Restore(flow, PARIS_INCIDENT);
                    }
                    else
                    {
                        sceneID = location?.SceneID ?? SceneConsts.PARIS_SCENE;
                        Restore(flow, sceneID == SceneConsts.PARIS_SCENE ? PARIS_STATE : PARIS_SCENE);
                    }
                    break;
                case ActivityType.Rendezvous:
                    flow = Restore(flow, RENDEZVOUS_STATE);
                    flow = Restore(flow, RENDEZVOUS_INCIDENT);
                    break;
            }
            if (story.Moment != null)
            {
                Restore(flow, MOMENT);
                sceneID = SceneConsts.INCIDENT_SCENE;
            }
            if (!string.IsNullOrEmpty(sceneID))
            {
                AmbitionApp.SendMessage(GameMessages.LOAD_SCENE, sceneID);
            }
            uflow.Execute();
            AmbitionApp.SendMessage(GameMessages.GAME_LOADED);
        }

        private UMachine RestoreEstate(UMachine flow, out string sceneID)
        {
            flow = Restore(flow, ESTATE_STATE);
            Restore(flow, ESTATE_ENTER);
            sceneID = SceneConsts.ESTATE_SCENE;
            return flow;
        }

        private UMachine Restore(UMachine flow, string state)
        {
            flow.RestoreStates(new string[]{state}, new Dictionary<int, UMachine>());
            return flow.GetState(state) as UMachine;
        }
    }
}
