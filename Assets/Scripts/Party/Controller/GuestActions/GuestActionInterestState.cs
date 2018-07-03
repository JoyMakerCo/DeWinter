using System;
using UFlow;

namespace Ambition
{
    public class GuestActionInterestState : UState
    {
        override public void OnEnterState()
        {
            MapModel map = AmbitionApp.GetModel<MapModel>();
            UController controller = _machine._uflow.GetController(_machine);
            GuestVO guest = map.Room.Guests[controller.transform.GetSiblingIndex()];
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            GuestActionVO action = AmbitionApp.Create<string, GuestActionVO>("Interest");
            action.Tags = new string[]{guest.Like, ""};
            int index = Util.RNG.Generate(1, model.Interests.Length);
            if (guest.Like == model.Interests[index]) index=0;
            guest.Like = model.Interests[index];
            guest.Dislike = model.Interests[(index + 1)%model.Interests.Length];
            action.Tags[1] = guest.Like;
            guest.Action = action;
            AmbitionApp.SendMessage<GuestVO>(guest);
        }
    }
}
