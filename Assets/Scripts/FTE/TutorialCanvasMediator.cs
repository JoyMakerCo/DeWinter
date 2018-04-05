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
			if (TutorialPrefab != null && AmbitionApp.IsActiveState(TutorialConsts.TUTORIAL))
				Instantiate(TutorialPrefab, this.transform);
			Destroy(this);
		}
	}
}
