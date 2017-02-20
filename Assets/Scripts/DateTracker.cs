using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Globalization;

namespace DeWinter
{
	public class DateTracker : MonoBehaviour
	{
	    private Text myText;
	    private CalendarModel _model;

	    void Start()
	    {
	        myText = GetComponent<Text>();
	        _model = DeWinterApp.GetModel<CalendarModel>();
	        updateDate();
	    }

	    public void updateDate()
	    {
			myText.text = _model.GetMonthString() + " " + _model.Today.Day.ToString() + ", " + _model.Today.Year.ToString();
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
}