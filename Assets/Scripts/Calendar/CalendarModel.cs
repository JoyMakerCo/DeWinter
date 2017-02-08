using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Newtonsoft.Json;

namespace DeWinter
{
	public class CalendarModel : DocumentModel
	{
		private DateTime _startDate;
		private int _gameLength;

		public int Day=0;

		[JsonProperty("eventChance")]
		public float EventChance;

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

		public DateTime Date
		{
			get { return _startDate.AddDays(Day); }
		}

		public int EndDate
		{
			get { return _startDate.AddDays(_gameLength); }
		}

		public int uprisingDay; //The Day of the Uprising that the Game Ends On

		public int NextStyleSwitchDay;

		public CalendarModel() : base("CalendarData")
		{
			uprisingDay= (new Random()).Next(25, 31);
		}
	}
}