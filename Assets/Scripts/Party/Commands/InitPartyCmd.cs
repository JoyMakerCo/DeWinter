using System;
using System.Collections.Generic;
using Core;
using Util;
using UnityEngine;
namespace Ambition
{
    public class InitPartyCmd : ICommand<PartyVO>
    {
        public void Execute(PartyVO party)
        {
            CharacterModel characters = AmbitionApp.GetModel<CharacterModel>();
            GameModel game = AmbitionApp.Game;
            CalendarModel calendar = AmbitionApp.Calendar;
            IncidentModel story = AmbitionApp.Story;
            PartyModel model = AmbitionApp.GetModel<PartyModel>();

            if (string.IsNullOrEmpty(party.ID) || !model.LoadParty(party.ID, out party))
            {
                party.ID = null; 
                if (party.Faction == FactionType.None)
                {
                    List<FactionType> factions = new List<FactionType>(AmbitionApp.Politics.Factions.Keys);
                    factions.Remove(FactionType.None);
                    party.Faction = RNG.TakeRandom(factions);
                }
            }

            if (string.IsNullOrEmpty(party.Host))
            {
                string gender = RNG.Generate(2) < 1 ? "male" : "female";
                string[] host = new string[3];
                IEnumerable<string> locs = AmbitionApp.GetPhrases(gender + "_title").Values;
                host[0] = RNG.TakeRandom(locs);
                locs = AmbitionApp.GetPhrases(gender + "_name").Values;
                host[1] = RNG.TakeRandom(locs);
                locs = AmbitionApp.GetPhrases("last_name").Values;
                host[2] = RNG.TakeRandom(locs);
                party.Host = string.Join(" ", host);
            }

            if (party.Size == PartySize.None)
            {
                ChapterVO chapter = AmbitionApp.Game.GetChapter();
                int chance = RNG.Generate(chapter.TrivialPartyChance + chapter.DecentPartyChance + chapter.GrandPartyChance);
                if (chance < chapter.GrandPartyChance)
                {
                    party.Size = PartySize.Grand;
                }
                else if (chance < chapter.DecentPartyChance + chapter.GrandPartyChance)
                {
                    party.Size = PartySize.Decent;
                }
                else party.Size = PartySize.Trivial;
            }

            if (party.phrases?.Length != 4)
            {
                party.phrases = new int[4];
                party.phrases[0] = GetRandomPhrase(PartyConstants.PARTY_REASON + party.Faction.ToString().ToLower());
                party.phrases[1] = GetRandomPhrase(PartyConstants.PARTY_FLUFF_INTRO);
                party.phrases[2] = GetRandomPhrase(PartyConstants.PARTY_FLUFF_ADJECTIVE);
                party.phrases[3] = GetRandomPhrase(PartyConstants.PARTY_FLUFF_NOUN);
            }

            switch (party.RSVP)
            {
                case RSVP.Accepted:
                case RSVP.Required:
                    AmbitionApp.SendMessage(PartyMessages.ACCEPT_INVITATION, party);
                    break;
                case RSVP.Declined:
                    AmbitionApp.SendMessage(PartyMessages.DECLINE_INVITATION, party);
                    break;
                default:
                    if (party.Day >= 0)
                    {
                        AmbitionApp.SendMessage(CalendarMessages.SCHEDULE, party);
                    }
                    break;
            }
        }

        private int GetRandomPhrase(string key) => RNG.Generate(AmbitionApp.GetPhrases(key).Count);
    }
}
