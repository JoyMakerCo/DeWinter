using System;

namespace DeWinter
{
	public class CalendarDayVO
	{
		public DateTime Date;
		public int Day;

		public CalendarDayVO (int day, DateTime date)
		{
			this.Day = day;
			this.Date = date;
		}
	}
}