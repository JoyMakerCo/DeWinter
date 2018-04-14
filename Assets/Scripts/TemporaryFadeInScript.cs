using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Eliminate this in favor of the state machine
namespace Ambition
{
	public class TemporaryFadeInScript : MonoBehaviour {

		// Use this for initialization
		void Start ()
		{
			AmbitionApp.SendMessage(GameMessages.FADE_IN);
			Destroy(this);
		}
	}
}
