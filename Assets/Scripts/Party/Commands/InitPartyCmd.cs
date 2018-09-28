using System;
using System.Collections.Generic;
using Core;

namespace Ambition
{
    public class InitPartyCmd : ICommand<PartyVO>
    {
        public void Execute(PartyVO party)
        {
            CharacterModel characters = AmbitionApp.GetModel<CharacterModel>();

            // TODO: Assignable Character Configs for Notables
            if (string.IsNullOrEmpty(party.Host))
                party.Host = Util.RNG.TakeRandom(characters.Notables).Name;

            party.Description = (party.ID != null)
                ? AmbitionApp.GetString("party.description." + party.ID)
                : GetRandomText("party_reason." + party.Faction);

            party.Name = (party.ID != null)
                ? AmbitionApp.GetString("party.name." + party.ID)
                : AmbitionApp.GetString("party.name.default",
                new Dictionary<string, string>(){
                    {"$HOST", party.Host},
                    {"$IMPORTANCE", AmbitionApp.GetString("party_importance." + ((int)party.Importance).ToString())},
                    {"$REASON", party.Description}});

            string str = AmbitionApp.GetString("party_fluff", new Dictionary<string, string>(){
                {"$INTRO",GetRandomText("party_fluff_intro")},
                {"$ADJECTIVE",GetRandomText("party_fluff_adjective")},
                {"$NOUN",GetRandomText("party_fluff_noun")}});

            party.Invitiation = AmbitionApp.GetString("party.invitation." + party.ID, new Dictionary<string, string>(){
                {"$PLAYER", AmbitionApp.GetModel<GameModel>().PlayerName},
                //{"$PRONOUN", AmbitionApp.GetString(party.Host.Gender == Gender.Female ? "her" : "his")},
                {"$PRONOUN", AmbitionApp.GetString("their")}, // TODO
                {"$PARTY",party.Description},
                {"$DATE", AmbitionApp.GetModel<CalendarModel>().GetDateString(party.Date)},
                {"$SIZE", AmbitionApp.GetString("party_importance." + ((int)party.Importance).ToString())},
                {"$FLUFF", str}});


            // Random Faction
            if (party.Faction == null)
                party.Faction = Util.RNG.TakeRandom(new List<string>(AmbitionApp.GetModel<FactionModel>().Factions.Keys).ToArray());

            if (party.Importance == PartySize.None) party.Importance = (PartySize)Util.RNG.Generate(1,4);
            if (party.Turns == 0) party.Turns = ((int)party.Importance * 5) + 1;

            AmbitionApp.GetModel<CalendarModel>().Schedule(party);
        }

        private string GetRandomText(string phrase)
        {
            string[] phrases = AmbitionApp.GetPhrases(phrase);
            return Util.RNG.TakeRandom(phrases);
        }
    }
}
