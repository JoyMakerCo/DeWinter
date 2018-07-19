using System;
using UFlow;

namespace Ambition
{
    public class GuestActionCheckRoundsLink : ULink
    {
        public override bool Validate()
        {
            UController controller = _machine._uflow.GetController(_machine);
            if (controller == null) return false;

            MapModel map = AmbitionApp.GetModel<MapModel>();
            GuestVO guest = map.Room.Guests[controller.transform.GetSiblingIndex()];
            return guest.Action != null && guest.Action.Rounds > 0;
        }
    }
}
