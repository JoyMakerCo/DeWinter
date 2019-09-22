using System;
namespace Ambition
{
    public class StyleChangeDecision : UFlow.ULink
    {
        public override bool Validate()
        {
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            return calendar.Today >= calendar.NextStyleSwitchDay;
        }
    }
}
