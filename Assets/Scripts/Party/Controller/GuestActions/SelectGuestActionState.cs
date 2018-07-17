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
            if (Util.RNG.Generate(chart[map.Room.Difficulty-1]) > 0)
            {
                map.Room.Guests[index].Action = null;
            }
            else
            {
                GuestActionFactory factory = (GuestActionFactory)AmbitionApp.GetFactory<string, GuestActionVO>();
                GuestActionVO[] actions = factory.Actions.Values.ToArray();
                actions = Array.FindAll(actions, a=>a.Difficulty <= map.Room.Difficulty);
                int choice = actions.Select(a=>a.Chance).Sum();
                int i=0;                
                for (choice = Util.RNG.Generate(choice); actions[i].Chance<=choice; i++)
                    choice -= actions[i].Chance;
                map.Room.Guests[index].Action = actions[i];
            }
        }
    }
}
