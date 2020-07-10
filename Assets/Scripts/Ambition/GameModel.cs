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
    public class GameModel : DocumentModel, IInitializable, IDisposable, IConsoleEntity, IResettable
    {
        private const string CONFIG = "GameData";

        [JsonIgnore]
        public DateTime StartDate;

        [JsonIgnore]
        public DateTime EndDate;

        [JsonProperty("day")]
        public ushort Day;

        [JsonProperty("resting")]
        public bool IsResting;

        [JsonIgnore]
        public DateTime Date => StartDate.AddDays(Day);

        [JsonProperty("allegiance")]
        public FactionType Allegiance;

        public string PlayerName => AmbitionApp.Localize(playerID + ".name");

        [JsonProperty("playerID")]
        public string playerID = null;

        [JsonProperty("political_chance")]
        public int PoliticalChance = 20;

        [JsonProperty("party_intro_incident")]
        public string DefaultIntroIncident;

        [JsonProperty("missed_party_penalty")]
        public int MissedPartyPenalty;

        public Observable<int> Livre;
        public Observable<int> Exhaustion;
        public Observable<int> Credibility;
        public Observable<int> Peril;

        [JsonIgnore]
        public DateTime NextStyleSwitchDay;

        [JsonProperty("activity")]
        public ActivityType Activity;

        [JsonProperty("livre")]
        private int _livre
        {
            get => Livre.Value;
            set => Livre.Value = value;
        }

        [JsonProperty("exhaustion")]
        private int _exhaustion
        {
            get => Exhaustion.Value;
            set => Exhaustion.Value = value;
        }

        [JsonProperty("credibility")]
        private int _credibility
        {
            get => Credibility.Value;
            set => Credibility.Value = value;
        }

        [JsonProperty("peril")]
        private int _peril
        {
            get => Peril.Value;
            set => Peril.Value = value;
        }

        [JsonIgnore]
        private ReputationVO _reputation;

        [JsonProperty("reputation", Order = 10)]
        public int Reputation
        {
            get => _reputation.Reputation;
            set
            {
                if (value < 0) value = 0;
                int level = _reputation.Level = Array.FindIndex(_levels, r => r > value);
                _reputation.Reputation = value;
                _reputation.ReputationMax = _levels[level];
                if (level > 0)
                {
                    _reputation.Reputation -= _levels[level - 1];
                    _reputation.ReputationMax -= _levels[level - 1];
                }
                AmbitionApp.SendMessage(_reputation);
            }
        }

        public void Initialize()
        {
            Livre.Observe(HandleLivre);
            Exhaustion.Observe(HandleExhaustion);
            Peril.Observe(HandlePeril);
            Credibility.Observe(HandleCred);
        }

        public void Dispose()
        {
            Livre.Remove(HandleLivre);
            Exhaustion.Remove(HandleExhaustion);
            Peril.Remove(HandlePeril);
            Credibility.Remove(HandleCred);
        }

        public ushort Convert(DateTime date) => (ushort)(date.Subtract(StartDate).Days);

        public int Level => _reputation.Level;

        [JsonProperty("vip")]
        private readonly int[] _vip;
        public int PartyInviteImportance => _vip[Level];


        public GameModel() : base(CONFIG) { }

        private void HandleLivre(int livre) => AmbitionApp.SendMessage(GameConsts.LIVRE, livre);
        private void HandleCred(int cred) => AmbitionApp.SendMessage(GameConsts.CRED, cred);
        private void HandlePeril(int peril) => AmbitionApp.SendMessage(GameConsts.PERIL, peril);
        private void HandleExhaustion(int exhaustion)
        {
            if (exhaustion < 0) AmbitionApp.SendMessage(GameConsts.WELL_RESTED);
            else AmbitionApp.SendMessage(GameConsts.EXHAUSTION, exhaustion);
        }

        [JsonProperty("levels")]
        private int[] _levels;

        [JsonProperty("exhaustion_penalty")]
        private int[] _exhaustionPenalty;

        public bool IsWellRested => Exhaustion.Value < 0;

        [JsonProperty("well_rested_bonus")]
        public int WellRestedBonus { get; private set; }

        public int ExhaustionPenalty => Exhaustion.Value < 0
                ? WellRestedBonus
                : Exhaustion.Value < _exhaustionPenalty.Length
                ? _exhaustionPenalty[Exhaustion.Value]
                : _exhaustionPenalty[_exhaustionPenalty.Length - 1];

        [JsonIgnore]
        public ChapterVO[] Chapters = new ChapterVO[0];

        public void Reset()
        {
            playerID = null;
            Chapters = new ChapterVO[0];
            LoadFile(CONFIG);
        }

        public string[] Dump()
        {
            return new string[]
            {
                "GameModel:",
                "Allegiance: " + Allegiance.ToString(),
                "Player: " + PlayerName,
                "Livre: " + Livre.Value.ToString(),
                "Exhaustion: " + Exhaustion.Value.ToString(),
                "Credibility: " + Credibility.Value.ToString(),
                "Peril: " + Peril.Value.ToString(),
                "Reputation: " + Reputation.ToString(),
                "Level: " + Level.ToString(),
            };
        }


        public void Invoke( string[] args )
        {
            ConsoleModel.warn("GameModel has no invocation.");
        }
    }
}
