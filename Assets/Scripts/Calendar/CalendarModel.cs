using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Newtonsoft.Json;

namespace DeWinter
{
	public class CalendarModel : DocumentModel
	{
		public int Day=0;

		[JsonProperty("startDate")]
		private string _startDateStr
		{
			set { _startDate = DateTime.Parse(value); }
		}

		private DateTime _startDate;
		public DateTime Date
		{
			get { return _startDate.AddDays(Day); }
		}

		[JsonProperty("eventChance")]
		public float EventChance;

		[JsonProperty("gameLength")]
		private int _gameLength;

		public int DaysLeft
		{
			get { return _gameLength - Day; }
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