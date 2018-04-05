using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
	public class TutorialOnly : MonoBehaviour
	{
		void Awake ()
		{
			if (AmbitionApp.IsActiveState("TutorialController"))
				GameObject.Destroy(this);
			else
				GameObject.Destroy(this.gameObject);
		}
	}
}
