using System;
using System.Collections.Generic;
using Core;
using UnityEngine;
namespace Ambition
{
    public class InitPartyCmd : ICommand<PartyVO>
    {
        public void Execute(PartyVO party)
        {
            CharacterModel characters = AmbitionApp.GetModel<CharacterModel>();
            GameModel game = AmbitionApp.GetModel<GameModel>();
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            PartyVO[] parties = model.GetParties(game.Convert(party.Date));

            // TODO: Assignable Character Configs for Notables
            if (string.IsNullOrEmpty(party.Host))
                party.Host = Util.RNG.TakeRandom(characters.Characters.Keys);

            string reason = GetRandomText("party_reason." + party.Faction.ToString().ToLower());

            party.Description = (!string.IsNullOrWhiteSpace(party.LocalizationKey))
                ? AmbitionApp.Localize("party.description." + party.LocalizationKey)
                : reason;

            if (string.IsNullOrEmpty(party.ID))
            {
                int index = Array.IndexOf(parties, party);
                if (index < 0) index = parties.Length;
                party.ID = party.Faction.ToString() + party.Date.ToString() + "S" + party.Size.ToString() + index.ToString();
            }

            if (!string.IsNullOrWhiteSpace(party.LocalizationKey))
            {
                party.Name = AmbitionApp.GetString("party.name." + party.LocalizationKey, 
                    new Dictionary<string, string>(){
                        {"$HOST", party.Host},
                        {"$IMPORTANCE", AmbitionApp.Localize("party_importance." + ((int)party.Size).ToString())},
                        {"$REASON", reason}});
            }

            if (string.IsNullOrEmpty(party.IntroIncident))
            {
                party.IntroIncident = game.DefaultIntroIncident;
            }

            string str = AmbitionApp.GetString("party_fluff", new Dictionary<string, string>(){
                {"$INTRO",GetRandomText("party_fluff_intro")},
                {"$ADJECTIVE",GetRandomText("party_fluff_adjective")},
                {"$NOUN",GetRandomText("party_fluff_noun")}});

            if (party.InvitationDate.Equals(default))
            {
                party.InvitationDate = game.Date;
            }

            if (string.IsNullOrWhiteSpace(party.Invitation))
            {
                party.Invitation = AmbitionApp.GetString("party.invitation." + party.LocalizationKey, new Dictionary<string, string>(){
                    {"$PLAYER", AmbitionApp.GetModel<GameModel>().PlayerName},
                    //{"$PRONOUN", AmbitionApp.GetString(party.Host.Gender == Gender.Female ? "her" : "his")},
                    {"$PRONOUN", AmbitionApp.Localize("their")}, // TODO
                    {"$PARTY",party.Description},
                    {"$DATE", AmbitionApp.GetModel<LocalizationModel>().Date },
                    {"$SIZE", AmbitionApp.Localize("party_importance." + ((int)party.Size).ToString())},
                    {"$FLUFF", str}});
            }

            // string substitutions for the party
			AmbitionApp.GetModel<LocalizationModel>().SetPartyFaction( party.Faction );

            // Random Faction
            if (party.Faction == FactionType.Neutral)
                party.Faction = Util.RNG.TakeRandomExcept(AmbitionApp.GetModel<FactionModel>().Factions.Keys, FactionType.Neutral);

            model.Schedule(party);
        }

        private string GetRandomText(string phrase)
        {
            return Util.RNG.TakeRandom(AmbitionApp.GetPhrases(phrase).Values);
        }
    }
}
