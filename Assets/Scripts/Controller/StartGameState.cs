using System.Collections;
using System.Collections.Generic;
using UFlow;

namespace Ambition
{
	public class StartGameState : UState
	{
		public override void OnEnterState ()
		{
			AmbitionApp.SendMessage<string>(GameMessages.LOAD_SCENE, "Game_Estate");
		}
	}
}
