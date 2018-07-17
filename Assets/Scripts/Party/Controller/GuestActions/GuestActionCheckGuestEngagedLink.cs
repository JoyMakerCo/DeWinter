using System;
using UFlow;
using UnityEngine;

namespace Ambition
{
    public class GuestActionCheckGuestEngagedLink : ULink
    {
        public override void Initialize()
        {
            AmbitionApp.Subscribe<GuestVO[]>(PartyMessages.GUESTS_SELECTED, HandleGuests);
        }

        public override void Dispose()
        {
            AmbitionApp.Unsubscribe<GuestVO[]>(PartyMessages.GUESTS_SELECTED, HandleGuests);
        }

        private void HandleGuests(GuestVO[] guests)
        {
            UController controller = _machine._uflow.GetController(_machine);
            if (controller !=  null)
            {        
                ConversationModel model = AmbitionApp.GetModel<ConversationModel>();
                GuestVO guest = model.Guests[controller.transform.GetSiblingIndex()];
                if (model.Remark != null
                    && model.Remark.Interest != guest.Dislike
                    && Array.IndexOf(guests, guest) >= 0)
                    Activate();
            }
        }
    }
}
