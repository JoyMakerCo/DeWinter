using System;
using UFlow;

namespace Ambition
{
    public class GuestActionInterestState : UState
    {
        public override void OnEnterState()
        {
/*            MapModel map = AmbitionApp.GetModel<MapModel>();
            UController controller = _Machine._UFlow.GetController(_Machine);
            CharacterVO guest = map.Room.Value.Guests[controller.transform.GetSiblingIndex()];
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            GuestActionVO action = AmbitionApp.Create<string, GuestActionVO>("Interest");

            //action.Tags = new string[]{guest.Like, ""};
            //int index = Util.RNG.Generate(1, model.Interests.Length);
            //if (guest.Like == model.Interests[index]) index=0;
            //guest.Like = model.Interests[index];
            //guest.Dislike = model.Interests[(index + 1)%model.Interests.Length];
            //action.Tags[1] = guest.Like;
            //guest.Action = action;
            AmbitionApp.SendMessage(guest);
 */       }
    }
}
