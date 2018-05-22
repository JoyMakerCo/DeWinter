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
			AmbitionApp.RegisterState<FadeOutState>("GameController", "NewGame");
			AmbitionApp.RegisterState<InitGameState>("GameController", "InitGame");
			AmbitionApp.RegisterState("GameController", "WaitForLoad");
			AmbitionApp.RegisterState<InvokeMachineState, string>("GameController", "InvokeIncidentMachine", "IncidentController");
			AmbitionApp.RegisterState<InvokeMachineState, string>("GameController", "InvokeEstateMachine", "EstateController");

			AmbitionApp.RegisterLink<AmbitionDelegateLink, string>("GameController", "NewGame", "InitGame", GameMessages.FADE_OUT_COMPLETE);
			AmbitionApp.RegisterLink<WaitForSceneLoadedState, string>("GameController", "InitGame", "WaitForLoad", SceneConsts.ESTATE_SCENE);
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
