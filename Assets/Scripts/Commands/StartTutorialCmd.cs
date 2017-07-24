using System;
using Core;

namespace Ambition
{
	public class StartTutorialCmd : ICommand
	{
		public void Execute ()
		{
			PartyModel pmod = AmbitionApp.GetModel<PartyModel>();
			PartyVO p = new PartyVO(1);
			p.MapID = "Tutorial";
			p.Turns = 10;
			p.invited = true;
			p.invitationDistance = 1;
			p.Date = AmbitionApp.GetModel<CalendarModel>().Today;
			p.IntroText = "party_tutorial_welcome_dialog";

			pmod.Party = p;

			AmbitionApp.GetModel<EventModel>().SelectedEvent = null;
			AmbitionApp.UnregisterCommand<StartTutorialCmd>(GameMessages.START_TUTORIAL);
			AmbitionApp.RegisterCommand<CreateInvitationsCmd, DateTime>();
			AmbitionApp.SendMessage<string>(GameMessages.LOAD_SCENE, SceneConsts.GAME_PARTYLOADOUT); 
		}
	}
}