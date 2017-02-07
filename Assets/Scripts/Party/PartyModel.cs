using System;
using Core;

namespace DeWinter
{
	public class PartyModel : DocumentModel
	{
		public PartyModel(): base("PartyData") {}

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