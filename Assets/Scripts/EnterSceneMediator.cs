using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is suboptimal, but will become obselete when scenes are consolidated into prefabs
namespace Ambition
{
	public class EnterSceneMediator : MonoBehaviour
	{
		public GameObject [] StartupItems;
		public string ReadyMessageID;

		void Start()
		{
			StartCoroutine(ReadyCheck());
		}

		IEnumerator ReadyCheck()
		{
			bool ready;
			do
			{
				ready = true;
				foreach(GameObject obj in StartupItems)
				{
					ready = ready && obj.activeInHierarchy;
				}
				yield return null;
			}
			while (!ready);
			AmbitionApp.SendMessage(ReadyMessageID);
			Destroy(this.gameObject);
		}
	}
}
