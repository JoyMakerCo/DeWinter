using System;
using System.Collections.Generic;
using Core;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using UnityEngine;

namespace Ambition
{
    [Saveable]
    public class GameModel : DocumentModel, IDisposable, IConsoleEntity
    {
        [JsonProperty("allegiance")]
        public FactionType Allegiance;

        [JsonIgnore]
        public int GameID = 0;

        public string PlayerName => AmbitionApp.Localize(PlayerPhrase + ".name");
        public string PlayerPhrase = null;

        [JsonProperty("chapter")]
        public int Chapter = 0;

        [JsonProperty("political_chance")]
        public int PoliticalChance = 20;

        [JsonProperty("party_intro_incident")]
        public string DefaultIntroIncident;

        public Observable<int> Livre;
        public Observable<int> Exhaustion;
        public Observable<int> Credibility;
        public Observable<int> Peril;

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

        protected override void OnLoadComplete()
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

        [JsonProperty("incident_history")]
        public Dictionary<string,int> IncidentHistory;


        [JsonProperty("vip")]
        private readonly int[] _vip;
        public int PartyInviteImportance => _vip[Level];

        public int Level => _reputation.Level;

        [JsonProperty("num_paris_dailies")]
        public int NumParisDailies = 5;

        public GameModel() : base("GameData")
        {
            IncidentHistory = new Dictionary<string, int>();
        }

        private void HandleLivre(int livre) => AmbitionApp.SendMessage(GameConsts.LIVRE, livre);
        private void HandleCred(int cred) => AmbitionApp.SendMessage(GameConsts.CRED, cred);
        private void HandlePeril(int peril) => AmbitionApp.SendMessage(GameConsts.PERIL, peril);
        private void HandleExhaustion(int exhaustion)
        {
            if (exhaustion < 0) AmbitionApp.SendMessage(GameConsts.WELL_RESTED);
            else AmbitionApp.SendMessage(GameConsts.EXHAUSTION,
            exhaustion);
        }

        public void MarkCompleteIncident( IncidentVO ivo )
        {
            Debug.LogFormat("GameModel.MarkCompleteIncident {0}", ivo.Name);
            if (!IncidentHistory.ContainsKey( ivo.Name ))
            {
                IncidentHistory[ivo.Name] = 0;
            }

            IncidentHistory[ivo.Name]++;
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

        public string[] Dump()
        {
            var lines = new List<string>()
            {
                "GameModel:",
                "Allegiance: " + Allegiance.ToString(),
                "Player: " + PlayerName,
                "Chapter: " + Chapter,
                "Livre: " + Livre.Value.ToString(),
                "Exhaustion: " + Exhaustion.Value.ToString(),
                "Credibility: " + Credibility.Value.ToString(),
                "Peril: " + Peril.Value.ToString(),
                "Reputation: " + Reputation.ToString(),
                "Level: " + Level.ToString(),
            };
                
            lines.Add( "Play Counts: ");

            foreach (var kv in IncidentHistory)
            {
                lines.Add( string.Format("  {0}: {1}", kv.Key, kv.Value ) );
            }

            return lines.ToArray();
        }


        public void Invoke( string[] args )
        {
            ConsoleModel.warn("GameModel has no invocation.");
        }
    }
}
