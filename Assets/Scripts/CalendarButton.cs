using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DeWinter;

public class CalendarButton : MonoBehaviour {

    public int rowID; //Button Row
    public int columnID; // Button Column?
    public GameObject screenFader; // It's for the RSVP pop-up
    public Image currentDayOutline;
    public Text date;
    public Image pastDayXImage;
    //Party Indicators
    public Image party1FactionImage;
    public Image party1PositiveReplyImage;
    public Image party1NegativeReplyImage;
    public Image party2FactionImage;
    public Image party2PositiveReplyImage;
    public Image party2NegativeReplyImage;

    //Faction Icon Sprites
    public Sprite crownFactionSprite;
    public Sprite churchFactionSprite;
    public Sprite militaryFactionSprite;
    public Sprite bourgeoisieFactionSprite;
    public Sprite thirdEstateFactionSprite;

    Dictionary<string, Sprite> factionSprites = new Dictionary<string, Sprite>();

    private Image myBlockImage;
    private Color defaultColor;
	private DateTime _day;

    int startPositionPlusDays;

    void Awake()
    {
        myBlockImage = this.GetComponent<Image>();
        defaultColor = myBlockImage.color;
        StockSpriteDictionary();
        DeWinterApp.Subscribe<DateTime>(HandleCalendarDay);
    }

    void OnDestroy()
    {
		DeWinterApp.Unsubscribe<DateTime>(HandleCalendarDay);
    }

	private void HandleCalendarDay(DateTime day)
	{
		_day = day.Date;
    	int offset = (_day.Day - (int)(_day.DayOfWeek))%7;
    	offset = (int)Math.Floor((float)((offset < 0 ? offset + 7 : offset) + _day.Day - 1)/7f);
		_day = _day.AddDays(7*(rowID - offset) + columnID - (int)(_day.DayOfWeek));

		bool isCurrentMonth = (_day.Month == day.Date.Month);
        List<Party> parties;
		int count=0;
		bool doEnable;

		myBlockImage.color = isCurrentMonth ? defaultColor : Color.gray;
		date.text = day.Day.ToString();

		if (DeWinterApp.GetModel<CalendarModel>().Parties.TryGetValue(_day, out parties))
		{
			count = parties.FindAll(p => p.invited).Count;
		}

		doEnable = count > 0;
		party1PositiveReplyImage.enabled = doEnable && parties[0].RSVP == 1;
		party1NegativeReplyImage.enabled = doEnable && parties[0].RSVP == -1;
		party1FactionImage.enabled = doEnable;
		if (doEnable)
		{
			party1FactionImage.sprite = factionSprites[parties[0].faction];
		}

		doEnable = count > 1;
		party2PositiveReplyImage.enabled = doEnable && (parties[1].RSVP == 1);
		party2NegativeReplyImage.enabled = doEnable && (parties[1].RSVP == -1);
		party2FactionImage.enabled = doEnable;
		if (doEnable)
		{
			party2FactionImage.sprite = factionSprites[parties[1].faction];
		}

		doEnable = (_day == day.Date);

		currentDayOutline.enabled = doEnable;
		if (doEnable) this.transform.SetAsLastSibling();
		pastDayXImage.enabled = (day.Date > _day);
    }

    public void RSVP()
    {
    	if (_day != default(DateTime))
    	DeWinterApp.SendMessage<DateTime>(CalendarMessages.RSVP, _day);
    }

    void StockSpriteDictionary()
    {
        factionSprites.Add("Crown", crownFactionSprite);
        factionSprites.Add("Church", churchFactionSprite);
        factionSprites.Add("Military", militaryFactionSprite);
        factionSprites.Add("Bourgeoisie", bourgeoisieFactionSprite);
        factionSprites.Add("Third Estate", thirdEstateFactionSprite);
    }
}