using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Ambition;

namespace Ambition
{
	public class CalendarButton : MonoBehaviour
	{
	    public Image currentDayOutline;
	    public Text dateText;
	    public Image pastDayXImage;

		public SpriteConfig CalendarSpriteConfig;

	    //Party Indicators
	    public Image Party1Icon;
	    public Image NewParty1Icon;
	    public Image Party1RSVPIcon;

		public Image Party2Icon;
	    public Image NewParty2Icon;
	    public Image Party2RSVPIcon;

	    private Image myBlockImage;
	    private Color defaultColor;
		private DateTime _day;
		private Button _btn;
		private List<PartyVO> _parties;

		public void SetDay(DateTime day, DateTime today, DateTime currentMonth)
		{
			if (day != _day)
			{
				if (_parties != null) _parties.Clear();
				Party1Icon.enabled = false;
				NewParty1Icon.enabled = false;
				Party1RSVPIcon.enabled = false;
				Party2Icon.enabled = false;
				NewParty2Icon.enabled = false;
				Party2RSVPIcon.enabled = false;
			}
			
			_day = day;

			myBlockImage.color = (_day.Month == currentMonth.Month)
				? defaultColor
				: Color.Lerp(defaultColor, Color.black, .5f);
			dateText.text = _day.Day.ToString();

			bool isToday = (_day == today);
			currentDayOutline.enabled = isToday;
			// if (isToday) this.transform.SetAsLastSibling();

			_btn.interactable = (_day >= today);
			pastDayXImage.enabled = !_btn.interactable;
		}

		public DateTime Date
		{
			get { return _day; }
		}

		public void AddParty(PartyVO party)
		{
			if (party != null && _day == party.Date)
			{
				PartyVO accept;
				if (_parties == null) _parties = new List<PartyVO>();
				if (!_parties.Contains(party)) _parties.Add(party);
				Party1Icon.enabled = _parties.Count > 1; // Show for more than 1 party
				Party2Icon.enabled = true; // Show for at least 1 party
				accept = _parties.Find(p=>p.RSVP > 0);

				// Show party 2's RSVP icon only if there's an accepted party or all declined parties
				Party2RSVPIcon.enabled = accept!=null || _parties.TrueForAll(p=>p.RSVP < 0);
				// Set the Party 2 acceptance icon
				Party2RSVPIcon.sprite = CalendarSpriteConfig.GetSprite(accept!=null ? "accepted" : "declined");
				if (accept == null) accept = _parties.Find(p=>p.RSVP==0);
	
				// Set Party 2's Faction icon
				if (accept != null) Party2Icon.sprite = CalendarSpriteConfig.GetSprite(accept.Faction);

				if (Party1Icon.enabled)
				{
					party = _parties.Find(p=>p != accept && p.RSVP==0);
					Party1RSVPIcon.enabled = party != null;
					if (party == null) party = _parties.Find(p=>p != accept);
					Party1Icon.sprite = CalendarSpriteConfig.GetSprite(party.Faction);
				}
			}
		}

		void Awake()
		{
	        myBlockImage = this.GetComponent<Image>();
	        defaultColor = myBlockImage.color;
			_btn = this.GetComponent<Button>();
			_btn.onClick.AddListener(HandleClick);
			Party2RSVPIcon.sprite = CalendarSpriteConfig.GetSprite("declined");
	    }

		private void HandleClick()
	    {
	    	AmbitionApp.SendMessage<DateTime>(CalendarMessages.SELECT_DATE, _day);
	    }
	}
}
