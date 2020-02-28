using UFlow;
using System.Linq;
using System.Collections.Generic;

namespace Ambition
{
    public class CheckMissedPartiesState : UState
    {
        public override void OnEnterState()
        {
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            PartyVO[] parties = calendar.GetEvents<PartyVO>(calendar.Yesterday).Where(p=>p.RSVP == RSVP.New).ToArray();
            AmbitionApp.GetModel<GameModel>().Reputation -= 40 * parties.Length;
            foreach (PartyVO party in parties)
			{                    
				Dictionary<string, string> subs = new Dictionary<string, string>()
                {{"$PARTYNAME",party.Name}};
		    	AmbitionApp.OpenMessageDialog(DialogConsts.MISSED_RSVP_DIALOG, subs);
			}
        }
    }
}
