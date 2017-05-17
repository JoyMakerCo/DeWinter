using System;
using Core;

namespace Ambition
{
	public class TutorialPartyCmd : ICommand<RoomVO>
	{
		public void Execute (RoomVO room)
		{
			AmbitionApp.OpenDialog(TutorialPartyDialogMediator.DIALOG_ID);
			AmbitionApp.UnregisterCommand<TutorialPartyCmd, RoomVO>();
		}
	}
}