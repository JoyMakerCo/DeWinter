using System;
using System.Collections.Generic;
using Core;
using Newtonsoft.Json;
using Util;

namespace Ambition
{
//    [Saveable]
    public class PartyModel : DocumentModel, IConsoleEntity
    {
        public PartyModel() : base("PartyData") { }
        public PartyVO Party;

        public int Turns;
        public int Turn = 0;
        public int TurnsLeft => Turns - Turn;
        public int IncidentIndex = 0;
        public IncidentVO[] Incidents;
        public IncidentVO Incident;
        public IncidentVO RequiredIncident => ((Party?.RequiredIncidents?.Length ?? 0) > IncidentIndex)
            ? Party.RequiredIncidents[IncidentIndex]
            : null;

        public ItemVO LastOutfit;

        [JsonProperty("charmed_remark_bonus")]
        public int CharmedRemarkBonus;

        [JsonProperty("free_remark_counter")]
		public int FreeRemarkCounter;

        [JsonProperty("boredom_penalty")]
        public int BoredomPenalty;

        [JsonProperty("boredom_remark_penalty")]
        public int BoredomRemarkPenalty;

        [JsonProperty("offended_remark_penalty")]
        public int OffendedRemarkPenalty;

        public bool Complete => Party?.IsComplete ?? true;

		[JsonProperty("guest_difficulty")]
		public GuestDifficultyVO[] GuestDifficultyStats;

		[JsonProperty("maxPlayerDrinkAmount")]
		public int MaxDrinkAmount;

		[JsonProperty("topic_list")]
		public string[] Interests;

		[JsonProperty("turnTimer")]
		public float RoundTime = 5.0f;

		[JsonProperty("repartee_bonus")]
		public float ReparteeBonus;

        [JsonProperty("deck_size")]
        public int DeckSize;

        [JsonProperty("hand_size")]
		public int HandSize = 5;

		[JsonProperty("ambushHandSize")]
		public int AmbushHandSize = 3;

		private GuestActionVO[] _guestActions;
		[JsonProperty("guest_actions")]
		public GuestActionVO[] GuestActions
		{
			get { return _guestActions; }
			set
			{
				_guestActions = value;
				GuestActionFactory factory = new GuestActionFactory();
				foreach(GuestActionVO action in value)
					factory.Actions[action.Type] = action;
				AmbitionApp.RegisterFactory(factory);
			}
		}

		[JsonProperty("guest_action_chance")]
		public int[] GuestActionChance;
		[JsonProperty("charmed_guest_action_chance")]
		public int[] CharmedGuestActionChance;

        [JsonProperty("flee_party_penalty")]
        public int FleePartyPenalty = 25;
        [JsonProperty("flee_faction_penalty")]
        public int FleeFactionPenalty = 50;

        [JsonProperty("remark_result")]
        public Dictionary<string, RemarkResult> RemarkResults;

        public Queue<RemarkVO> Deck;
        public List<RemarkVO> Discard;
        public int MaxDeckSize => Deck.Count + Discard.Count;

        private int _intoxication;
		public int Intoxication
		{
			get { return _intoxication; }
			set {
				_intoxication = value > 0 ? value : 0;
				AmbitionApp.SendMessage<int>(GameConsts.INTOXICATION, _intoxication);
			}
		}
		public int MaxIntoxication = 100;

		private int _drink;
		public int Drink
		{
			get { return _drink; }
			set {
				_drink = value;
				AmbitionApp.SendMessage<int>(GameConsts.DRINK, _drink);
			}
		}


/* 
        public int Turns;
        public int Turn = 0;
        public int TurnsLeft => Turns - Turn;
        public int IncidentIndex = 0;
        public IncidentVO[] Incidents;
        public IncidentVO Incident;
        public IncidentVO RequiredIncident => ((Party?.RequiredIncidents?.Length ?? 0) > IncidentIndex)
            ? Party.RequiredIncidents[IncidentIndex]
            : null;

        public ItemVO LastOutfit;
*/
		public string[] Dump()
		{

			var lines = new List<string>();
			lines.Add( "PartyModel:");
			lines.Add(string.Format("Turn: {0}/{1}", Turn,Turns ));
			lines.Add("Incidents: ");
			for (int i = 0; i < Incidents.Length; i++)
			{
				string prefix = (i == IncidentIndex) ? "> " : "  ";
				lines.Add( prefix + Incidents[i].ToString() );
			}


			lines.Add( "Party: " + Party.ToString() );
			var outfit = "null";
			if (LastOutfit != null)
			{
				outfit = LastOutfit.ToString();
			}
			lines.Add( "Last Outfit: " + outfit );
			
			return lines.ToArray();
		}

		public void Invoke( string[] args )
		{
		ConsoleModel.warn("PartyModel has no invocation.");
		}  
	}

    public struct RemarkResult
    {
        [JsonProperty("opinion_min")]
        public int OpinionMin;

        [JsonProperty("opinion_max")]
        public int OpinionMax;

        [JsonProperty("remarks")]
        public int Remarks;

        [JsonProperty("reset")]
        public bool ResetInvolvement;
    }

}
