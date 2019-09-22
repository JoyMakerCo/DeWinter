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
        public string LocalizationKey;      // ID corresponds with the localization phrases
        public string Name { get; set; }
        public string Description;
        public string Invitation;  // Configured or Assigned upon creation
        public string[] Tags;
        public CharacterVO[] Guests;

        public FactionType Faction;
        public PartySize Size = PartySize.None;
        public RSVP RSVP = RSVP.New;

        [JsonIgnore]
        public DateTime Date { get; set; }

        [JsonProperty("complete")]
        public bool IsComplete { get; set; }

        public IncidentVO[] RequiredIncidents;
        public IncidentVO[] SupplementalIncidents;
        public IncidentVO IntroIncident;
        public IncidentVO ExitIncident;

        [JsonProperty("invitation_date")]
        public DateTime InvitationDate;

        public MapVO Map = null;
        public string Host;

        public bool Attending => RSVP == RSVP.Accepted || RSVP == RSVP.Required;

        //Drinking and Intoxication
        public int MaxIntoxication = 4;
        public int maxPlayerDrinkAmount = 3;

        public CommodityVO[] Requirements;
        public List<CommodityVO> Rewards = new List<CommodityVO>();

        public PartyVO() { }
        public PartyVO(PartyVO party)
        {
            LocalizationKey = party.LocalizationKey;
            Name = party.Name;
            Description = party.Description;
            Invitation = party.Invitation;
            Faction = party.Faction;
            RequiredIncidents = party.RequiredIncidents;
            Map = party.Map;
            RSVP = party.RSVP;
            Date = party.Date;
            IntroIncident = party.IntroIncident;
            ExitIncident = party.ExitIncident;
            InvitationDate = party.InvitationDate;
            Host = party.Host;
            MaxIntoxication = party.MaxIntoxication;
            maxPlayerDrinkAmount = party.maxPlayerDrinkAmount;
            Requirements = this.Requirements?.ToArray();
            Rewards = this.Rewards?.ToList();
        }
    }
}
