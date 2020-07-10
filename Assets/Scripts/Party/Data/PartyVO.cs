using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ambition;
using Newtonsoft.Json;

using Core;

namespace Ambition
{
    [Serializable]
    public class PartyVO
    {
        public string LocalizationKey;      // ID corresponds with the localization phrases
        public string ID;                   // Unique Key
        public string Name;
        public string Description;
        public string Invitation;  // Configured or Assigned upon creation

        public FactionType Faction;
        public PartySize Size = PartySize.None;
        public RSVP RSVP = RSVP.New;

        public DateTime Date
        {
            get => new DateTime(_date);
            set => _date = value.Ticks;
        }

        [JsonProperty("complete")]
        public bool IsComplete { get; set; }

        public string[] RequiredIncidents;
        public string[] SupplementalIncidents;
        public string IntroIncident;
        public string ExitIncident;

        public DateTime InvitationDate
        {
            get => new DateTime(_invitationDate);
            set => _invitationDate = value.Ticks;
        }

        public string Map = null;
        public string Host;

        public bool Attending => RSVP == RSVP.Accepted || RSVP == RSVP.Required;

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
            _invitationDate = party._invitationDate;
            Host = party.Host;
            Requirements = this.Requirements?.ToArray();
            Rewards = this.Rewards?.ToList();
        }

        [JsonProperty("invitation_date")]
        private long _invitationDate;

        [JsonProperty("date")]
        private long _date;

        public override string ToString()
        {
            return string.Format( "PartyVO: {0} [{1}]", Name, Faction );
        }
    }
}
