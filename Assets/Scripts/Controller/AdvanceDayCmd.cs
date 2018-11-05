using System;
namespace Ambition
{
    public class AdvanceDayCmd : Core.ICommand
    {
        public void Execute()
        {
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            calendar.Today = calendar.DaysFromNow(1);
        }
    }
}
