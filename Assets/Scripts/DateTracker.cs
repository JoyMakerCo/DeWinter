using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Globalization;

public class DateTracker : MonoBehaviour {
    private Text myText;

    void Start()
    {
        myText = GetComponent<Text>();
        updateDate();
    }

    public void updateDate()
    {
        string month = GameData.calendar.monthList[GameData.currentMonth].name;
        string day = dayString(GameData.currentDay + 1); //Zero Indexed Days
        myText.text = month + " " + day + ", 1795";
    }

    string dayString(int day)
    {
        if (day <= 0)
        {
            return day.ToString();
        }
        switch (day % 10)
        {
            case 1:
                return day + "st";
            case 2:
                return day + "nd";
            case 3:
                return day + "rd";
            default:
                return day + "th";
        }
    }
}
