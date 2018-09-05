using System.Collections.Generic;
using Core;

namespace Ambition
{
	public class RSVPCmd : ICommand<PartyVO>
	{
		public void Execute (PartyVO party)
		{
			PartyModel model = AmbitionApp.GetModel<PartyModel>();
			CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            List<PartyVO> parties;
            if (!calendar.Parties.TryGetValue(party.Date, out parties))
            {
                parties = new List<PartyVO>(){party};
                calendar.Parties.Add(party.Date, parties);
            }
            else if (!parties.Contains(party))
            {
                parties.Add(party);
            }
            

			if (party.RSVP == -1)
			{
                int rep = (calendar.Today == party.Date) ? 40 : 20;
				AmbitionApp.SendMessage(new AdjustFactionVO(party.Faction, -rep));
				AmbitionApp.GetModel<GameModel>().Reputation -= rep;
			}
			AmbitionApp.SendMessage(PartyMessages.PARTY_UPDATE, party);
            if (party.Date == calendar.Today && party.RSVP == 1)
			{
                if (model.Party != null && model.Party != party) // Cancel previously accepted party!
                {
                    model.Party.RSVP = -1;
                    AmbitionApp.SendMessage(PartyMessages.PARTY_UPDATE, model.Party);
                }
                model.Party = party;
			}
		}
	}
}
