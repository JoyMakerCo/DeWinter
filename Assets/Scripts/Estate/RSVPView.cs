using System;
using System.Collections.Generic;
using UnityEngine;
using Dialog;

namespace Ambition
{
    public class RSVPView : DialogView<CalendarEvent[]>, ISubmitHandler
    {
        public GameObject PartyInvitationPrefab;
        public GameObject RendezvousInvitationPrefab;
        public Transform DialogParent;

        private CalendarEvent[] _events;
        private CalendarEvent _selection;

        public override void OnOpen(CalendarEvent[] events)
        {
            Slim(events, RSVP.Declined);
            Slim(events, RSVP.New);
            _events = events;
            _selection = Array.Exists(_events, e=>e.IsAttending)
                ? null
                : Array.Find(_events, e => e.RSVP == RSVP.New);
            Array.ForEach(_events, Open);
        }

        public override void OnClose()
        {
            AmbitionApp.SendMessage(GameMessages.DIALOG_CLOSED, ID);
        }

        public void Submit()
        {
            if (_selection != null)
            {
                AmbitionApp.SendMessage(PartyMessages.ACCEPT_INVITATION, _selection);
            }
        }
        public void Cancel() => Close();

        private void Open(CalendarEvent evnt)
        {
            Open(evnt as PartyVO);
            Open(evnt as RendezVO);
        }

        private void Open(PartyVO party)
        {
            if (party != null)
            {
                GameObject obj = Instantiate(PartyInvitationPrefab, DialogParent);
                RSVPDialog dlog = obj.GetComponent<RSVPDialog>();
                dlog.Initialize(party);
            }
        }

        private void Open(RendezVO rendez)
        {
            if (rendez != null)
            {
                GameObject obj = Instantiate(RendezvousInvitationPrefab, DialogParent);
                RendezvousDialog dlog = obj.GetComponent<RendezvousDialog>();
                dlog.Initialize(rendez);
            }
        }

        private CalendarEvent[] Slim(CalendarEvent[] events, RSVP rsvp)
        {
            if (events.Length <= 2) return events;

            List<CalendarEvent> eligible = new List<CalendarEvent>(events);
            int count = eligible.Count;
            int i = 0;
            eligible.Sort((a, b) => a.Created.CompareTo(b.Created));
            while (count > 2 && i < count)
            {
                if (eligible[i].RSVP == rsvp)
                {
                    eligible.RemoveAt(i);
                    --count;
                }
                else ++i;
            }
            return eligible.ToArray();
        }
    }
}
