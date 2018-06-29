using System;
using UnityEngine;

namespace Ambition
{
    public class GuestActionView : MonoBehaviour
    {
        public ActionMap[] Actions;
        private GuestVO _guest;

        [Serializable]
        public struct ActionMap
        {
            public string ActionType;
            public GameObject View;
        }

        void Awake()
        {
            AmbitionApp.Subscribe<GuestVO[]>(HandleGuests);
            gameObject.SetActive(false);
        }

        void OnDestroy()
        {
            AmbitionApp.Unsubscribe<GuestVO[]>(HandleGuests);
            AmbitionApp.Unsubscribe(PartyMessages.START_TURN, HandleTurn);
        }

        private void HandleGuests(GuestVO [] guests)
        {
            int index = transform.GetSiblingIndex();
            AmbitionApp.Unsubscribe(PartyMessages.START_TURN, HandleTurn);
            _guest = (index < guests.Length) ? guests[index] : null;
            if (_guest != null)
            {
                gameObject.SetActive(_guest.Action != null);
                AmbitionApp.Subscribe(PartyMessages.START_TURN, HandleTurn);
                HandleTurn();
            }
            else gameObject.SetActive(false);
        }

        private void HandleTurn()
        {
            GuestActionVO action = _guest.Action;
            if (action != null)
            {
Debug.Log(action.Type + _guest.Name);
                GuestActionIconView icon;
                foreach(ActionMap map in Actions)
                {
                    map.View.SetActive(action != null && map.ActionType == action.Type);
                    icon = map.View.GetComponent<GuestActionIconView>();
                    if (icon != null) icon.Action = action;
                }
            }
            gameObject.SetActive(action != null);
        }
    }
}
