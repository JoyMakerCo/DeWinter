﻿using UFlow;
namespace Ambition
{
    public class CheckLocationLink : ULink
    {
        public override bool Validate()
        {
            ParisModel paris = AmbitionApp.GetModel<ParisModel>();
            return !string.IsNullOrEmpty(paris.LocationID ?? paris.Location?.ID);
        }
    }
}
