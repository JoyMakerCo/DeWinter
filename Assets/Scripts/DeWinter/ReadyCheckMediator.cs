using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DeWinter
{
	public class ReadyCheckMediator : MonoBehaviour
	{
		public GameObject[] CheckObjects;

		void Update ()
		{
			foreach(GameObject obj in CheckObjects)
			{
				if (!obj.activeInHierarchy)
					return;
			}
			string sceneName = SceneManager.GetActiveScene().name;
			DeWinterApp.SendMessage<string>(GameMessages.SCENE_READY, sceneName);
			GameObject.Destroy(this.gameObject);
		}
	}
}