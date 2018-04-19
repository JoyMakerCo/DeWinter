using System;
using Core;

namespace Ambition
{
	public class AdvanceDayCmd : ICommand
	{
		public void Execute()
		{
			CalendarModel cmod = AmbitionApp.GetModel<CalendarModel>();
			cmod.Today = cmod.Today.AddDays(1);
		}
	}
}
