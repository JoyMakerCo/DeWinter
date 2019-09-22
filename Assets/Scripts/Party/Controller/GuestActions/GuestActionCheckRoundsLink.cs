using System;
using UFlow;

namespace Ambition
{
    public class GuestActionCheckRoundsLink : ULink
    {
        public override bool Validate()
            => false;
/*        {
            UController controller = _machine._UFlow.GetController(_machine);
            if (controller == null) return false;

            MapModel map = AmbitionApp.GetModel<MapModel>();
            CharacterVO guest = map.Room.Value.Guests[controller.transform.GetSiblingIndex()];
            return guest.Action != null && guest.Action.Rounds > 0;
        }
*/
    }
}
