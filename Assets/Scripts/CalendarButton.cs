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

    private Text myText;
    private Text myDate;
    private Image myBlockImage;
    private Image myXImage;
    private Image myCircleImage;
    private Image mySlashImage;
    private Outline myOutline;
    private Color defaultColor;
	private CalendarModel _calendarModel;
	private DateTime _day;

    int startPositionPlusDays;

    void Start()
    {
		_calendarModel = DeWinterApp.GetModel<CalendarModel>();
        myText = this.transform.Find("Text").GetComponent<Text>();
        myDate = this.transform.Find("DateText").GetComponent<Text>();
        myBlockImage = this.GetComponent<Image>();
        myXImage = this.transform.Find("XImage").GetComponent<Image>();
        myCircleImage = this.transform.Find("CircleImage").GetComponent<Image>();
        mySlashImage = this.transform.Find("SlashImage").GetComponent<Image>();
        myOutline = this.GetComponent<Outline>();
        defaultColor = myBlockImage.color;
    }

    private void SetButtonDay()
    {
		_day = _calendarModel.Today;
    	int offset = (_day.Day - (int)(_day.DayOfWeek))%7;
    	offset = (int)Math.Floor((float)((offset < 0 ? offset + 7 : offset) + _day.Day - 1)/7f);
		_day = _day.AddDays(7*(rowID - offset) + columnID - (int)(_day.DayOfWeek));
    }

    // TODO: Respond to events, not update once per frame
    void Update ()
    {
        SetButtonDay();
		bool isCurrentMonth = (_day.Month == _calendarModel.Today.Month);
		myDate.text = _day.Day.ToString();
        
        List<Party> parties;
		Party party=null;
		int count = 0;

        if (_calendarModel.Parties.TryGetValue(_day, out parties))
        {
        	foreach (Party p in parties)
        	{
        		if (p.invited)
        		{
					if (p.RSVP == 1 || party == null || party.RSVP == 0)
        				party = p;
        			count++;
        		}
        	}
        }
    	switch (count)
    	{
    		case 0:
				myText.text = "";
    			break;
    		case 1:
				myText.text = party.SizeString() + " " + party.faction + " Party";
				break;
			default:
				myText.text = count.ToString() + " Parties";
				break;
    	}

		myBlockImage.color = isCurrentMonth ? defaultColor : Color.gray;

		// For days that have already passed
		myXImage.color = _calendarModel.Today > _day ? Color.white : Color.clear;

		// For Today
		myOutline.effectColor = _calendarModel.Today == _day ? Color.black : Color.clear;

		// For confirmed parties
		myCircleImage.color = (party != null && party.RSVP == 1) ? Color.white : Color.clear;

		// For negative RSVP
		mySlashImage.color = (party != null && party.RSVP == -1) ? Color.white : Color.clear;
    }

    public void RSVP()
    {
        //Pop Up Window
        List<Party> parties;
		if (_day != default(DateTime) && _calendarModel.Parties.TryGetValue(_day, out parties))
		{
			parties = parties.FindAll(x => x.invited);
			if (parties.Count == 0) return; // No parties!

			Party party = parties.Find(x => x.RSVP == 1);
			if (party != null)
			{
				object[] objectStorage = new object[2];
				objectStorage[0] = party;
				objectStorage[1] = (_day == _calendarModel.Today);
				screenFader.gameObject.SendMessage("CreateCancellationPopUp", objectStorage);
			}
			else if (parties.Count == 1)
			{
				screenFader.gameObject.SendMessage("CreateRSVPPopUp", parties[0]);
			}
			else
			{
				object[] objectStorage = new object[3];
				objectStorage[0] = parties[0];
				objectStorage[1] = parties[1];
				objectStorage[2] = _day;
				screenFader.gameObject.SendMessage("CreateTwoPartyChoicePopUp", objectStorage);
			}
		}
    }
}