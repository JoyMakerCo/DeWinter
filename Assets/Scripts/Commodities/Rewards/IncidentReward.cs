using System;
namespace Ambition
{
    public class IncidentReward : Core.ICommand<CommodityVO>
    {
        public void Execute(CommodityVO reward)
        {
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            IncidentVO incident = calendar.FindIncident(reward.ID);
            if (incident != null)
            {
                if (reward.Value > 0) calendar.Incident = incident;
                else calendar.Schedule(incident);
            }
            else UnityEngine.Debug.LogWarning(">> WARNING! The requested incident " + reward.ID + " could not be found!");
        }
    }
}
