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
            int index = Util.RNG.Generate(1, model.Interests.Length);
            if (guest.Like == model.Interests[index]) index=0;
            guest.Like = model.Interests[index];
            guest.Dislike = model.Interests[(index + 1)%model.Interests.Length];
            AmbitionApp.SendMessage<GuestVO[]>(map.Room.Guests);
        }
    }
}
