using System;
using Core;

namespace DeWinter
{
	public class AdvanceDayCmd : ICommand
	{
		public void Execute()
		{
			CalendarModel model = DeWinterApp.GetModel<CalendarModel>();
			model.Day++;
		}
	}
}

