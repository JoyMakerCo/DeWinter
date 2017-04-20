using System;
using System.Collections.Generic;
using Core;
using Newtonsoft.Json;
using Util;

namespace DeWinter
{
	public class PartyModel : DocumentModel, IInitializable, IDisposable
	{
		public int DrinkAmount;

		public PartyModel(): base("PartyData") {}

		public Party Party;

		private DateTime _today;

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

		public void Initialize()
		{
			DeWinterApp.Subscribe<Party>(PartyMessages.RSVP, HandleRSVP);
			DeWinterApp.Subscribe<DateTime>(HandleDay);
		}

		public void Dispose()
		{
			DeWinterApp.Unsubscribe<Party>(PartyMessages.RSVP, HandleRSVP);
			DeWinterApp.Unsubscribe<DateTime>(HandleDay);
		}

		private void HandleDay(DateTime date)
		{
			_today = date;
			List<Party> parties;
			if (DeWinterApp.GetModel<CalendarModel>().Parties.TryGetValue(date, out parties))
			{
				Party = parties.Find(p => p.RSVP == 1);
			}
			else
			{
				Party = null;
			}
		}

		private void HandleRSVP (Party party)
	    {
	    	if (Party == party && party.RSVP < 1)
	    	{
	    		Party = null;
	    	}
	    	else if (party.Date == _today && party.RSVP == 1)
	    	{
	    		Party = party;
			}
		}
	}
}