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

		public GuestVO[] Guests;

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

		[JsonProperty("reparteeBonus")]
		private float _reparteeBonus = 1.25f;

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

		private List<RemarkVO> _hand;
		public List<RemarkVO> Hand
		{
			get { return _hand; }
			set {
				_hand = value;
				DeWinterApp.SendMessage<List<RemarkVO>>(_hand);
			}
		}

		public void Initialize()
		{
			DeWinterApp.Subscribe<Party>(PartyMessages.RSVP, HandleRSVP);
			DeWinterApp.Subscribe<DateTime>(HandleDay);
			DeWinterApp.Subscribe(PartyMessages.REPARTEE_BONUS, HandleRepartee);
			DeWinterApp.Subscribe<RoomVO>(HandleRoom);
		}

		public void Dispose()
		{
			DeWinterApp.Unsubscribe<Party>(PartyMessages.RSVP, HandleRSVP);
			DeWinterApp.Unsubscribe<DateTime>(HandleDay);
			DeWinterApp.Unsubscribe(PartyMessages.REPARTEE_BONUS, HandleRepartee);
			DeWinterApp.Unsubscribe<RoomVO>(HandleRoom);
		}

		private void HandleRoom(RoomVO room)
		{
			Guests = room.Guests;
			DeWinterApp.SendMessage<GuestVO[]>(Guests);
		}

		private void HandleDay(DateTime date)
		{
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
	    	else if (party.Date == DeWinterApp.GetModel<CalendarModel>().Today && party.RSVP == 1)
	    	{
	    		Party = party;
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