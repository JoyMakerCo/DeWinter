using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Ambition
{
	public class LeaveEstateBtn : MonoBehaviour
	{
	    private Text _text;
        private PartyVO _party;

	    void Awake()
	    {
			_text = this.GetComponentInChildren<Text>();
			AmbitionApp.Subscribe<PartyVO>(HandleParty);

            HandleParty(AmbitionApp.GetModel<PartyModel>().Party);
	    }

	    void OnDestroy()
	    {
			AmbitionApp.Unsubscribe<PartyVO>(HandleParty);
	    }

		private void HandleParty(PartyVO party)
		{
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            if (party != null && party.Date == calendar.Today)
            {
                if (party.RSVP == RSVP.Accepted) _party = party;
                else
                {
                    List<ICalendarEvent> events;
                    _party = calendar.Timeline.TryGetValue(calendar.Today, out events)
                                     ? events.OfType<PartyVO>().FirstOrDefault(p => p.RSVP == RSVP.Accepted)
                                     : null;
                }
                _text.text = _party != null ? "Go to the Party!" : "Explore Paris";
            }
		}

        public void LeaveEstate()
        {
            print("Trying to leave the estate!");
            AmbitionApp.SendMessage(EstateMessages.LEAVE_ESTATE);
        }
    }
}
