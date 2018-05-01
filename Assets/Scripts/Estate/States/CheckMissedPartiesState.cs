using UFlow;
using System;
using System.Collections.Generic;

namespace Ambition
{
    public class CheckMissedPartiesState : UState
    {
        override public void OnEnterState()
        {
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
	       	List<PartyVO> missed;
			if (calendar.Parties.TryGetValue(calendar.Yesterday, out missed))
			{
                missed = missed.FindAll(p=>p.RSVP == 0);
                AmbitionApp.GetModel<GameModel>().Reputation -= 40*missed.Count;
				foreach (PartyVO party in missed)
				{                    
					Dictionary<string, string> subs = new Dictionary<string, string>()
                    {{"$PARTYNAME",party.Name}};
			    	AmbitionApp.OpenMessageDialog(DialogConsts.MISSED_RSVP_DIALOG, subs);
				}
			}
        }
    }
}
