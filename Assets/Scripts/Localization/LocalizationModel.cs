using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;


namespace Ambition
{
    [Core.Saveable]
    public class LocalizationModel : Core.Model
    {
        private const string LOC_DIR = "Localization/";

        public Dictionary<string, string> Substitutions = new Dictionary<string, string>();

        public string HeaderTitlePhrase;

        public void Update()
        {
            SetPlayerName(AmbitionApp.Game.PlayerName);
            SetMoment(AmbitionApp.Story.Moment);
            SetCurrentQuest(AmbitionApp.Gossip.Quests?.Count > 0 ? AmbitionApp.Gossip.Quests[0] : null);
            SetParty(AmbitionApp.GetModel<PartyModel>().Party);
            SetPartyOutfit(AmbitionApp.Inventory.GetEquippedItem(ItemType.Outfit) as OutfitVO);
            SetDate(AmbitionApp.Calendar.Today);
        }

        public void SetPlayerName(string name) => Substitutions[LocalizationConsts.PLAYER_NAME] = name;

        public void SetMoment(MomentVO moment)
        {
            Substitutions[LocalizationConsts.CHARACTER_1] = moment?.Character1.Name;
            Substitutions[LocalizationConsts.CHARACTER_2] = moment?.Character2.Name;
            switch(moment?.Speaker)
            {
                case SpeakerType.Character1:
                    Substitutions[LocalizationConsts.SPEAKER] = moment.Character1.Name;
                    break;
                case SpeakerType.Character2:
                    Substitutions[LocalizationConsts.SPEAKER] = moment.Character2.Name;
                    break;
                case SpeakerType.Player:
                    Substitutions[LocalizationConsts.SPEAKER] = Substitutions[LocalizationConsts.PLAYER_NAME];
                    break;
                default:
                    Substitutions[LocalizationConsts.SPEAKER] = "";
                    break;
            }
        }

        public void SetCurrentQuest( QuestVO quest )
        {
            Dictionary<string, string> subs = new Dictionary<string, string>()
            {
                ["%t"]=quest.Reward.Type.ToString(),
                ["%v"]= quest.Reward.Value.ToString()
            };
            CommodityVO[] rewards;
            Substitutions[LocalizationConsts.QUESTGOSSIPFACTION] = AmbitionApp.Localize(quest.Faction.ToString().ToLower());
            Substitutions[LocalizationConsts.QUESTTIME] = (quest.Due - quest.Created).ToString();
            for (int tier = AmbitionApp.Gossip.RewardTiers.Length - 1; tier >= 0; --tier)
            {
                rewards = AmbitionApp.Gossip.RewardTiers[tier].Rewards;
                if (Array.Exists(rewards, t => t.Type == quest.Reward.Type && t.Value == quest.Reward.Value))
                {
                    Substitutions[LocalizationConsts.QUESTREWARD] = AmbitionApp.Localize(GossipConsts.QUEST_REWARD_LOC + quest.Reward.Type.ToString().ToLower() + "." + tier, subs);
                }
            }
        }

        public string GetPartyName(PartyVO party)
        {
            //Dictionary<string, string> subs = new Dictionary<string, string>();
            //CharacterVO character = AmbitionApp.GetModel<CharacterModel>().GetCharacter(party.Host);
            //if (party.phrases.Length > 0)
            //    subs["$REASON"] = AmbitionApp.Localize(PartyConstants.PARTY_REASON + party.Faction.ToString().ToLower() + "." + party.phrases[0]);
            //subs["$IMPORTANCE"] = AmbitionApp.Localize(PartyConstants.PARTY_IMPORTANCE + (int)party.Size);
            //subs["$HOST"] = GetFormalName(character, party.Host);
            string result = AmbitionApp.Localize(PartyConstants.PARTY_NAME + party.ID);
            return string.IsNullOrEmpty(result)
                ? AmbitionApp.Localize(PartyConstants.PARTY_REASON + party.Faction.ToString().ToLower() + "." + party.phrases[0])
                //? AmbitionApp.Localize(PartyConstants.PARTY_NAME_DEFAULT, subs)
                : result;
        }

        public string GetPartyInvitation(PartyVO party, CharacterVO host)
        {
            Dictionary<string, string> subs = new Dictionary<string, string>();
            string result;

            subs["$HOST"] = GetFormalName(host, party.Host);
            if (party.phrases?.Length > 1)
                subs["$INTRO"] = AmbitionApp.Localize(PartyConstants.PARTY_FLUFF_INTRO + party.phrases[0]);
            if (party.phrases?.Length > 2)
                subs["$ADJECTIVE"] = AmbitionApp.Localize(PartyConstants.PARTY_FLUFF_ADJECTIVE + party.phrases[1]);
            if (party.phrases?.Length > 3)
                subs["$NOUN"] = AmbitionApp.Localize(PartyConstants.PARTY_FLUFF_NOUN + party.phrases[2]);
            result = AmbitionApp.Localize(PartyConstants.PARTY_FLUFF, subs);
            subs.Clear();
            subs["$FLUFF"] = result;
            subs["$PLAYER"] = AmbitionApp.Game.PlayerName;
            subs["$PRONOUN"] = host == null || host.Gender == Gender.NonBinary || host.Gender == Gender.Unknown
                ? AmbitionApp.Localize("their")
                : host.Gender == Gender.Male
                ? AmbitionApp.Localize("his")
                : AmbitionApp.Localize("her");
            subs["$DATE"] = Localize(AmbitionApp.Calendar.StartDate.AddDays(party.Day));
            subs["$SIZE"] = AmbitionApp.Localize(PartyConstants.PARTY_SIZE + (int)party.Size);
            subs["$PARTY"] = GetPartyName(party);

            result = AmbitionApp.Localize(PartyConstants.PARTY_INVITATION + party.ID, subs);
            return string.IsNullOrEmpty(result)
                ? AmbitionApp.Localize(PartyConstants.PARTY_INVITATION_LOC, subs)
                : result;
        }

