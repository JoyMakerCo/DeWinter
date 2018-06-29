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
            int index = Util.RNG.Generate(1, model.Interests.Length);
UnityEngine.Debug.Log("Guest Action Interest " + controller.transform.GetSiblingIndex().ToString());
            if (guest.Like == model.Interests[index]) index=0;
            guest.Like = model.Interests[index];
            guest.Dislike = model.Interests[(index + 1)%model.Interests.Length];
            guest.Action = Array.Find(model.GuestActions, a=>a.Type == "Interest");
            AmbitionApp.SendMessage<GuestVO[]>(map.Room.Guests);
        }
    }
}
