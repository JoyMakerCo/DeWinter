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
	    public int DayIndex;

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
		private int _currentMonth;
		private Button _btn;
		private CalendarModel _model;

		void OnEnable()
		{
	        myBlockImage = this.GetComponent<Image>();
	        defaultColor = myBlockImage.color;
			_btn = this.GetComponent<Button>();
			_model = AmbitionApp.GetModel<CalendarModel>();

			AmbitionApp.Subscribe<DateTime>(HandleViewMonth);
			AmbitionApp.Subscribe<DateTime>(CalendarMessages.VIEW_MONTH, HandleViewMonth);
			AmbitionApp.Subscribe<PartyVO>(HandlePartyUpdated);

			Party1Icon.enabled = false;
	    	NewParty1Icon.enabled = false;
	    	Party1RSVPIcon.enabled = false;
			Party2Icon.enabled = false;
	    	NewParty2Icon.enabled = false;
	    	Party2RSVPIcon.enabled = false;

			_currentMonth = _model.Today.Month;
			_btn.onClick.AddListener(HandleClick);
			HandleViewMonth(_model.Today);
	    }

	    void OnDisable()
	    {
			AmbitionApp.Unsubscribe<DateTime>(HandleViewMonth);
			AmbitionApp.Unsubscribe<DateTime>(CalendarMessages.VIEW_MONTH, HandleViewMonth);
			AmbitionApp.Unsubscribe<PartyVO>(HandlePartyUpdated);
			_btn.onClick.RemoveListener(HandleClick);
	    }

		private void HandleViewMonth(DateTime date)
		{
			int firstOffset = ((int)(date.DayOfWeek) + 7 - ((date.Day-1)%7))%7;
			_day = date.AddDays(DayIndex - firstOffset);
			_currentMonth = date.Month;

			myBlockImage.color = (_day.Month == _currentMonth)
				? defaultColor
				: Color.Lerp(defaultColor, Color.black, .5f);
			dateText.text = _day.Day.ToString();

			bool isToday = (_day == _model.Today);
			currentDayOutline.enabled = isToday;
			if (isToday) this.transform.SetAsLastSibling();

			_btn.interactable = (_day >= _model.Today);
			pastDayXImage.enabled = !_btn.interactable;
			UpdateParties();
		}

		private void HandleClick()
	    {
	    	AmbitionApp.SendMessage<DateTime>(CalendarMessages.SELECT_DATE, _day);
	    }

	    private void HandlePartyUpdated(PartyVO party)
		{
			if (party.Date == _day)
				UpdateParties();
		}

	    private void UpdateParties()
	    {
	    	List<PartyVO> parties;
			_model.Parties.TryGetValue(_day, out parties);
			if (parties != null && parties.Count > 0)
			{
				PartyVO party = parties.Find(p=>p.RSVP != 0);
				bool enabled = party != null;
				Party1Icon.enabled = true;
				Party1RSVPIcon.enabled = enabled;
				NewParty1Icon.enabled = !enabled;
				if (!enabled) party = parties[0];
				Party1RSVPIcon.sprite = CalendarSpriteConfig.GetSprite(party.RSVP == 1 ? "accepted" : "declined");
				Party1Icon.sprite = CalendarSpriteConfig.GetSprite(party.Faction);

				party = parties.Find(p=>p!=party);
				enabled = party != null;
				Party2Icon.enabled = enabled;
				Party2RSVPIcon.enabled = enabled && party.RSVP != 0;
				NewParty2Icon.enabled = enabled && party.RSVP == 0;
				if (enabled)
				{
					Party2Icon.sprite = CalendarSpriteConfig.GetSprite(party.Faction);
					Party2RSVPIcon.sprite = CalendarSpriteConfig.GetSprite(party.RSVP == 1 ? "accepted" : "declined");
				}
			}
			else
			{
				Party1Icon.enabled = false;
		    	NewParty1Icon.enabled = false;
		    	Party1RSVPIcon.enabled = false;
				Party2Icon.enabled = false;
		    	NewParty2Icon.enabled = false;
		    	Party2RSVPIcon.enabled = false;
			}
	    }
	}
}
