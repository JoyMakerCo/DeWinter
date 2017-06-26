using System;
using Core;

namespace Ambition
{
	public class StartTutorialCmd : ICommand
	{
		private const string TUTORIAL_MAP_ID = "Tutorial";
		public void Execute ()
		{
			PartyModel pmod = DeWinterApp.GetModel<PartyModel>();
			Party p = new Party(1);
			p.tutorial = true;
			p.turns = p.turnsLeft = 10;
			p.invited = true;
			p.invitationDistance = 1;
			p.Date = DeWinterApp.GetModel<CalendarModel>().Today;
			pmod.Party = p;

			MapModel mmod = DeWinterApp.GetModel<MapModel>();
			mmod.Map = mmod.Maps[TUTORIAL_MAP_ID];

			DeWinterApp.GetModel<EventModel>().SelectedEvent = null;
			DeWinterApp.UnregisterCommand<StartTutorialCmd>(GameMessages.START_TUTORIAL);
			DeWinterApp.RegisterCommand<CreateInvitationsCmd, DateTime>();
			DeWinterApp.SendMessage<string>(GameMessages.LOAD_SCENE, SceneConsts.GAME_PARTYLOADOUT); 
		}
	}
}