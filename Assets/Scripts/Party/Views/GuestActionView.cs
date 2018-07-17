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
                HandleTurn();
                AmbitionApp.Subscribe(PartyMessages.START_TURN, HandleTurn);
            }
            else gameObject.SetActive(false);
        }

        private void HandleTurn()
        {
            GuestActionVO action = _guest.Action;
            if (action != null)
            {
                GuestActionIcon icon;
                foreach(ActionMap map in Actions)
                {
                    if (map.ActionType == action.Type)
                    {
                        map.View.SetActive(true);
                        icon = map.View.GetComponent<GuestActionIcon>();
                        if (icon != null) icon.SetAction(action);
                    }
                    else map.View.SetActive(false);
                }
                gameObject.SetActive(true);
            }
            else gameObject.SetActive(false);
        }
    }
}
