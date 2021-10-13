using System;
namespace Ambition
{
    public class IncidentReward : Core.ICommand<CommodityVO>
    {
        public void Execute(CommodityVO reward)
        {
            if (!string.IsNullOrEmpty(reward.ID))
            {
                IncidentModel story = AmbitionApp.Story;
                IncidentVO incident = story.LoadIncident(reward.ID, IncidentType.Reward);
                if (incident != null)
                {
                    CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
                    if (reward.Value >= 0)
                    {
                        incident.Date = calendar.Today.AddDays(reward.Value);
                    }
                    story.Incidents[reward.ID] = incident;
                    if (incident.IsScheduled)
                        AmbitionApp.SendMessage(CalendarMessages.SCHEDULE, incident);
                }
            }
        }
    }
}
