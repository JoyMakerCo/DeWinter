using System;
namespace Ambition
{
    public class IncidentReward : Core.ICommand<CommodityVO>
    {
        public void Execute(CommodityVO reward)
        {
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            IncidentVO incident = calendar.FindUnscheduled<IncidentVO>(reward.ID);
            if (incident != null)
            {
                calendar.Schedule(incident, reward.Value > 0 || !incident.IsScheduled ? calendar.Today : incident.Date);
            }
#if (UNITY_EDITOR)
            else UnityEngine.Debug.LogWarning(">> WARNING! The requested incident " + reward.ID + " could not be found!");
#endif
        }
    }
}
