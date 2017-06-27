using UnityEngine.SceneManagement;
using System;
using Core;

namespace Ambition
{
	public class LoadSceneCmd : ICommand<string>
	{
		public void Execute (string sceneName)
		{
			SceneManager.LoadScene(sceneName);
			// TODO: Make commands able to persist with a callback
		}
	}
}