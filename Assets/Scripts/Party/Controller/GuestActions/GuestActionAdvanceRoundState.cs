using UFlow;
using UnityEngine;

namespace Ambition
{
    public class GuestActionAdvanceRoundState : UState
    {
        override public void OnEnterState()
        {
            UController controller = _machine._uflow.GetController(_machine);
            if (controller !=  null)
            {                
                MapModel map = AmbitionApp.GetModel<MapModel>();
                GuestVO guest = map.Room.Guests[controller.transform.GetSiblingIndex()];
                if (guest.Action != null) guest.Action.Rounds--;
            }
        }
    }
}
