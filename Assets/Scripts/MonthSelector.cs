using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

namespace DeWinter
{
	public class MonthSelector : MonoBehaviour
	{
	    public Text displayMonthText;
	    private DateTime _date;
	    private CalendarModel _model;

	    // Update is called once per frame
	    void Start()
	    {
	    	_model = DeWinterApp.GetModel<CalendarModel>();
	    	DeWinterApp.Subscribe<DateTime>(HandleCalendarDay);
	    }

	    void OnDestroy()
	    {
			DeWinterApp.Unsubscribe<DateTime>(HandleCalendarDay);
	    }

	    public void NextMonth()
	    {
	    	if (_date.Month < _model.EndDate.Month)
	    	{
	    		_date = _date.AddMonths(1);
	    		UpdateMonth();
	    	}
	    }

	    public void PrevMonth()
	    {
			if (_date.Month > _model.StartDate.Month)
	    	{
	    		_date = _date.AddMonths(-1);
	    		UpdateMonth();
	    	}
	    }

	    public void CurrMonth()
	    {
	    	HandleCalendarDay(_model.Today);
	    }

		private void HandleCalendarDay(DateTime day)
	    {
	    	_date = new DateTime(day.Year, day.Month, 1);
	    	UpdateMonth();
		}

	    private void UpdateMonth()
	    {
			displayMonthText.text = _model.GetMonthString(_date);
			DeWinterApp.SendMessage<DateTime>(CalendarMessages.VIEW_MONTH, _date);
	    }
	}
}