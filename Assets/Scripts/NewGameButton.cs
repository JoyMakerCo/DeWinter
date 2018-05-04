using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ambition
{
	public class NewGameButton : MonoBehaviour
	{
		public void OnClick ()
		{
			AmbitionApp.RegisterState<FadeOutState>("NewGame");
			AmbitionApp.RegisterState<InitGameState>("InitGame");
			AmbitionApp.RegisterState("WaitForLoad");
			AmbitionApp.RegisterState<InvokeMachineState, string>("InvokeIncidentMachine", "IncidentController");
			AmbitionApp.RegisterState<InvokeMachineState, string>("InvokeEstateMachine", "EstateController");

			AmbitionApp.RegisterLink<WaitForMessageLink>("GameController", "NewGame", "InitGame", GameMessages.FADE_OUT_COMPLETE);
			AmbitionApp.RegisterLink<WaitForMessageLink>("GameController", "InitGame", "WaitForLoad", GameMessages.SCENE_LOADED);
			AmbitionApp.RegisterLink<CheckIncidentLink>("GameController", "WaitForLoad", "InvokeIncidentMachine");
			AmbitionApp.RegisterLink("GameController", "WaitForLoad", "InvokeEstateMachine");

			AmbitionApp.InvokeMachine("GameController");
#if (UNITY_EDITOR)			
			if (!Input.GetKey(KeyCode.LeftAlt))
#endif
			{
				IncidentModel emod = AmbitionApp.RegisterModel<IncidentModel>();
				emod.Incident = emod.FindEvent("Yvette");
			}
		}
	}
}
