using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
	public class NewGameButton : MonoBehaviour
	{
		public void OnClick ()
		{
			AmbitionApp.RegisterState<InitGameState>("InitGame");
			AmbitionApp.RegisterState<StartGameState>("StartGame");
			AmbitionApp.RegisterLink<WaitForMessageLink>("GameController", "InitGame", "StartGame", GameMessages.FADE_OUT_COMPLETE);
			AmbitionApp.InvokeMachine("GameController");
#if (UNITY_EDITOR)			
			if (!Input.GetKey(KeyCode.LeftAlt))
#endif
			{
				IncidentModel emod = AmbitionApp.GetModel<IncidentModel>();
				emod.Incident = emod.FindEvent("Yvette");
			}
			AmbitionApp.SendMessage<string>(GameMessages.LOAD_SCENE, SceneConsts.GAME_ESTATE);
		}
	}
}
