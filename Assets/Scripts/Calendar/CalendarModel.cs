using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Newtonsoft.Json;
using Util;

namespace DeWinter
{
	public class CalendarModel : DocumentModel, IInitializable
	{
		public static readonly string[] MONTHS = {
			"Janvier",
        	"Fevrier",
        	"Mars",
        	"Avril",
        	"Mai",
        	"Juin",
        	"Juillet",
        	"Aout",
        	"Septembre",
        	"Octobre",
        	"Novembre",
        	"Decembre"
        };

		public Dictionary<DateTime, List<Party>> Parties;

		private DateTime _startDate;
		private int _gameLength;
		private int _day=-1;

		public int Day
		{
			get { return _day; }
			set {
				if (value != _day)
				{
					CalendarDayVO msg = new CalendarDayVO(_day = value, Today);
					DeWinterApp.SendMessage<CalendarDayVO>(msg);
				}
			}
		}

		public DateTime StartDate
		{
			get { return _startDate; }
		}

		public string GetMonthString(DateTime date)
		{
			return MONTHS[date.Month-1];
		}

		public string GetMonthString(int day)
		{
			return MONTHS[_startDate.AddDays(day).Month-1];
		}

		public string GetMonthString()
		{
			return MONTHS[Today.Month - 1];
		}

		public string GetDateString()
		{
			return GetDateString(Today);
		}

		public string GetDateString(DateTime d)
		{
			return d.Day.ToString() + " " + GetMonthString(d) + ", " + d.Year.ToString();
		}

		public string GetDateString(int day)
		{
			return GetDateString(_startDate.AddDays(day));
		}

		[JsonProperty("gameLength")]
		public int DaysLeft
		{
			get { return _gameLength - Day; }
			private set { _gameLength = value; }
		}

		[JsonProperty("startDate")]
		private string _startDateStr
		{
			set { _startDate = DateTime.Parse(value); }
		}

		public DateTime Today
		{
			get { return _startDate.AddDays(_day); }
		}

		public DateTime EndDate
		{
			get { return _startDate.AddDays(_gameLength); }
		}

		public DateTime DaysFromNow(int days)
		{
			return Today.AddDays(days);
		}

		public DateTime Yesterday
		{
			get { return DaysFromNow(-1); }
		}

		public int uprisingDay; //The Day of the Uprising that the Game Ends On

		public int NextStyleSwitchDay;

		public CalendarModel() : base("CalendarData")
		{
			Parties = new Dictionary<DateTime, List<Party>>();
		}

		public void Initialize()
		{
			uprisingDay= (new Random()).Next(25, 31);
			DeWinterApp.Subscribe(CalendarMessages.ADVANCE_DAY, AdvanceDay);
		}

		// TODO: Parties should probably store their dates
		public void AddParty(DateTime date, Party party)
		{
			if (!Parties.ContainsKey(date))
			{
				Parties.Add(date, new List<Party>{party});
			}
			else
			{
				Parties[date].Add(party);
			}
		}

		public void AddParty(int day, Party party)
		{
			AddParty(_startDate.AddDays(day), party);
		}

		private void AdvanceDay() { Day++; }

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