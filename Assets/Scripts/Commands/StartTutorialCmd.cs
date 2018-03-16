﻿using System;
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
			party.Name = AmbitionApp.GetString("party.name." + party.ID);
			party.Description = AmbitionApp.GetString("party.description." + party.ID);
			model.Party = party;
			model.Confidence = model.StartConfidence = model.MaxConfidence = 120;
			calendar.Parties[party.Date]=new System.Collections.Generic.List<PartyVO>(){party};

			AmbitionApp.GetModel<IncidentModel>().Incident = null;
			AmbitionApp.UnregisterCommand<StartTutorialCmd>(GameMessages.START_TUTORIAL);
			AmbitionApp.UnregisterCommand<TutorialConfidenceCheckCmd>(PartyMessages.SHOW_MAP);
			AmbitionApp.RegisterCommand<WorkTheRoomTutorialCmd, RoomVO>(MapMessage.GO_TO_ROOM);
			AmbitionApp.RegisterCommand<TutorialRailroadCommand, RoomVO>();
			AmbitionApp.RegisterCommand<TutorialConfidenceCheckCmd>(PartyMessages.SHOW_MAP);

			AmbitionApp.RegisterCommand<TutorialLoadoutCmd, string>(GameMessages.DIALOG_CLOSED);
		}
	}
}
