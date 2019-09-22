using System;
using System.Linq;
using System.Collections.Generic;
using Core;

namespace Ambition
{
    public class InitPartyCmd : ICommand<PartyVO>
    {
        public void Execute(PartyVO party)
        {
            CharacterModel characters = AmbitionApp.GetModel<CharacterModel>();
            GameModel model = AmbitionApp.GetModel<GameModel>();

            // TODO: Assignable Character Configs for Notables
            if (string.IsNullOrEmpty(party.Host))
                party.Host = Util.RNG.TakeRandom(characters.Characters.Keys.ToArray());

            string reason = GetRandomText("party_reason." + party.Faction.ToString().ToLower());

            party.Description = (!string.IsNullOrWhiteSpace(party.LocalizationKey))
                ? AmbitionApp.GetString("party.description." + party.LocalizationKey)
                : reason;

            if (!string.IsNullOrWhiteSpace(party.LocalizationKey))
            {
                party.Name = AmbitionApp.GetString("party.name." + party.LocalizationKey, 
                    new Dictionary<string, string>(){
                        {"$HOST", party.Host},
                        {"$IMPORTANCE", AmbitionApp.GetString("party_importance." + ((int)party.Size).ToString())},
                        {"$REASON", reason}});
            }

            if (party.Name == null) // No name = randomly generated party
            {
                if (party.IntroIncident == null)
                    party.IntroIncident = UnityEngine.Resources.Load<IncidentConfig>(model.DefaultIntroIncident)?.GetIncident();            }

            string str = AmbitionApp.GetString("party_fluff", new Dictionary<string, string>(){
                {"$INTRO",GetRandomText("party_fluff_intro")},
                {"$ADJECTIVE",GetRandomText("party_fluff_adjective")},
                {"$NOUN",GetRandomText("party_fluff_noun")}});

            if (string.IsNullOrWhiteSpace(party.Invitation))
            {
                party.Invitation = AmbitionApp.GetString("party.invitation." + party.LocalizationKey, new Dictionary<string, string>(){
                    {"$PLAYER", AmbitionApp.GetModel<GameModel>().PlayerName},
                    //{"$PRONOUN", AmbitionApp.GetString(party.Host.Gender == Gender.Female ? "her" : "his")},
                    {"$PRONOUN", AmbitionApp.GetString("their")}, // TODO
                    {"$PARTY",party.Description},
                    {"$DATE", AmbitionApp.GetModel<LocalizationModel>().Date },
                    {"$SIZE", AmbitionApp.GetString("party_importance." + ((int)party.Size).ToString())},
                    {"$FLUFF", str}});
            }

            // Random Faction
            if (party.Faction == FactionType.Neutral)
                party.Faction = Util.RNG.TakeRandomExcept(AmbitionApp.GetModel<FactionModel>().Factions.Keys.ToArray(), FactionType.Neutral);

            if (party.RequiredIncidents == null) party.RequiredIncidents = new IncidentVO[Util.RNG.Generate(1, 4)];

            AmbitionApp.GetModel<CalendarModel>().Schedule(party);
            AmbitionApp.SendMessage(party);
        }

        private string GetRandomText(string phrase)
        {
            string[] phrases = AmbitionApp.GetPhrases(phrase);
            return Util.RNG.TakeRandom(phrases);
        }
    }
}
