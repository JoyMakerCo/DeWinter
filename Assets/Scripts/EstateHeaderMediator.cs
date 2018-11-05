using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EstateHeaderMediator : MonoBehaviour {

    public Text CalendarText;
    public Shadow CalendarTextShadow;
    public Text WardrobeText;
    public Shadow WardrobeTextShadow;
    public Text JournalText;
    public Shadow JournalTextShadow;
    public Text EstateText;
    public Shadow EstateTextShadow;

    private bool _calendarSelected;
    private bool _wardrobeSelected;
    private bool _journalSelected;
    private bool _estateSelected;

    public Color DarkColor;
    public Color LightColor;

    private void Awake()
    {
        CalendarClicked(); //Always start with the Calendar being selected
    }

    public void CalendarHover()
    {
        CalendarText.color = LightColor;
        CalendarTextShadow.effectColor = DarkColor;
    }

    public void WardrobeHover()
    {
        WardrobeText.color = LightColor;
        WardrobeTextShadow.effectColor = DarkColor;
    }

    public void JournalHover()
    {
        JournalText.color = LightColor;
        JournalTextShadow.effectColor = DarkColor;
    }

    public void EstateHover()
    {
        EstateText.color = LightColor;
        EstateTextShadow.effectColor = DarkColor;
    }

    public void CalendarMouseExit()
    {
        if (_calendarSelected)
        {
            CalendarText.color = LightColor;
            CalendarTextShadow.effectColor = DarkColor;
        } else
        {
            CalendarText.color = DarkColor;
            CalendarTextShadow.effectColor = LightColor;
        }
    }

    public void WardrobeMouseExit()
    {
        if (_wardrobeSelected)
        {
            WardrobeText.color = LightColor;
            WardrobeTextShadow.effectColor = DarkColor;
        } else
        {
            WardrobeText.color = DarkColor;
            WardrobeTextShadow.effectColor = LightColor;
        }
    }

    public void JournalMouseExit()
    {
        if (_journalSelected)
        {
            JournalText.color = LightColor;
            JournalTextShadow.effectColor = DarkColor;
        }
        else
        {
            JournalText.color = DarkColor;
            JournalTextShadow.effectColor = LightColor;
        }
    }

    public void EstateMouseExit()
    {
        if (_estateSelected)
        {
            EstateText.color = LightColor;
            EstateTextShadow.effectColor = DarkColor;
        }
        else
        {
            EstateText.color = DarkColor;
            EstateTextShadow.effectColor = LightColor;
        }
    }

    public void CalendarClicked()
    {
        _calendarSelected = true;
        _wardrobeSelected = false;
        _journalSelected = false;
        _estateSelected = false;
        CalendarHover(); //This has to be in there because the screen opens on the Calendar tab
        WardrobeMouseExit();
        JournalMouseExit();
        EstateMouseExit();
    }

    public void WardrobeClicked()
    {
        _calendarSelected = false;
        _wardrobeSelected = true;
        _journalSelected = false;
        _estateSelected = false;
        WardrobeHover();
        CalendarMouseExit();
        JournalMouseExit();
        EstateMouseExit();
    }

    public void JournalClicked()
    {
        _calendarSelected = false;
        _wardrobeSelected = false;
        _journalSelected = true;
        _estateSelected = false;
        JournalHover();
        CalendarMouseExit();
        WardrobeMouseExit();
        EstateMouseExit();
    }

    public void EstateClicked()
    {
        _calendarSelected = false;
        _wardrobeSelected = false;
        _journalSelected = false;
        _estateSelected = true;
        EstateHover();
        CalendarMouseExit();
        WardrobeMouseExit();
        JournalMouseExit();
    }
}
