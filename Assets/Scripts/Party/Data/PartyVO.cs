using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ambition;
using Newtonsoft.Json;

namespace Ambition
{
    [Serializable]
    public class PartyVO : ICalendarEvent
    {
        public string ID;           // ID also corresponds with the localization phrases
        public string Name;         // Assigned upon creation
        public string Description;  // Assigned upon creation
        public string Invitiation;  // Assigned upon creation

        public string Faction;
        public PartySize Importance; //Used for party size. 0 means
        public RSVP RSVP = RSVP.New; //0 means no RSVP yet, 1 means Attending and -1 means Decline
        public DateTime Date { get; set; }

        [JsonProperty("invitation_date")]
        public DateTime InvitationDate;

        [JsonProperty("map")]
        public string MapID;        // ID for parties with pregenerated maps

        public string Host;
        //public NotableVO Host;
        public EnemyVO[] Enemies;

        [JsonProperty("turns")]
        public int Turns;

        //Drinking and Intoxication
        public int MaxIntoxication = 4;
        public int maxPlayerDrinkAmount = 3;

        public List<CommodityVO> Requirements = new List<CommodityVO>();
        public List<CommodityVO> Rewards = new List<CommodityVO>();
    }
}
