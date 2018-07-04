using System;
using UFlow;

namespace Ambition
{
    public class GuestActionToastState : UState
    {
        override public void OnEnterState()
        {
            MapModel map = AmbitionApp.GetModel<MapModel>();
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            GuestActionVO action = Array.Find(model.GuestActions, a=>a.Type == "Toast");
            foreach (GuestVO guest in map.Room.Guests)
            {
                guest.Opinion += action.Rewards["opinion"];
                if (guest.Opinion > 100) guest.Opinion = 100;
            }
        }
    }
}
