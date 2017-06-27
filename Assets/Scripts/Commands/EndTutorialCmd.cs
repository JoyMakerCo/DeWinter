using System;
using Core;

namespace Ambition
{
	public class EndTutorialCmd : ICommand
	{
		public void Execute ()
		{
			AmbitionApp.UnregisterCommand<StartTutorialCmd>(GameMessages.START_TUTORIAL);
			AmbitionApp.UnregisterCommand<TutorialPartyWelcomeCmd, RoomVO>();
			AmbitionApp.UnregisterCommand<TutorialConfidenceCheckCmd, int>(GameConsts.CONFIDENCE);
			AmbitionApp.UnregisterCommand<EndTutorialCmd>(PartyMessages.END_PARTY);

			AmbitionApp.RegisterCommand<ConfidenceCheckCmd, int>(GameConsts.CONFIDENCE);
			AmbitionApp.RegisterCommand<EndPartyCmd>(PartyMessages.END_PARTY);
		}
	}
}