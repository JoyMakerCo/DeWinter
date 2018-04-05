using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

namespace Ambition
{
	public class TutorialLoadoutCmd : ICommand<string>
	{
		public void Execute(string windowID)
		{
			if (windowID == IncidentView.DIALOG_ID)
			{
				AmbitionApp.UnregisterCommand<TutorialLoadoutCmd, string>(GameMessages.DIALOG_CLOSED);
				AmbitionApp.SendMessage<string>(GameMessages.LOAD_SCENE, SceneConsts.GAME_PARTYLOADOUT);
			}
		}
	}
}
