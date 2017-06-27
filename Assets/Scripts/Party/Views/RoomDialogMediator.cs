using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using Dialog;

namespace Ambition
{
	public class RoomDialogMediator : DialogView, IDialog<RoomVO>
	{
		public void OnOpen(RoomVO room)
		{
			
		}

	    // Poll for input
	    void Update()
	    {
			if(Input.GetKeyDown(KeyCode.D))
	        {
	        	AmbitionApp.SendMessage(PartyMessages.DRINK);
	        }
	        else if (Input.GetKeyDown(KeyCode.F))
	        {
				AmbitionApp.SendMessage(PartyMessages.FLIP_REMARK);
	        }
			else if (Input.GetKeyDown(KeyCode.C))
	        {
				AmbitionApp.SendMessage(PartyMessages.BUY_REMARK);
	        }
			else if (Input.GetKeyDown(KeyCode.E))
	        {
				AmbitionApp.SendMessage(PartyMessages.EXCHANGE_REMARK, AmbitionApp.GetModel<PartyModel>().Remark);
	        }
        }
	}
}
