using System;
using System.Collections.Generic;
using Core;
namespace Ambition
{
    public class NewGameCmd : Core.ICommand<string>
    {
        public void Execute(string playerID)
        {
            GameModel game = App.Service<ModelSvc>()?.GetModel<GameModel>();
            if (game == null)
            {
                App.Service<CommandSvc>().Execute<InitGameCmd>();
                game = App.Service<ModelSvc>().GetModel<GameModel>();
            }
            IncidentModel incidentModel = AmbitionApp.GetModel<IncidentModel>();
            AmbitionApp.Execute<InitPlayerCmd, string>(playerID);
            IncidentVO[] incidents = incidentModel.GetIncidents(IncidentType.Timeline);
            foreach (IncidentVO incident in incidents)
            {
                if (incident.IsScheduled)
                {
                    incidentModel.Schedule(incident, incident.Date.Subtract(game.StartDate).Days);
                }
            }

            if (!FMODUnity.RuntimeManager.AnyBankLoading()) AmbitionApp.Execute<FinishLoadingCmd>();
            else AmbitionApp.RegisterCommand<FinishLoadingCmd>(AudioMessages.ALL_SOUNDS_LOADED);
        }
    }
}
