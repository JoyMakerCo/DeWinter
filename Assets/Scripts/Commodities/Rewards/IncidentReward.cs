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
                var incidentDate = calendar.Today;
                // value of 0 or less is default scheduling:
                // - scheduled date for scheduled incidents
                // - today for unscheduled incidents
                if (reward.Value <= 0)
                {
                    if (incident.IsScheduled)
                    {
                        incidentDate = incident.Date;
                    }
                }
                else
                {
                    // value of 1 or more is Nth from today (i.e. 1 = today, 2 = tomorrow...)
                    incidentDate = calendar.Today.AddDays( reward.Value-1 );
                }

                calendar.Schedule(incident, incidentDate);
            }
#if (UNITY_EDITOR)
            else UnityEngine.Debug.LogWarning(">> WARNING! The requested incident " + reward.ID + " could not be found!");
#endif
        }
    }
}
