using System;
using UFlow;

namespace Ambition
{
    public class StyleChangeDecision : ULink
    {
        public override bool Validate()
        {
            return AmbitionApp.GetModel<CalendarModel>().Today >= AmbitionApp.GetModel<GameModel>().NextStyleSwitchDay;
        }
    }
}
