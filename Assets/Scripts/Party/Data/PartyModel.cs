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

		public PartyVO Party;

		public List<RewardVO> Rewards;

		//TODO: Temp, until buffs are figured out
		public bool ItemEffect;
		public bool Repartee;

		public RemarkVO Remark;

		private int _turnsLeft;
		public int TurnsLeft
		{
			get { return _turnsLeft; }
			set {
				_turnsLeft = value < 0 ? 0 : value > Party.Turns ? Party.Turns : value;
				AmbitionApp.SendMessage<int>(PartyConstants.TURNSLEFT, _turnsLeft);
			}
		}

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

		public int MaxDrinkAmount
		{
			get
			{
				return (int)ApplyBuffs(GameConsts.DRINK, _maxPlayerDrinkAmount);
			}
		}

		[JsonProperty("conversationIntroList")]
		public string[] ConversationIntros;

		[JsonProperty("hostRemarkIntroList")]
		public string[] HostIntros;

		[JsonProperty("topicList")]
		public string[] Interests;

		[JsonProperty("femaleTitleList")]
		public string[] FemaleTitles;

		[JsonProperty("maleTitleList")]
		public string[] MaleTitles;

		[JsonProperty("femaleFirstNameList")]
		public string[] FemaleNames;

		[JsonProperty("maleFirstNameList")]
		public string[] MaleNames;

		[JsonProperty("lastNameList")]
		public string[] LastNames;	

		[JsonProperty("turnTimer")]
		public float TurnTime = 5.0f;

		[JsonProperty("reparteeBonus")]
		public float ReparteeBonus = 1.25f;

		[JsonProperty("confidenceCost")]
		public int ConfidenceCost = 10;

		public string LastInterest;

		public int MaxHandSize = 5;

		private int _intoxication;
		public int Intoxication
		{
			get { return _intoxication; }
			set {
				_intoxication = value > 0 ? value : 0;
				AmbitionApp.SendMessage<int>(GameConsts.INTOXICATION, _intoxication);
			}
		}

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

		private List<RemarkVO> _hand;
		public List<RemarkVO> Hand
		{
			get { return _hand; }
			set {
				_hand = value;
				AmbitionApp.SendMessage<List<RemarkVO>>(_hand);
			}
		}

		public void AddRemark(RemarkVO remark)
		{
			if (_hand.Count < MaxHandSize)
			{
				_hand.Add(remark);
			}
			AmbitionApp.SendMessage<List<RemarkVO>>(_hand);
		}

		private void HandleClearRemarks()
		{
			_hand.Clear();
			AmbitionApp.SendMessage<List<RemarkVO>>(_hand);
		}

		public void Initialize()
		{
			AmbitionApp.Subscribe<PartyVO>(PartyMessages.RSVP, HandleRSVP);
			AmbitionApp.Subscribe<DateTime>(HandleDay);
			AmbitionApp.Subscribe(PartyMessages.REPARTEE_BONUS, HandleRepartee);
			AmbitionApp.Subscribe<RequestAdjustValueVO<int>>(HandleAdjustTurns);
			AmbitionApp.Subscribe(PartyMessages.CLEAR_REMARKS, HandleClearRemarks);
		}

		public void Dispose()
		{
			AmbitionApp.Unsubscribe<PartyVO>(PartyMessages.RSVP, HandleRSVP);
			AmbitionApp.Unsubscribe<DateTime>(HandleDay);
			AmbitionApp.Unsubscribe(PartyMessages.REPARTEE_BONUS, HandleRepartee);
			AmbitionApp.Unsubscribe<RequestAdjustValueVO<int>>(HandleAdjustTurns);
			AmbitionApp.Unsubscribe(PartyMessages.CLEAR_REMARKS, HandleClearRemarks);
		}

		private void HandleDay(DateTime date)
		{
			List<PartyVO> parties;
			if (AmbitionApp.GetModel<CalendarModel>().Parties.TryGetValue(date, out parties))
			{
				Party = parties.Find(p => p.RSVP == 1);
			}
			else
			{
				Party = null;
			}
		}

		private void HandleRSVP (PartyVO party)
	    {
	    	if (Party == party && party.RSVP < 1)
	    	{
	    		Party = null;
	    	}
	    	else if (party.Date == AmbitionApp.GetModel<CalendarModel>().Today && party.RSVP == 1)
	    	{
	    		Party = party;
			}
		}

		private void HandleAdjustTurns(RequestAdjustValueVO<int> turnsLeft)
		{
			if (turnsLeft.Type == PartyConstants.TURNSLEFT)
			{
				TurnsLeft = turnsLeft.Value;
			}
		}

		private void HandleRepartee()
		{
		}

		private void HandleBored()
		{
/*
			g.currentInterestTimer = Mathf.Clamp(g.currentInterestTimer - 1, 0, g.maxInterestTimer);
		            if (g.currentInterestTimer <= 0 && g.lockedInState == LockedInState.Interested && !g.isEnemy) //Guest must not be locked in and must not be an Enemy, Enemies don't get bored they merely wait
		            {
		                ChangeGuestOpinion(g, -10);
		                if (g.currentOpinion <= 0)
		                {
		                    g.lockedInState = LockedInState.PutOff;
		                }
		            }
*/
		}
	}
}