using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Kill this class when Estate is a sub-machine of the main machine
namespace Ambition
{
	public class InvokeEstate : MonoBehaviour
	{
		// Use this for initialization
		void Start () {
			AmbitionApp.InvokeMachine("EstateController");
		}
	}
}
