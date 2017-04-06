using UnityEngine.SceneManagement;
using System;
using Core;

namespace DeWinter
{
	public class LoadSceneCmd : ICommand<string>
	{
		public void Execute (string sceneName)
		{
			SceneManager.LoadScene(sceneName);
		}
	}
}