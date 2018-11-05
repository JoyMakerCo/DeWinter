using UnityEngine;
using System.Collections;

namespace Ambition
{
    public abstract class GuestViewMediator : MonoBehaviour
    {
        protected int _index;
        protected GuestVO _guest;

        public GuestVO Guest => _guest;

        protected void InitGuest()
        {
            ConversationModel model = AmbitionApp.GetModel<ConversationModel>();
            _index = transform.GetSiblingIndex();
            AmbitionApp.Subscribe<GuestVO[]>(HandleGuests);
            AmbitionApp.Subscribe<GuestVO>(HandleGuest);
            if (model.Guests != null) HandleGuests(model.Guests);
            else this.gameObject.SetActive(false);
        }

        protected void Cleanup()
        {
            AmbitionApp.Unsubscribe<GuestVO[]>(HandleGuests);
            AmbitionApp.Unsubscribe<GuestVO>(HandleGuest);
        }

        private void HandleGuests(GuestVO[] guests)
        {
            _guest = (_index < guests.Length) ? guests[_index] : null;
            gameObject.SetActive(_guest != null);
            HandleGuest(_guest);
        }

        protected abstract void HandleGuest(GuestVO guest);
    }
}
