using System;
using Core;
using UFlow;

namespace Ambition
{
    public class ValidateRoomLink : ULink
    {
        public override bool Validate() => true; //AmbitionApp.GetModel<MapModel>().Room.Value?.Cleared ?? false;
    }
}
