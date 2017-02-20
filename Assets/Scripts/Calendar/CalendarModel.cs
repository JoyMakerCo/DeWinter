using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Newtonsoft.Json;

namespace DeWinter
{
	public class CalendarModel : DocumentModel
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

// TODO: Register commands to respond to messages
					DeWinterApp.SendCommand<PayDayCmd, CalendarDayVO>(msg);
				}
			}
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
			uprisingDay= (new Random()).Next(25, 31);
			Parties = new Dictionary<DateTime, List<Party>>();
		}
	}
}