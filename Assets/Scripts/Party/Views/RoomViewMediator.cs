using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using Dialog;

namespace Ambition
{
	public class RoomViewMediator : MonoBehaviour
	{
	    // Poll for input
	    void Update()
	    {
			if(Input.GetKeyDown(KeyCode.D))
	        {
	        	AmbitionApp.SendMessage(PartyMessages.DRINK);
	        }
			else if (Input.GetKeyDown(KeyCode.C))
	        {
				AmbitionApp.SendMessage(PartyMessages.BUY_REMARK);
	        }
        }
	}
}
