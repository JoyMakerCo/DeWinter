using System;
using System.Collections.Generic;
using Core;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using UnityEngine;
using Util;

namespace Ambition
{
    [Saveable]
    public class GameModel : ObservableModel<GameModel>, IResettable
    {
        [JsonProperty("playerID")]
        public string playerID = null;

        [JsonProperty("livre")]
        public int Livre = 0;

        [JsonProperty("credibility")]
        public int Credibility = 0;

        [JsonProperty("exhaustion")]
        private int _exhaustion;
        public int Exhaustion
        {
            get => _exhaustion;
            set
            {
                if (value < 0) _exhaustion = -1;
                else if (value < MaxExhaustion) _exhaustion = value;
                else _exhaustion = MaxExhaustion;
                Broadcast();
                AmbitionApp.SendMessage(GameConsts.EXHAUSTION, _exhaustion);
            }
        }

        [JsonProperty("peril")]
        public int Peril = 0;

        [JsonProperty("perilous")]
        public bool Perilous;

        [JsonProperty("misc")]
        public Dictionary<string, int> Misc = new Dictionary<string, int>();

        [JsonProperty("tutorials")]
        public List<string> Tutorials = new List<string>();

        [JsonProperty("allegiance")]
        public FactionType Allegiance;

        [JsonProperty("activity")]
        public ActivityType Activity;

        [JsonIgnore]
        public string PlayerName => AmbitionApp.Localize(playerID + ".name");

        [JsonIgnore]
        public int PoliticalChance;

        [JsonIgnore]
        public int MissedPartyPenalty;

        [JsonIgnore] // Player-specific start date for party invitations
        public RequirementVO[] StartInvitationsReqirements = new RequirementVO[0];

        [JsonIgnore]
        public int[] Levels;

        [JsonIgnore]
        public bool IsWellRested => _exhaustion < 0;

        [JsonIgnore]
        public int WellRestedBonus;

        [JsonIgnore]
        public int[] ExhaustionPenalties;

        [JsonIgnore]
        public int MaxExhaustion => ExhaustionPenalties.Length-1;

        [JsonIgnore]
        public int ExhaustionPenalty => _exhaustion >= 0
            ? ExhaustionPenalties[_exhaustion]
            : WellRestedBonus;

        [JsonIgnore]
        public ChapterVO[] Chapters = new ChapterVO[0];

        [JsonIgnore]
        public int Chapter
        {
            get
            {
                DateTime today = AmbitionApp.Calendar.Today;
                for (int i = Chapters.Length-1; i>0; --i)
                {
                    if (Chapters[i].Date <= today) return i;
                }
                return 0;
            }
        }

        [JsonIgnore]
        public bool Initialized = false;

        [JsonIgnore]
        public int MaxSaves;

        [JsonIgnore]
        public int SaveSlotID = -1;

        public ChapterVO GetChapter() => Chapters[Chapter];

        public void Reset()
        {
            playerID = null;
            Chapters = new ChapterVO[0];
            Misc.Clear();
            Tutorials.Clear();
            Allegiance = FactionType.None;
            playerID = null;
            Livre = 0;
            Credibility = 0;
            Peril = 0;
            Exhaustion = 0;
            Perilous = false;
            SaveSlotID = -1;
        }

        public override string ToString()
        {
            return "GameModel:" +
                "\n Allegiance: " + Allegiance.ToString() +
                "\n Player: " + PlayerName +
                "\n Livre: " + Livre.ToString() +
                "\n Exhaustion: " + Exhaustion.ToString() +
                "\n Credibility: " + Credibility.ToString() +
                "\n Peril: " + Peril.ToString();
        }
    }
}
