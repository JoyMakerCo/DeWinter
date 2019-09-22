using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        private List<GameObject> _pool = new List<GameObject>();

        void OnEnable()
        {
            _calendar = AmbitionApp.GetModel<CalendarModel>();
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
            List<PartyVO[]> types = new List<PartyVO[]>();

            Vector2 xform = Headers[0].rectTransform.anchoredPosition;
            GameObject item;
            int itemCount = 0;
            bool show;
            IEnumerable<PartyVO> parties = _calendar.GetEvents<PartyVO>().Where(p => p.Date >= _calendar.Today);
            int count = parties.Count();
            types.Add(new PartyVO[0]);
            types.Add(parties.Where(p => p.Attending).ToArray());
            types.Add(parties.Where(p => p.RSVP == RSVP.Declined).ToArray());
            types.Add(parties.Where(p => p.RSVP == RSVP.New).ToArray());

            for (GameObject obj; _pool.Count < count; _pool.Add(obj))
            {
                obj = Instantiate(ListItemPrefab, ListContent.transform);
            }

            while (_pool.Count > count)
            {
                Destroy(_pool[0]);
                _pool.RemoveAt(0);
            }

            for (int i = 0; i < types.Count; i++)
            {
                Headers[i].rectTransform.anchoredPosition = xform;
                show = types[i].Length > 0;
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
