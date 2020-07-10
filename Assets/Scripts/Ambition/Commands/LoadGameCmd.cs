using System;
using System.Collections.Generic;
using Core;
using UFlow;
namespace Ambition
{
    public class LoadGameCmd : ICommand<string>
    {
        private static int[] START_INCIDENT = new int[] {2};
        private static int[] PARTY_STATE = new int[] {4};
        private static int[] EVENING_STATE = new int[] {5};
        private static int[] ESTATE_STATE = new int[] {16};
        private static int[] ENTER_ESTATE_STATE = new int[] { 1 };
        private static int[] PARIS_STATE = new int[] {18};
        private static int[] PARTY_OUTFIT = new int[] { 1 }; // Moment == null; Turn = -1
        private static int[] PARTY_INTRO = new int[] { 2 }; // Moment != null; Turn = 0
        private static int[] PARTY_ROOM = new int[] { 12 }; // Moment != null; Room > -1
        private static int[] PARTY_MAP = new int[] { 10 }; // Moment == null; Room < 0
        private static int[] PARTY_OUTTRO = new int[] { 4 }; // Moment != null; Room = -1; Turn > 0
        private static int[] MOMENT = new int[] { 3 }; // Moment != null; Room = -1; Turn > 0

        public void Execute(string savedGameData)
        {
            GameModel game = App.Service<ModelSvc>()?.GetModel<GameModel>();
            if (game == null)
            {
                App.Service<CommandSvc>().Execute<InitGameCmd>();
                game = App.Service<ModelSvc>().GetModel<GameModel>();
            }
            UFlowSvc uflow = AmbitionApp.GetService<UFlowSvc>();
            IncidentModel incidentModel = AmbitionApp.GetModel<IncidentModel>();
            PartyModel partyModel = AmbitionApp.GetModel<PartyModel>();
            AmbitionApp.GetService<ModelSvc>().Restore(savedGameData);
            AmbitionApp.Execute<InitPlayerCmd, string>(game.playerID);

            foreach (KeyValuePair<string, LoadedIncident> loaded in incidentModel.Dependencies)
            {
                if (!incidentModel.Incidents.ContainsKey(loaded.Key))
                {
                    switch (loaded.Value.Type)
                    {
                        case IncidentType.Party:
                            partyModel.LoadParty(loaded.Value.Filepath);
                            break;
                    }
                }
            }

            incidentModel.SetDay(game.Day, false);
            partyModel.SetDay(game.Day);
            uflow.Reset();
            UMachine machine = uflow.Instantiate(FlowConsts.DAY_FLOW_CONTROLLER);
            switch (game.Activity)
            {
                case ActivityType.Estate:
                case ActivityType.Location:
                    if (incidentModel.Moment == null)
                    {
                        machine.RestoreState(ESTATE_STATE, null);
                        machine = uflow.GetMachine(FlowConsts.ESTATE_CONTROLLER);
                        machine.RestoreState(ENTER_ESTATE_STATE, null);
                    }
                    else
                        machine.RestoreState(START_INCIDENT, null);
                    break;
                case ActivityType.Party:
                    machine.RestoreState(PARTY_STATE, null);
                    machine = uflow.GetMachine(FlowConsts.PARTY_CONTROLLER);
                    if (partyModel.Turn < 0)
                    {
                        if (incidentModel.Moment == null)
                            machine.RestoreState(PARTY_OUTFIT, null);
                        else
                            machine.RestoreState(PARTY_INTRO, null);
                    }
                    else if (incidentModel.Moment == null)
                    {
                        machine.RestoreState(PARTY_MAP, null);
                    }
                    else if (partyModel.Room >= 0)
                    {
                        machine.RestoreState(PARTY_ROOM, null);
                    }
                    else machine.RestoreState(PARTY_OUTTRO, null);

                    break;
                case ActivityType.Evening:
                    machine.RestoreState(EVENING_STATE, null);
                    break;
            }
            if (incidentModel.Moment != null)
            {
                machine = uflow.GetMachine(FlowConsts.INCIDENT_CONTROLLER);
                machine.RestoreState(MOMENT, null);
            }

            if (!FMODUnity.RuntimeManager.AnyBankLoading()) AmbitionApp.Execute<FinishLoadGameCmd>();
            else AmbitionApp.RegisterCommand<FinishLoadGameCmd>(AudioMessages.ALL_SOUNDS_LOADED);
        }
    }
}
