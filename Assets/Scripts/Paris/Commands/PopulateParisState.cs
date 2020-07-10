using System;
using System.Collections.Generic;
using Util;

namespace Ambition
{
    public class PopulateParisState : UFlow.UState
    {
        public override void OnEnterState()
        {
            ParisModel model = AmbitionApp.GetModel<ParisModel>();
            model.Locations.ForEach(l => AmbitionApp.SendMessage(ParisMessages.SHOW_LOCATION, l));
            if (model.Daily != null)
            {
                Array.ForEach(model.Daily, l => AmbitionApp.SendMessage(ParisMessages.SHOW_LOCATION, l));
            }
        }
    }
}
