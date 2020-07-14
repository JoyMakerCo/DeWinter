using System;
using System.Collections.Generic;
using Core;
namespace Ambition
{
    public class NewGameCmd : Core.ICommand<string>
    {
        public void Execute(string playerID)
        {
            if (App.Service<ModelSvc>()?.GetModel<GameModel>() == null)
            {
                App.Service<CommandSvc>().Execute<InitGameCmd>();
            }
            IncidentModel incidentModel = AmbitionApp.GetModel<IncidentModel>();
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            AmbitionApp.Execute<InitPlayerCmd, string>(playerID);
            List<IncidentVO> incidents = new List<IncidentVO>(incidentModel.Incidents.Values);
            foreach (IncidentVO incident in incidents)
            {
                if (incident.IsScheduled)
                {
                    incidentModel.Schedule(incident, incident.Date.Subtract(calendar.StartDate).Days);
                }
            }

            if (!FMODUnity.RuntimeManager.AnyBankLoading()) AmbitionApp.Execute<FinishLoadingCmd>();
            else AmbitionApp.RegisterCommand<FinishLoadingCmd>(AudioMessages.ALL_SOUNDS_LOADED);
        }
    }
}
