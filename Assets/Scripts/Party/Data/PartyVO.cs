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
    public class PartyVO : CalendarEvent
    {
        [JsonProperty("faction")]
        public FactionType Faction;

        [JsonProperty("size")]
        public PartySize Size = PartySize.None;

        [JsonProperty("required")]
        public string[] RequiredIncidents;

        [JsonProperty("incidents")]
        public string[] SupplementalIncidents;

        [JsonProperty("intro")]
        public string IntroIncident;

        [JsonProperty("exit")]
        public string ExitIncident;

        [JsonProperty("map")]
        public string Map = null;

        [JsonProperty("host")]
        public string Host = null;

        [JsonProperty("phrases")]
        public int[] phrases = new int[0];

        [JsonIgnore]
        public CommodityVO[] Requirements;

        public PartyVO() { }
        public PartyVO(string partyID) { ID = partyID; }
        public PartyVO(PartyVO party)
        {
            ID = party.ID;
            Faction = party.Faction;
            RequiredIncidents = party.RequiredIncidents;
            Map = party.Map;
            RSVP = party.RSVP;
            Day = party.Day;
            IntroIncident = party.IntroIncident;
            ExitIncident = party.ExitIncident;
            Created = party.Created;
            Host = party.Host;
            Requirements = this.Requirements?.ToArray();
        }

        public override string ToString()
        {
            return string.Format( "PartyVO: {0} [{1}]", ID, Faction );
        }
    }
}
