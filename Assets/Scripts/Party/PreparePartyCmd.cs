using System;
using System.Collections.Generic;
using Core;

namespace DeWinter
{
	public class PreparePartyCmd : ICommand
	{
		public void Execute()
		{
			CalendarModel model = DeWinterApp.GetModel<CalendarModel>();
			List<Party> parties;
	        if (model.Parties.TryGetValue(model.Today, out parties))
	        {
	        	parties = parties.FindAll(x => x.RSVP == 1);
	        	switch (parties.Count)
	        	{
	        		case 1:
	        			DeWinterApp.GetModel<PartyModel>().Party = parties[0];
						UnityEngine.Debug.Log("Tonights Party is: a " + parties[0].SizeString() + " " + parties[0].faction + " Party");
	        			break;
	        		case 2:
						DeWinterApp.OpenDialog<Party []>("TwoPartyRSVPdPopUpModal", new Party[]{parties[0], parties[1]});
		    			break;    			
	        	}
		    }
		}
	}
}