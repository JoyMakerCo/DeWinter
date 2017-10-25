using System;
using System.Collections.Generic;
using UFlow;

namespace Ambition
{
	public class ShowInvitationsState : UState
	{
		public override void OnEnterState ()
		{
			CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
			List<PartyVO> parties;
			if (calendar.Parties.TryGetValue(calendar.Today, out parties))
			{
				PartyVO party = parties.Find(p=>!p.invited);
				if (party != null)
					AmbitionApp.OpenDialog<PartyVO>(DialogConsts.RSVP, party);
					party.invited = true;
			}
		}
	}
}
