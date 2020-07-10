﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class LeaveEstateBtn : MonoBehaviour
	{
	    private Text _text;
        private DateTime _today;
        private PartyVO _party;

	    void Awake()
	    {
            CalendarModel model = AmbitionApp.GetModel<CalendarModel>();
            _today = model.Today;
            _text = this.GetComponentInChildren<Text>();
            _party = AmbitionApp.GetModel<PartyModel>().GetParty();
            UpdateParty(_party);
            AmbitionApp.Subscribe<PartyVO>(HandleParty);
        }

        void OnDestroy() => AmbitionApp.Unsubscribe<PartyVO>(HandleParty);

        private void HandleParty(PartyVO party)
        {
            if (party != null && party.Date == _today)
            {
                if (party.RSVP == RSVP.Accepted || party.RSVP == RSVP.Required)
                    UpdateParty(party);
                else if (_party == party && party.RSVP == RSVP.Declined)
                    UpdateParty(null);
            }
        }

        private void UpdateParty(PartyVO party)
        {
            _party = (party?.Attending ?? false ) ? party : null;
            _text.text = AmbitionApp.Localize("calendar.btn." + (_party == null ? "paris" : "party"));
        }

        public void LeaveEstate() => AmbitionApp.SendMessage(EstateMessages.LEAVE_ESTATE);
    }
}
