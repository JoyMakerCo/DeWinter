using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ambition;
using Newtonsoft.Json;

public class PartyVO
{
	[JsonProperty("id")]
    public string ID;

    public string Name;
	
	[JsonProperty("description")]
    public string Description;

	[JsonProperty("faction")]
    public string Faction;

	[JsonProperty("importance")]
    public int Importance;

    public bool invited=false;
    public int RSVP = 0; //0 means no RSVP yet, 1 means Attending and -1 means Decline

	[JsonProperty("date")]
    public DateTime Date;

	[JsonProperty("invitation_date")]
    public DateTime InvitationDate;

	[JsonProperty("map")]
    public string MapID;		// ID for parties with pregenerated maps

    public NotableVO Host;
    public EnemyVO[] Enemies;
	
	[JsonProperty("turns")]
    public int Turns;

	[JsonProperty("guests")]
	public string[] Guests;

	[JsonProperty("invitation")]
	public string Invitation;

    //Drinking and Intoxication
    public int MaxIntoxication = 4;
    public int maxPlayerDrinkAmount = 3;
    public int drinkStrength = 20;

    public List<RemarkVO> playerHand = new List<RemarkVO>();
    public string lastTone;

    public bool blackOutEnding = false; //Did they Party end normally or via Blacking Out?
    public string blackOutEffect; // This is used for the After Party Report
    public int blackOutEffectAmount; //This is also used for the After Party Report

	public List<CommodityVO> Rewards = new List<CommodityVO>();

    public PartyVO() {}

    void SetExclusiveFaction(string excludeFaction)
    {
		FactionModel fmod = AmbitionApp.GetModel<FactionModel>();
		string[] factions = Enumerable.ToArray(fmod.Factions.Keys);
		Faction = factions[new Random().Next(1, factions.Length)];
		if (Faction == excludeFaction) Faction = factions[0];
    }
}
