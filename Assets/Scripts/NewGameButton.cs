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
			AmbitionApp.RegisterTransition("GameController", "InitGame", "StartGame");
			AmbitionApp.InvokeMachine("GameController");
			if (!Input.GetKey(KeyCode.LeftAlt))
			{
				IncidentModel emod = AmbitionApp.GetModel<IncidentModel>();
				emod.Config = emod.FindEvent("Yvette");
			}
		}
	}
}
