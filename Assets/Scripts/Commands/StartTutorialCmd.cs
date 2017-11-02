using System;
using Core;

namespace Ambition
{
	public class StartTutorialCmd : ICommand
	{
		public void Execute ()
		{
			PartyModel model = AmbitionApp.GetModel<PartyModel>();
			CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
			PartyVO party = Array.Find(model.Parties, p=>p.ID == "tutorial");
			party.invited = true;
			party.RSVP = 1;
			model.Party = party;
			calendar.Parties[party.Date]=new System.Collections.Generic.List<PartyVO>(){party};

			AmbitionApp.GetModel<EventModel>().Event = null;
			AmbitionApp.UnregisterCommand<StartTutorialCmd>(GameMessages.START_TUTORIAL);
			AmbitionApp.RegisterCommand<WorkTheRoomTutorialCmd, RoomVO>(MapMessage.GO_TO_ROOM);
			AmbitionApp.RegisterCommand<TutorialRailroadCommand, RoomVO>();
			AmbitionApp.SendMessage<string>(GameMessages.LOAD_SCENE, SceneConsts.GAME_PARTYLOADOUT); 
		}
	}
}
