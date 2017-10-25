using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Newtonsoft.Json;
using Util;

namespace Ambition
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

		public Dictionary<DateTime, List<PartyVO>> Parties = new Dictionary<DateTime, List<PartyVO>>();

		private DateTime _startDate;
		private int _gameLength;

		private int _day = 0;

		public DateTime StartDate
		{
			get { return _startDate; }
		}

		public string GetMonthString(DateTime date)
		{
			return MONTHS[date.Month-1];
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

		[JsonProperty("gameLength")]
		public int DaysLeft
		{
			get { return _gameLength - _day; }
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
			set {
				_day = (value - _startDate).Days;
				AmbitionApp.SendMessage<DateTime>(value);
			}
		}

		public DateTime EndDate
		{
			get { return _startDate.AddDays(_gameLength); }
		}

		public DateTime DaysFromNow(int days)
		{
			return _startDate.AddDays(days + _day);
		}

		public DateTime Yesterday
		{
			get { return DaysFromNow(-1); }
		}

		public DateTime uprisingDay; //The Day of the Uprising that the Game Ends On

		public DateTime NextStyleSwitchDay;

		public CalendarModel() : base("CalendarData") {}

		public void Initialize()
		{
			uprisingDay= _startDate.AddDays(new Random().Next(25, 31));
		}

		public void UpdateParty(PartyVO party)
		{
			if (!Parties.ContainsKey(party.Date))
			{
				Parties.Add(party.Date, new List<PartyVO>{party});
			}
			else if (!Parties[party.Date].Contains(party))
			{
				Parties[party.Date].Add(party);
			}
			AmbitionApp.SendMessage<PartyVO>(party);
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
