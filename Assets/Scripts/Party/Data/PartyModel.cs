using System;
using System.Collections.Generic;
using Core;
using Newtonsoft.Json;
using Util;

namespace Ambition
{
	public class PartyModel : DocumentModel, IInitializable, IDisposable
	{
		public int DrinkAmount;

		public PartyModel(): base("PartyData") {}

		private PartyVO _party;
		public PartyVO Party
		{
			get { return _party; }
			set {
				_party = value;
				AmbitionApp.SendMessage<PartyVO>(_party);
			}
		}

		public bool IsAmbush=false;

		//TODO: Temp, until buffs are figured out
		public bool ItemEffect;
		public bool Repartee;

		[JsonProperty("free_remark_counter")]
		public int FreeRemarkCounter;

		private RemarkVO _remark;
		public RemarkVO Remark
		{
			get { return _remark; }
			set {
				_remark = value;
				AmbitionApp.SendMessage<RemarkVO>(_remark);
			}
		}

		private int _turnsLeft;
		public int TurnsLeft
		{
			get { return _turnsLeft; }
			set {
				_turnsLeft = value;
				AmbitionApp.SendMessage<int>(PartyConstants.TURNSLEFT, _turnsLeft);
			}
		}

		private int _turn;
		public int Turn
		{
			get { return _turn; }
			set {
				_turn = value;
				AmbitionApp.SendMessage<int>(PartyConstants.TURN, _turn);
			}
		}

		public GuestVO[] TargetedGuests;

		// Temporary Repo for buffs
		protected Dictionary<string, List<ModifierVO>> Modifiers = new Dictionary<string, List<ModifierVO>>();

		public void AddBuff(string id, string type, float multiplier, float bonus)
		{
			if (!Modifiers.ContainsKey(id))
			{
				Modifiers.Add(id, new List<ModifierVO>());
			}
			Modifiers[id].RemoveAll(m => m.Type == type);
			Modifiers[id].Add(new ModifierVO(id, type, multiplier, bonus));
		}

		public void RemoveBuff(string id, string type)
		{
			if (Modifiers.ContainsKey(id))
			{
				Modifiers[id].RemoveAll(t => t.Type == type);
			}
		}

		protected float ApplyBuffs(string id, float value)
		{
			List<ModifierVO> mods;
			float result = value;
			if (Modifiers.TryGetValue(id, out mods))
			{
				foreach (ModifierVO mod in mods)
				{
					result += value*(mod.Multiplier - 1.0f) + mod.Bonus;
				}
			}
			return result; 
		}

		[JsonProperty("maxPlayerDrinkAmount")]
		protected int _maxPlayerDrinkAmount;

		[JsonProperty("guest_difficulty")]
		public GuestDifficultyVO[] GuestDifficultyStats;

		public int MaxDrinkAmount
		{
			get
			{
				return (int)ApplyBuffs(GameConsts.DRINK, _maxPlayerDrinkAmount);
			}
		}

		[JsonProperty("topic_list")]
		public string[] Interests;

		[JsonProperty("turnTimer")]
		public float TurnTime = 5.0f;

		[JsonProperty("repartee_bonus")]
		public float ReparteeBonus;

		[JsonProperty("confidence_cost")]
		public int[] ConfidenceCost;

		public int RemarksBought=0;

		[JsonProperty("maxHandSize")]
		public int MaxHandSize = 5;

		[JsonProperty("ambushHandSize")]
		public int AmbushHandSize = 3;

		private int _intoxication;
		public int Intoxication
		{
			get { return _intoxication; }
			set {
				_intoxication = value > 0 ? value : 0;
				AmbitionApp.SendMessage<int>(GameConsts.INTOXICATION, _intoxication);
			}
		}
		public int MaxIntoxication = 3;

		[JsonProperty("parties")]
		public PartyVO[] Parties;

		public int MaxConfidence;
		public int StartConfidence;

		private int _confidence;
		public int Confidence
		{
			get { return _confidence; }
			set {
				_confidence = (value < 0 ? 0 : value > MaxConfidence ? MaxConfidence : value);
				AmbitionApp.SendMessage<int>(GameConsts.CONFIDENCE, _confidence);
			}
		}

		private int _drink;
		public int Drink
		{
			get { return _drink; }
			set {
				_drink = value;
				AmbitionApp.SendMessage<int>(GameConsts.DRINK, _drink);
			}
		}

		private List<RemarkVO> _remarks = new List<RemarkVO>();
		public List<RemarkVO> Remarks
		{
			get { return _remarks; }
			set
			{
				_remarks = value;
				AmbitionApp.SendMessage<List<RemarkVO>>(Remarks);
			}
		}

		private void HandleClearRemarks()
		{
			Remarks = new List<RemarkVO>();
		}

		private void HandleClearRemark(GuestVO guest)
		{
			Remarks.Remove(Remark);
			AmbitionApp.SendMessage<List<RemarkVO>>(Remarks);
		}

		public void Initialize()
		{
			AmbitionApp.Subscribe(PartyMessages.CLEAR_REMARKS, HandleClearRemarks);
			AmbitionApp.Subscribe<GuestVO>(PartyMessages.GUEST_SELECTED, HandleClearRemark);
		}

		public void Dispose()
		{
			AmbitionApp.Unsubscribe(PartyMessages.CLEAR_REMARKS, HandleClearRemarks);
			AmbitionApp.Unsubscribe<GuestVO>(PartyMessages.GUEST_SELECTED, HandleClearRemark);
		}
	}
}
