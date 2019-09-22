using System;
using UFlow;
namespace Ambition
{
    public class CheckLocationLink : ULink
    {
        override public bool Validate() => AmbitionApp.GetModel<ParisModel>().Location != null;
    }
}
