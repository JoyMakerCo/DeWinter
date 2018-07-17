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
            int amt = action.Values["opinion"];
            Array.ForEach(map.Room.Guests, g=>{g.Opinion+=amt;});
        }
    }
}
