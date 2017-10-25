using System;
using System.Collections.Generic;
using UFlow;

namespace Ambition
{
	public class CheckInvitationsTransition : UTransition
	{
		public override bool InitializeAndValidate ()
		{
			CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
			List<PartyVO> parties;
			return calendar.Parties.TryGetValue(calendar.Today, out parties) && parties.Exists(p=>!p.invited);
		}
	}
}
