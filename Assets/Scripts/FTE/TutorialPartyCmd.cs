using System;
using Core;

namespace DeWinter
{
	public class TutorialPartyCmd : ICommand<DateTime>
	{
		public void Execute (DateTime date)
		{
			DeWinterApp.OpenDialog(TutorialPartyDialogMediator.DIALOG_ID);
			DeWinterApp.UnregisterCommand<TutorialPartyCmd, RoomVO>();
		}
	}
}