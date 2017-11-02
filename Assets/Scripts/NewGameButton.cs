﻿using System.Collections;
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
		}
	}
}