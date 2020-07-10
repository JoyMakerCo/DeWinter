using System;
namespace Ambition
{
    public class IncidentReward : Core.ICommand<CommodityVO>
    {
        public void Execute(CommodityVO reward)
        {
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            IncidentModel model = AmbitionApp.GetModel<IncidentModel>();
            if (reward.Value == 1) model.Schedule(reward.ID);
            else model.Schedule(reward.ID, calendar.Day + reward.Value - 1);
        }
    }
}
