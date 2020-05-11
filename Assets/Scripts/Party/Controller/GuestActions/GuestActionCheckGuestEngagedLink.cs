using System;
using UFlow;
using UnityEngine;

namespace Ambition
{
    public class GuestActionCheckGuestEngagedLink : ULink, Util.IInitializable, IDisposable
    {
        public void Initialize()
        {
            AmbitionApp.Subscribe<CharacterVO>(PartyMessages.GUEST_SELECTED, HandleGuest);
        }

        public void Dispose()
        {
            AmbitionApp.Unsubscribe<CharacterVO>(PartyMessages.GUEST_SELECTED, HandleGuest);
        }

        private void HandleGuest(CharacterVO guest)
        {
/*
            UController controller = _machine._UFlow.GetController(_machine);
            if (guest != null && controller !=  null)
            {        
                ConversationModel model = AmbitionApp.GetModel<ConversationModel>();
                int index = controller.transform.GetSiblingIndex();
                if (model.Remark != null
                    //&& model.Remark.Interest != guest.Dislike
                    && guest == model.Guests[index])
                    Activate();
            }
*/
        }
    }
}
