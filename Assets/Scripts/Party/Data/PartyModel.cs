using System;
using System.Collections.Generic;
using Core;
using Newtonsoft.Json;
using Util;

namespace Ambition
{
	public class PartyModel : DocumentModel
	{
		public PartyModel(): base("PartyData") {}

		private PartyVO _party;
		public PartyVO Party
		{
			get { return _party; }
			set {
                CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
                _party = value;
                if (_party != null)
                {
                    _party.RSVP = RSVP.Accepted;
                    if (default(DateTime).Equals(_party.InvitationDate))
                        _party.InvitationDate = calendar.Today;
                    calendar.Schedule(_party, calendar.Today);
                }
            }
		}

		[JsonProperty("free_remark_counter")]
		public int FreeRemarkCounter;

        [JsonProperty("boredom_penalty")]
        public int BoredomPenalty;

        public int Turns
        {
            get { return _party != null ? _party.Turns : 0; }
            set {
                if (_party != null)
                {
                    _party.Turns = value;
                    AmbitionApp.SendMessage(PartyMessages.TURNS_LEFT, Turns - Turn);
                }
            }
        }

        public int TurnsLeft
        {
            get { return Turns - Turn; }
        }

		private int _turn;
		public int Turn
		{
			get { return _turn; }
			set {
				_turn = value;
                AmbitionApp.SendMessage(PartyMessages.TURN, _turn);
                AmbitionApp.SendMessage(PartyMessages.TURNS_LEFT, Turns - _turn);
			}
		}

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
				AmbitionApp.RegisterFactory<string, GuestActionVO>(factory);
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
