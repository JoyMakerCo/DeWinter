using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Ambition;

namespace Ambition
{
	[Serializable]
	public class FactionSprite
	{
		public string Faction;
		public Sprite Symbol;
	}

	public class CalendarButton : MonoBehaviour
	{

	    public int rowID; //Button Row
	    public int columnID; // Button Column?
	    public Image currentDayOutline;
	    public Text dateText;
	    public Image pastDayXImage;

	    //Party Indicators
	    public Image[] partyFactionImages;
	    public GameObject[] partyPositiveReplyImages;
		public GameObject[] partyNegativeReplyImages;

	    //Faction Icon Sprites
		public FactionSprite[] FactionSprites;

	    private Image myBlockImage;
	    private Color defaultColor;
		private DateTime _day;
		private List<Party> _parties;
		private int _displayMonth;
		private Button _btn;
		private CalendarModel _model;

	    void Awake()
	    {
	        myBlockImage = this.GetComponent<Image>();
	        defaultColor = myBlockImage.color;
			_btn = this.GetComponent<Button>();
			_btn.onClick.AddListener(RSVP);
			_model = Ambition.GetModel<CalendarModel>();
			Ambition.Subscribe<DateTime>(HandleCalendarDay);
			Ambition.Subscribe<DateTime>(CalendarMessages.VIEW_MONTH, HandleCalendarDay);
			Ambition.Subscribe<Party>(HandlePartyUpdated);
			Ambition.Subscribe<Party>(PartyMessages.RSVP, HandlePartyUpdated);
	    }

	    void OnDestroy()
	    {
			_btn.onClick.RemoveListener(RSVP);
			Ambition.Unsubscribe<DateTime>(HandleCalendarDay);
			Ambition.Unsubscribe<DateTime>(CalendarMessages.VIEW_MONTH, HandleCalendarDay);
			Ambition.Unsubscribe<Party>(HandlePartyUpdated);
			Ambition.Unsubscribe<Party>(PartyMessages.RSVP, HandlePartyUpdated);
	    }

		private void HandleCalendarDay(DateTime date)
		{
			_day = new DateTime(date.Year, date.Month, 1);
			int offset = (6 + (int)(_day.DayOfWeek))%7;
			_day = _day.AddDays(rowID*7 + columnID - offset);

			bool isCurrentMonth = (_day.Month == date.Month);
	        List<Party> parties;

			myBlockImage.color = isCurrentMonth ? defaultColor : Color.gray;
			dateText.text = _day.Day.ToString();

			if (_model.Parties.TryGetValue(_day, out parties))
			{
				_parties = parties.FindAll(p => p.invited);
			}
			else
			{
				_parties = new List<Party>();
			}

			for (int i=partyFactionImages.Length-1; i>=0; i--)
			{
				partyFactionImages[i].enabled = _parties.Count > i;
				if (partyFactionImages[i].enabled)
					partyFactionImages[i].sprite = Array.Find(FactionSprites, s=>s.Faction == _parties[i].faction).Symbol;
			}

			for (int i=partyPositiveReplyImages.Length-1; i>=0; i--)
			{
				partyPositiveReplyImages[i].SetActive((i < _parties.Count) && (_parties[i].RSVP == 1));
			}

			for (int i=partyNegativeReplyImages.Length-1; i>=0; i--)
			{
				partyNegativeReplyImages[i].SetActive((i < _parties.Count) && (_parties[i].RSVP == -1));
			}

			bool isToday = (_day == _model.Today);
			currentDayOutline.enabled = isToday;
			if (isToday) this.transform.SetAsLastSibling();
			pastDayXImage.enabled = (_model.Today > _day);
		}

		private void HandlePartyUpdated(Party p)
		{
			if (p.Date == _day)
			{
				int index = _parties.IndexOf(p);
				if (index < 0)
				{
					index = _parties.Count;
					_parties.Add(p);
				}
				if (index < partyPositiveReplyImages.Length)
					partyPositiveReplyImages[index].SetActive(p.RSVP == 1); 
				if (index < partyNegativeReplyImages.Length)
					partyNegativeReplyImages[index].SetActive(p.RSVP == -1);
				if (index < partyFactionImages.Length)
				{
					partyFactionImages[index].enabled = true;
					partyFactionImages[index].sprite = Array.Find(FactionSprites, s=>s.Faction == _parties[index].faction).Symbol;
				}
			}
		}

	    private void RSVP()
	    {
	    	Ambition.SendMessage<DateTime>(CalendarMessages.SELECT_DATE, _day);
	    }
	}
}