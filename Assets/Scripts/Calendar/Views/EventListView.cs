using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class EventListView : MonoBehaviour
    {
        // CONSTANTS
        private const int TIME_SPAN = 31; // One Month

        // PUBLIC DATA
        public EventListItemView ListItem;

        // PRIVATE DATA
        private List<EventListItemView> _pool;

        // PUBLIC METHODS

        // PRIVATE METHODS
        private void Awake()
        {
            if (_pool == null) _pool = new List<EventListItemView>() { ListItem };
        }

        void OnEnable()
        {
            AmbitionApp.Subscribe<PartyVO>(HandleParty);
            AmbitionApp.Subscribe<RendezVO>(HandleRendez);
            AmbitionApp.Calendar.Observe(HandleRefresh);
        }

        void HandleRefresh(CalendarModel calendar)
        {
            PartyVO[] parties;
            RendezVO[] rendezs;
            bool showDate;
            int i = 0;
            int span = 0;
            for (int day = calendar.Day; span < TIME_SPAN; ++day)
            {
                showDate = true;
                parties = calendar.GetOccasions<PartyVO>(day);
                rendezs = calendar.GetOccasions<RendezVO>(day);
                if (parties.Length == 0 && rendezs.Length == 0) ++span;
                else
                {
                    span = 0;
                    foreach (PartyVO party in parties)
                    {
                        Instantiate(party, i, showDate);
                        i++;
                        showDate = false;
                    }
                    foreach(RendezVO rendez in rendezs)
                    {
                        Instantiate(rendez, i, showDate);
                        i++;
                        showDate = false;
                    }
                }
            }
            while (i < _pool.Count)
            {
                _pool[i++].gameObject.SetActive(false);
            }
        }

        void OnDisable()
        {
            AmbitionApp.Unsubscribe<PartyVO>(HandleParty);
            AmbitionApp.Unsubscribe<RendezVO>(HandleRendez);
            AmbitionApp.Calendar.Unobserve(HandleRefresh);
        }

        private void HandleRendez(RendezVO rendez)
        {
            EventListItemView item = _pool.Find(i => i.Event == rendez);
            if (item != null) item.Event = rendez;
        }

        private void HandleParty(PartyVO party)
        {
            EventListItemView item = _pool.Find(i => i.Event == party);
            if (item != null) item.Event = party;
        }

        private void Instantiate(CalendarEvent e, int index, bool showDate)
        {
            EventListItemView item;
            if (index < _pool.Count)
            {
                item = _pool[index];
            }
            else
            {
                GameObject obj = Instantiate(ListItem.gameObject, ListItem.transform.parent);
                item = obj.GetComponent<EventListItemView>();
                _pool.Add(item);
            }
            item.Event = e;
            item.gameObject.SetActive(true);
            item.DateText.enabled = showDate;
        }
    }
}
