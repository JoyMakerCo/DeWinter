using System;
using Core;

namespace Ambition
{
	public class AdvanceDayCmd : ICommand
	{
		public void Execute()
		{
			CalendarModel cmod = DeWinterApp.GetModel<CalendarModel>();
			cmod.Today = cmod.Today.AddDays(1);
		}
	}
}