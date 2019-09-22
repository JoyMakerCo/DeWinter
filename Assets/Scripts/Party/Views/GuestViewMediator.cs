using UnityEngine;
using System.Collections;

namespace Ambition
{
    public abstract class GuestViewMediator : MonoBehaviour
    {
        protected int _index;
        protected CharacterVO _guest;

        public CharacterVO Guest => _guest;

        protected void InitGuest()
        {
            _index = transform.GetSiblingIndex();
            AmbitionApp.Subscribe<CharacterVO[]>(HandleGuests);
            AmbitionApp.Subscribe<CharacterVO>(HandleGuest);
            this.gameObject.SetActive(false);
        }

        protected void Cleanup()
        {
            AmbitionApp.Unsubscribe<CharacterVO[]>(HandleGuests);
            AmbitionApp.Unsubscribe<CharacterVO>(HandleGuest);
        }

        private void HandleGuests(CharacterVO[] guests)
        {
            _guest = (_index < guests.Length) ? guests[_index] : null;
            gameObject.SetActive(_guest != null);
            HandleGuest(_guest);
        }

        protected abstract void HandleGuest(CharacterVO guest);
    }
}
