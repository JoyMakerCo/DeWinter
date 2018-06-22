using UFlow;
using System;
using System.Linq;

namespace Ambition
{
    public class SelectGuestActionState : UState
    {
        override public void OnEnterState()
        {
            // Using Sibling Index is hacky as fuck, but this should work for our purposes FRN.
            // VERY dependent on the view.
            MapModel map = AmbitionApp.GetModel<MapModel>();
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            UController controller = _machine._uflow.GetController(_machine);
            int index = controller.transform.GetSiblingIndex();
            int [] chart = map.Room.Guests[index].State == GuestState.Charmed
                ? model.CharmedGuestActionChance
                : model.GuestActionChance;

// map.Room.Guests[index].Action = model.GuestActions[0];
// return;            
            if (Util.RNG.Generate(chart[map.Room.Difficulty-1]) > 0)
            {
                map.Room.Guests[index].Action = null;
                return;
            }

            GuestActionVO[] actions = Array.FindAll(model.GuestActions, a=>a.Difficuly <= map.Room.Difficulty);
            int total = actions.Select(a=>a.Chance).Sum();
            int choice = Util.RNG.Generate(total);
            foreach(GuestActionVO action in actions)
                if (choice > action.Chance) choice--;
                else
                {
                    map.Room.Guests[index].Action = action;
                    return;
                }
        }
    }
}
