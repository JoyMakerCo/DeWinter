using System;
using Core;

namespace DeWinter
{
	public class AdvanceDayCmd : ICommand<string>
	{
		public void Execute(string scene)
		{
			if (scene == SceneConsts.GAME_ESTATE)
			{
				CalendarModel cmod = DeWinterApp.GetModel<CalendarModel>();
				cmod.Today = cmod.Today.AddDays(1);
			}
		}
	}
}