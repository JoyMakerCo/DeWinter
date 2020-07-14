using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class EventListView : MonoBehaviour
    {
        public Transform ListContent;
        public Text[] Headers;

        public GameObject ListItemPrefab;

        private CalendarModel _calendar;
        private PartyModel _model;
        private List<GameObject> _pool = new List<GameObject>();

        void OnEnable()
        {
            _calendar = AmbitionApp.GetModel<CalendarModel>();
            _model = AmbitionApp.GetModel<PartyModel>();
            AmbitionApp.Subscribe<PartyVO>(HandleParty);
            UpdateParties();
        }

        void OnDisable()
        {
            AmbitionApp.Unsubscribe<PartyVO>(HandleParty);
        }

        private void HandleParty(PartyVO party)
        {
            if (party != null && party.Date >= _calendar.Today)
                UpdateParties();
        }

        private void UpdateParties()
        {
            List<List<PartyVO>> types = new List<List<PartyVO>>()
            {
                new List<PartyVO>(),
                new List<PartyVO>(),
                new List<PartyVO>(),
                new List<PartyVO>()
            };
            Vector2 xform = Headers[0].rectTransform.anchoredPosition;
            GameObject item;
            int itemCount = 0;
            bool show;
            foreach(PartyVO party in _model.Parties.Values)
            {
                if (party.Date >= _calendar.Today)
                {
                    switch(party.RSVP)
                    {
                        case RSVP.Accepted:
                        case RSVP.Required:
                            types[1].Add(party);
                            break;
                        case RSVP.Declined:
                            types[2].Add(party);
                            break;
                        case RSVP.New:
                            types[3].Add(party);
                            break;
                    }
                    if (++itemCount > _pool.Count)
                    {
                        item = Instantiate(ListItemPrefab, ListContent.transform);
                        _pool.Add(item);
                    }
                }
            }

            itemCount = 0;
            for (int i = 0; i < types.Count; i++)
            {
                Headers[i].rectTransform.anchoredPosition = xform;
                show = types[i].Count > 0;
                Headers[i].enabled = show;
                if (show)
                {
                    xform.y -= Headers[i].GetComponent<RectTransform>().rect.height;
                    foreach (PartyVO party in types[i])
                    {
                        item = _pool[itemCount];
                        item.GetComponent<EventListItemView>().Party = party;
                        item.GetComponent<RectTransform>().anchoredPosition = xform;
                        xform.y -= item.GetComponent<RectTransform>().rect.height;
                        itemCount++;
                    }
                }
            }
        }
    }
}
