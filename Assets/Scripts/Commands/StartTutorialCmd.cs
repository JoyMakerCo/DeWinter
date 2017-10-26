using System;
using Core;

namespace Ambition
{
	public class StartTutorialCmd : ICommand
	{
		public void Execute ()
		{
			PartyModel pmod = AmbitionApp.GetModel<PartyModel>();
			CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
			PartyVO p = new PartyVO(1);
			p.MapID = "Tutorial";
			p.Turns = 10;
			p.invited = true;
			p.RSVP = 1;
			p.invitationDistance = 1;
			p.Date = calendar.Today;
			p.IntroText = "party_tutorial_welcome_dialog";
			p.description = AmbitionApp.GetModel<LocalizationModel>().GetString("tutorial_party_description");

			pmod.Party = p;
			calendar.Parties[p.Date]=new System.Collections.Generic.List<PartyVO>(){p};

			AmbitionApp.GetModel<EventModel>().Event = null;
			AmbitionApp.UnregisterCommand<StartTutorialCmd>(GameMessages.START_TUTORIAL);
			AmbitionApp.RegisterCommand<WorkTheRoomTutorialCmd, RoomVO>(MapMessage.GO_TO_ROOM);
			AmbitionApp.RegisterCommand<TutorialRailroadCommand, RoomVO>();
			AmbitionApp.SendMessage<string>(GameMessages.LOAD_SCENE, SceneConsts.GAME_PARTYLOADOUT); 
		}
	}
}
