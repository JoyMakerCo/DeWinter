using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

namespace DeWinter
{
	public class MonthSelector : MonoBehaviour
	{
	    public Text displayMonthText;
	    private int _displayMonth;
	    private CalendarModel _model;

	    // Update is called once per frame
	    void Start()
	    {
	    	_model = DeWinterApp.GetModel<CalendarModel>();
	    	DeWinterApp.Subscribe<DateTime>(HandleCalendarDay);
	        DisplayMonth = _model.Today.Month;
	    }

	    public void ViewMonthAhead()
	    {
	    	DisplayMonth++;
	    }

	    public void ViewMonthBehind()
	    {
			DisplayMonth--;
	    }

		private void HandleCalendarDay(DateTime day)
	    {
			DisplayMonth = day.Date.Month;
	    }

	    public int DisplayMonth
	    {
	    	get { return _displayMonth; }
	    	set {
				_displayMonth = (value < _model.StartDate.Month)
					? _model.StartDate.Month
					: value > _model.EndDate.Month
					? _model.EndDate.Month
					: value;
				displayMonthText.text = _model.GetMonthString(_displayMonth);
	    	}
	    }
	}
}