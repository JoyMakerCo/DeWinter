using System;
using System.Collections.Generic;
using Core;

namespace Ambition
{
	public class PreparePartiesCmd : ICommand<DateTime>
	{
		public const int ADVANCE_RSVP_DAYS = 15;

		public void Execute (DateTime day)
		{
			CalendarModel cmod = AmbitionApp.GetModel<CalendarModel>();
			GameModel gmod = AmbitionApp.GetModel<GameModel>();
			FactionModel  fmod = AmbitionApp.GetModel<FactionModel>();

        	List<PartyVO> parties;

	        // New Party Invites
			for (int i = 0; i < ADVANCE_RSVP_DAYS; i++)
	        {
				if (cmod.Parties.TryGetValue(cmod.DaysFromNow(i), out parties))
	        	{
	        		parties = parties.FindAll(p =>
	        			i <= p.invitationDistance
	        			&& !p.invited
	        			&& p.partySize > 0
	        			&& (p.partySize <= fmod[p.faction].LargestAllowableParty || p.partySize <= gmod.PartyInviteImportance));
	        		foreach (PartyVO party in parties)
	        		{
	        			Dictionary<string,string> subs = new Dictionary<string, string>(){
							{"$HOSTNAME", party.host.Name},
							{"$FACTION",party.faction},
							{"$SIZE",party.SizeString()}};
						AmbitionApp.OpenMessageDialog(DialogConsts.INVITATION_DIALOG, subs);

	                    //Actually Inviting the Player
	                    party.invited = true;
		    		}
	        	}
	        }

	       	// Missed Parties
			if (cmod.Parties.TryGetValue(cmod.Yesterday, out parties))
			{
				foreach (PartyVO party in parties)
				{
					if (party.RSVP == 0 && fmod[party.faction].LargestAllowableParty >= party.partySize)
					{
						AmbitionApp.AdjustValue<int>(GameConsts.REPUTATION, -40);

						Dictionary<string, string> subs = new Dictionary<string, string>(){
							{"$PARTYNAME",party.Name()}};
				    	AmbitionApp.OpenMessageDialog(DialogConsts.MISSED_RSVP_DIALOG, subs);
					}
				}
			}
		}
	}
}