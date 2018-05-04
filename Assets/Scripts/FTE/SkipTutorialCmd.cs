using System;
using Core;

namespace Ambition
{
	public class SkipTutorialCmd : ICommand
	{
		public void Execute ()
		{
			AmbitionApp.RegisterCommand<OutOfConfidenceDialogCmd>(PartyMessages.SHOW_MAP);
			AmbitionApp.UnregisterCommand<StartTutorialCmd>(GameMessages.START_TUTORIAL);
			AmbitionApp.UnregisterCommand<SkipTutorialCmd>(GameMessages.SKIP_TUTORIAL);
		}
	}
}
