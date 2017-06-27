using System;
using Core;

namespace Ambition
{
	public class StartTutorialCmd : ICommand
	{
		private const string TUTORIAL_MAP_ID = "Tutorial";
		public void Execute ()
		{
			PartyModel pmod = AmbitionApp.GetModel<PartyModel>();
			PartyVO p = new PartyVO(1);
			p.tutorial = true;
			p.turns = p.turnsLeft = 10;
			p.invited = true;
			p.invitationDistance = 1;
			p.Date = AmbitionApp.GetModel<CalendarModel>().Today;
			pmod.Party = p;

			MapModel mmod = AmbitionApp.GetModel<MapModel>();
			mmod.Map = mmod.Maps[TUTORIAL_MAP_ID];

			AmbitionApp.GetModel<EventModel>().SelectedEvent = null;
			AmbitionApp.UnregisterCommand<StartTutorialCmd>(GameMessages.START_TUTORIAL);
			AmbitionApp.RegisterCommand<CreateInvitationsCmd, DateTime>();
			AmbitionApp.SendMessage<string>(GameMessages.LOAD_SCENE, SceneConsts.GAME_PARTYLOADOUT); 
		}
	}
}