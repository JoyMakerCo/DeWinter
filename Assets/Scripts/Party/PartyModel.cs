using System;
using System.Collections.Generic;
using Core;
using Newtonsoft.Json;

namespace DeWinter
{
	public class PartyModel : DocumentModel
	{
		public int DrinkAmount;

		public PartyModel(): base("PartyData") {}

		public Party Party;

		[JsonProperty("maxPlayerDrinkAmount")]
		public int MaxDrinkAmount;

		[JsonProperty("conversationIntroList")]
		public string[] ConversationIntros;

		[JsonProperty("hostRemarkIntroList")]
		public string[] HostIntros;

		[JsonProperty("dispositionList")]
		public Disposition[] Dispositions;

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
		public float TurnTimer;

		private int _intoxication;
		public int Intoxication
		{
			get { return _intoxication; }
			set {
				_intoxication = value;
				DeWinterApp.SendMessage<int>(GameConsts.INTOXICATION, _intoxication);
			}
		}

		private int _confidence;
		public int Confidence
		{
			get { return _confidence; }
			set {
				_confidence = value;
				DeWinterApp.SendMessage<int>(GameConsts.CONFIDENCE, _confidence);
			}
		}

		private int _drink;
		public int Drink
		{
			get { return _drink; }
			set {
				_drink = value;
				DeWinterApp.SendMessage<int>(GameConsts.DRINK, _drink);
			}
		}

		private List<Remark> _hand;
		public List<Remark> Hand
		{
			get { return _hand; }
			set {
				_hand = value;
				DeWinterApp.SendMessage<List<Remark>>(_hand);
			}
		}
	}
}