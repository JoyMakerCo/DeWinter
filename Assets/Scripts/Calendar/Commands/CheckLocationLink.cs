using System;
using UFlow;
namespace Ambition
{
    public class CheckLocationLink : ULink
    {
        override public bool Validate()
        {
            ParisModel paris = AmbitionApp.GetModel<ParisModel>();
            return paris.Location != null;
        }
    }
}
