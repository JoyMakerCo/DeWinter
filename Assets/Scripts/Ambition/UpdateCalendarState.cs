using System;
using System.Collections.Generic;
using UFlow;
namespace Ambition
{
    public class UpdateCalendarState : UState, Core.IState
    {
        public override void OnEnter()
        {
            CalendarModel calendar = AmbitionApp.Calendar;
            AmbitionApp.GetModel<LocalizationModel>().SetDate(calendar.Today);
            GameModel model = AmbitionApp.GetModel<GameModel>();
            AmbitionApp.Story.UpdateIncident();
            AmbitionApp.SendMessage(CalendarMessages.UPDATE_CALENDAR, calendar);
        }
    }
}
