using System;
using System.Linq;
using System.Collections.Generic;
using UFlow;
using Core;

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

			Array.ForEach(parties, AddParty);

			if (!_calendar.Parties.ContainsKey(day))
			{
				CreateParty(day);
			}

			if (Util.RNG.Generate(0,3) == 0) // Chance of a random future engagement
			{
				CreateParty(day.AddDays(Util.RNG.Generate(1,8)+Util.RNG.Generate(1,8)));
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
			CharacterModel characters = AmbitionApp.GetModel<CharacterModel>();
// TODO: More robust host system
party.Host = characters.Notables[Util.RNG.Generate(0,characters.Notables.Length)];

			if (party.Faction == null)
			{
				FactionModel fmod = AmbitionApp.GetModel<FactionModel>();
				string[] factions = fmod.Factions.Keys.ToArray();
				party.Faction = factions[Util.RNG.Generate(0,factions.Length)];
			}

			if (party.Importance == 0) party.Importance = Util.RNG.Generate(1,4);
			if (party.Turns == 0) party.Turns = (party.Importance * 5) + 1;

			string str = (party.ID != null) ? AmbitionApp.GetString("party.description." + party.ID) : null;
			party.Description = (str != null) ? str :  GetRandomText("party_reason."+party.Faction);

			str = (party.ID != null) ? AmbitionApp.GetString("party.name." + party.ID) : null;
			party.Name = (str != null) ? str :  AmbitionApp.GetString("party.name.default",
				new Dictionary<string, string>(){
					{"$HOST", party.Host.Name},
					{"$IMPORTANCE", AmbitionApp.GetString("party_importance." + party.Importance.ToString())},
					{"$REASON", party.Description}
				});

			str = AmbitionApp.GetString("party_fluff", new Dictionary<string, string>(){
				{"$INTRO",GetRandomText("party_fluff_intro")},
				{"$ADJECTIVE",GetRandomText("party_fluff_adjective")},
				{"$NOUN",GetRandomText("party_fluff_noun")}});
			party.Invitation = AmbitionApp.GetString("party_invitation", new Dictionary<string, string>(){
				{"$PLAYER", AmbitionApp.GetModel<GameModel>().PlayerName},
				{"$PRONOUN", AmbitionApp.GetString(party.Host.Gender == Gender.Female ? "her" : "his")},
				{"$PARTY",party.Description},
				{"$DATE", AmbitionApp.GetModel<CalendarModel>().GetDateString(party.Date)},
				{"$SIZE", AmbitionApp.GetString("party_importance." + party.Importance.ToString())},
				{"$FLUFF", str}});
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

		private string GetRandomText(string key)
		{
			string [] phrases = AmbitionApp.GetPhrases(key);
			return phrases[Util.RNG.Generate(0, phrases.Length)];
		}
	}
}
