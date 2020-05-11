using System;
using System.Linq;
using UFlow;

namespace Ambition
{
    public class GuestActionSelectLeadReward : UState
    {
        public override void OnEnterState()
        {
            //UController controller = _Machine._UFlow.GetController(_Machine);
            //if (controller != null)
            {
                MapModel map = AmbitionApp.GetModel<MapModel>();
                //CharacterVO guest = map.Room.Value.Guests[controller.transform.GetSiblingIndex()];
                //string reward = Util.RNG.TakeRandom(guest.Action.Tags);
                //guest.Action.Tags = new string[] { reward };
                //guest.Action.Rounds = guest.Action.Values[reward];
            }
        }
    }
}
