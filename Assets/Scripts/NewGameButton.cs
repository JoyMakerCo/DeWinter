using System;
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
			AmbitionApp.RegisterState<InvokeMachineState, string>("GameController", "InvokeIncidentMachine", "IncidentController");
			AmbitionApp.RegisterState<InvokeMachineState, string>("GameController", "InvokeEstateMachine", "EstateController");

			AmbitionApp.RegisterLink<AmbitionDelegateLink, string>("GameController", "NewGame", "InitGame", GameMessages.FADE_OUT_COMPLETE);
			AmbitionApp.RegisterLink<CheckIncidentLink>("GameController", "InitGame", "InvokeIncidentMachine");
			AmbitionApp.RegisterLink("GameController", "InitGame", "InvokeEstateMachine");

            AmbitionApp.InvokeMachine("GameController");

#if (UNITY_EDITOR)			
			if (!Input.GetKey(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.RightAlt))
#endif
			{
                IncidentModel emod = AmbitionApp.RegisterModel<IncidentModel>();
                emod.Incident = emod.Find("Yvette's Prologue");
			}

            AmbitionApp.InvokeMachine("GameController");
		}
	}
}
