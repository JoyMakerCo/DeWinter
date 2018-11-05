using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
	public class TutorialCanvasMediator : MonoBehaviour
	{
		public GameObject TutorialPrefab;
		void Awake ()
		{
			if (TutorialPrefab != null && AmbitionApp.IsActiveMachine(TutorialConsts.TUTORIAL_MACHINE))
				Instantiate(TutorialPrefab, transform, false);
			Destroy(this);
		}
	}
}
