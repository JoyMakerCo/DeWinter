using System.Collections;
using System.Collections.Generic;
using UFlow;

namespace Ambition
{
	public class StartGameState : UState
	{
		public override void OnEnterState ()
		{
			EventModel emod = AmbitionApp.GetModel<EventModel>();
			emod.Event = emod.eventInventories["intro"][0];

			AmbitionApp.SendMessage<string>(GameMessages.LOAD_SCENE, "Game_Estate");
		}
	}
}