        public void SetParty( PartyVO party )
        {
            AmbitionApp.GetModel<FactionModel>().Factions.TryGetValue(party?.Faction ?? FactionType.None, out FactionVO faction);
            Substitutions[LocalizationConsts.PARTYFACTION] = AmbitionApp.Localize( faction.Type.ToString().ToLower() );

            // map -100 to +100 to 0,1,2.
            int modIndex = 0;
            if (faction.Modesty < -33)
            {
                modIndex = 0;
            }
            else if (faction.Modesty < 33)
            {
                modIndex = 1;
            }
            else
            {
                modIndex = 2;
            }

            int luxIndex = 0;
            if (faction.Luxury < -33)
            {
                luxIndex = 0;
            }
            else if (faction.Luxury < 33)
            {
                luxIndex = 1;
            }
            else
            {
                luxIndex = 2;
            }
            Substitutions[LocalizationConsts.PARTYFACTIONMODESTY] = AmbitionApp.Localize( string.Format("modesty.{0}", modIndex) );
            Substitutions[LocalizationConsts.PARTYFACTIONLUXURY] = AmbitionApp.Localize( string.Format("luxury.{0}", luxIndex) );
        }

        public void SetPartyOutfit(OutfitVO outfit)
        {
            Substitutions[LocalizationConsts.OUTFITNAME] = GetItemName(outfit);
            Substitutions[LocalizationConsts.OUTFITTITLE] = GetItemName(outfit);
            Substitutions[LocalizationConsts.OUTFITMODESTY] = GetStatText(outfit, ItemConsts.MODESTY);
            Substitutions[LocalizationConsts.OUTFITLUXURY] = GetStatText(outfit, ItemConsts.LUXURY);
        }

        public string GetItemName(ItemVO item)
        {
            return item == null ? ""
                : string.IsNullOrEmpty(item.Name)
                ? AmbitionApp.Localize(item.ID + ItemConsts.ITEM_LOC_NAME)
                : item.Name;
        }

        public string GetStatText(OutfitVO outfit, string stat)
        {
            if (outfit == null) return null;
            Dictionary<string, string> phrases = AmbitionApp.GetPhrases("outfit." + stat);
            if (phrases.Count == 0) return "";
            int value = outfit.GetIntStat(stat);
            switch (stat)
            {
                case ItemConsts.NOVELTY:
                    value = (int)(.099f * phrases.Count * value);
                    break;
                case ItemConsts.MODESTY:
                case ItemConsts.LUXURY:
                    value = (int)Mathf.Floor(.00499f * phrases.Count * (value + 100));
                    break;
            }
            return new List<string>(phrases.Values)[value];
        }

        public string SetDate(DateTime date) => Substitutions[LocalizationConsts.DATE] = Localize(date);

        public string Localize(DateTime date)
        {
            Dictionary<string, string> subs = new Dictionary<string, string>();
            Substitutions[LocalizationConsts.MONTH] = AmbitionApp.GetPhrases("month")["month." + (date.Month - 1)];
            Substitutions[LocalizationConsts.DAY] = date.Day.ToString();
            Substitutions[LocalizationConsts.YEAR] = date.Year.ToString();
            Substitutions[LocalizationConsts.WEEKDAY] = AmbitionApp.GetPhrases("weekday")["weekday." + (int)(date.DayOfWeek)];
            Substitutions[LocalizationConsts.SHORT_DATE] = AmbitionApp.Localize("short_date", Substitutions);
            return AmbitionApp.Localize("date", Substitutions);
        }

        public string Date => Substitutions[LocalizationConsts.DATE];
        public string ShortDate => Substitutions[LocalizationConsts.SHORT_DATE];

        public string SetLocation(string locationID)
        {
            return Substitutions[LocalizationConsts.LOCATION] = "location." + locationID;
        }

        public string GetCharacterName(string characterID)
        {
            CharacterVO character = AmbitionApp.GetModel<CharacterModel>().GetCharacter(characterID);
            return character != null
                   ? AmbitionApp.Localize(CharacterConsts.LOC_NAME + character.ID)
                   : characterID;
        }

        public string GetCharacterName(CharacterVO character)
        {
            return AmbitionApp.Localize(CharacterConsts.LOC_NAME + character.ID);
        }

        public string GetFullName(CharacterVO character)
        {
            return AmbitionApp.Localize(CharacterConsts.LOC_FULL_NAME + character.ID);
        }

        public string GetFormalName(CharacterVO character, string defaultName)
        {
            string result = character == null
                ? null
                : AmbitionApp.Localize(CharacterConsts.LOC_FORMAL_NAME + character.ID);
            return string.IsNullOrEmpty(result) ? defaultName : result;
        }

        public string GetShortName(CharacterVO character, string defaultName)
        {
            string result = character == null
                ? null
                : AmbitionApp.Localize(CharacterConsts.LOC_SHORT_NAME + character.ID);
            return string.IsNullOrEmpty(result) ? defaultName : result;
        }

        public string GetServantName(ServantVO servant)
        {
            return AmbitionApp.Localize(ServantConsts.SERVANT_LOC_KEY + servant.ID);
        }

        public string GetServantTitle(ServantVO servant)
        {
            return AmbitionApp.Localize(ServantConsts.SERVANT_TITLE_KEY + servant.ID);
        }

        public string GetServantDescription(ServantVO servant)
        {
            return AmbitionApp.Localize(ServantConsts.SERVANT_DESCRIPTION_KEY + servant.ID);
        }
    }
}
