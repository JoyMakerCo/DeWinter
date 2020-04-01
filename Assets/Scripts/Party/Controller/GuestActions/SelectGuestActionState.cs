using UFlow;
using System;
using System.Linq;

namespace Ambition
{
    public class SelectGuestActionState : UState
    {
        public override void OnEnterState()
        {
            // Using Sibling Index is hacky as fuck, but this should work for our purposes FRN.
            // VERY dependent on the view.
/*DEPRECATED         MapModel map = AmbitionApp.GetModel<MapModel>();
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            UController controller = _UFlow.GetController(_Machine);
            int index = controller.transform.GetSiblingIndex();
            GuestVO guest = index < map.Room.Guests.Length ? map.Room.Guests[index] : null;
            if (guest != null && guest.State != GuestState.Offended)
            {
                int[] chart = guest.State == GuestState.Charmed
                    ? model.CharmedGuestActionChance
                    : model.GuestActionChance;
                if (Util.RNG.Generate(chart[map.Room.Difficulty - 1]) > 0)
                {
                    guest.Action = null;
                }
                else
                {
                    GuestActionFactory factory = (GuestActionFactory)AmbitionApp.GetFactory<string, GuestActionVO>();
                    GuestActionVO[] actions = factory.Actions.Values.ToArray();
                    actions = Array.FindAll(actions, a => a.Difficulty <= map.Room.Difficulty);
                    int choice = actions.Select(a => a.Chance).Sum();
                    int i = 0;
                    for (choice = Util.RNG.Generate(choice); actions[i].Chance <= choice; i++)
                        choice -= actions[i].Chance;
                    guest.Action = actions[i];
                }
            }
            else
            {
                controller.gameObject.SetActive(false);
            }
   */     }
    }
}
