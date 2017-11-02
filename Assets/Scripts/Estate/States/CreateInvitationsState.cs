using System;
using System.Linq;
using System.Collections.Generic;
using UFlow;

namespace Ambition
{
	public class CreateInvitationsState : UState
	{
		private CalendarModel _calendar;
		public override void OnEnterState ()
		{
			_calendar = AmbitionApp.GetModel<CalendarModel>();
			DateTime day = _calendar.Today;
			PartyVO[] parties = Array.FindAll(AmbitionApp.GetModel<PartyModel>().Parties, p=>p.InvitationDate == _calendar.Today);
			Random rnd = new Random();

			Array.ForEach(parties, AddParty);

			if (!_calendar.Parties.ContainsKey(day))
			{
				CreateParty(day);
			}

			if (rnd.Next(3) == 0) // Chance of a random future engagement
			{
				CreateParty(day.AddDays(rnd.Next(1,8)+rnd.Next(1,8)));
			}

	       	// Missed Parties
	       	// TODO: Create a new state for this
	       	List<PartyVO> missed;
			if (_calendar.Parties.TryGetValue(_calendar.Yesterday, out missed))
			{
				foreach (PartyVO party in missed)
				{
					AmbitionApp.GetModel<GameModel>().Reputation -= 40;

					Dictionary<string, string> subs = new Dictionary<string, string>(){
						{"$PARTYNAME",party.Name}};
			    	AmbitionApp.OpenMessageDialog(DialogConsts.MISSED_RSVP_DIALOG, subs);
				}
			}
		}

		private void CreateParty(DateTime date)
		{
			PartyVO party =  new PartyVO();
			party.Date = date;
			party.InvitationDate = _calendar.Today;
			AddParty(party);
		}

		private void AddParty(PartyVO party)
		{
			Random rnd = new Random();

			if (party.Faction == null)
			{
				FactionModel fmod = AmbitionApp.GetModel<FactionModel>();
				string[] factions = fmod.Factions.Keys.ToArray();
				party.Faction = factions[new Random().Next(factions.Length)];
			}

			if (party.Importance == 0) party.Importance = rnd.Next(1,4);
			if (party.Turns == 0) party.Turns = (party.Importance * 5) + 1;

			string str = (party.ID != null) ? AmbitionApp.GetString("party.name." + party.ID) : null;
			party.Name = (str != null) ? str :  AmbitionApp.GetString("party.name.default",
				new Dictionary<string, string>(){
					{"%IMPORTANCE", AmbitionApp.GetString("party_importance." + party.Importance.ToString())},
					{"%FACTION", AmbitionApp.GetString("faction." + party.Faction)}
				});

			str = (party.ID != null) ? AmbitionApp.GetString("party.description." + party.ID) : null;
			party.Description = (str != null) ? str :  AmbitionApp.GetString("party.description.default");
			party.Description = str;
			if (!_calendar.Parties.ContainsKey(party.Date))
			{
				_calendar.Parties.Add(party.Date, new List<PartyVO>{party});
				AmbitionApp.SendMessage<PartyVO>(party);
			}
			else if (!_calendar.Parties[party.Date].Contains(party))
			{
				_calendar.Parties[party.Date].Add(party);
				AmbitionApp.SendMessage<PartyVO>(party);
			}
		}
	}
}
