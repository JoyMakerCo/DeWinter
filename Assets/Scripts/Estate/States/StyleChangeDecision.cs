﻿using System;
using UFlow;

namespace Ambition
{
    public class StyleChangeDecision : ULink
    {
        public override bool Validate()
        {
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            return calendar.Today >= calendar.NextStyleSwitchDay;
        }
    }
}
