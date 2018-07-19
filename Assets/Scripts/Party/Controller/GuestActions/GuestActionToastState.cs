using System;
using UFlow;

namespace Ambition
{
    public class GuestActionToastState : UState
    {
        override public void OnEnterState()
        {
            UController controller = _machine._uflow.GetController(_machine);
            if (controller != null)
            {
                ConversationModel model = AmbitionApp.GetModel<ConversationModel>();
                GuestVO guest = model.Guests[controller.transform.GetSiblingIndex()];
                if (guest.Action != null && guest.Action.Values != null)
                {
                    int reward;
                    int others;
                    guest.Action.Values.TryGetValue("guest", out reward);
                    guest.Action.Values.TryGetValue("others", out others);
                    guest.Opinion += reward - others;
                    Array.ForEach(model.Guests, g => g.Opinion += others);
                }
            }
        }
    }
}
