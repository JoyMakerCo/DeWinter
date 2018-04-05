using UnityEngine.SceneManagement;
using System;
using Core;

namespace Ambition
{
	public class LoadSceneCmd : ICommand<string>
	{
		private string _scene;
		public void Execute (string sceneName)
		{
			_scene = sceneName;
			AmbitionApp.Subscribe(GameMessages.FADE_OUT_COMPLETE, HandleSwap);
			AmbitionApp.SendMessage(GameMessages.FADE_OUT);
		}

		private void HandleSwap()
		{
			SceneManager.LoadScene(_scene);
		}
	}
}