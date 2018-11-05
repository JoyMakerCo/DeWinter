using System;
using UFlow;
using UnityEngine;

namespace Ambition
{
    public class GuestActionCheckGuestEngagedLink : ULink
    {
        public override void Initialize()
        {
            AmbitionApp.Subscribe<GuestVO>(PartyMessages.GUEST_SELECTED, HandleGuest);
        }

        public override void Dispose()
        {
            AmbitionApp.Unsubscribe<GuestVO>(PartyMessages.GUEST_SELECTED, HandleGuest);
        }

        private void HandleGuest(GuestVO guest)
        {
            UController controller = _machine._uflow.GetController(_machine);
            if (guest != null && controller !=  null)
            {        
                ConversationModel model = AmbitionApp.GetModel<ConversationModel>();
                int index = controller.transform.GetSiblingIndex();
                if (model.Remark != null
                    && model.Remark.Interest != guest.Dislike
                    && guest == model.Guests[index])
                    Activate();
            }
        }
    }
}
