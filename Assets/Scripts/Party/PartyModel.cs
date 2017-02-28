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
		public Disposition[] ConversationIntros;

		[JsonProperty("hostRemarkIntroList")]
		public Disposition[] HostIntros;

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
	}
}