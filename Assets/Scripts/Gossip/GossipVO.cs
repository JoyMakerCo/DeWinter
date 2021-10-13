using System;
using Newtonsoft.Json;

namespace Ambition
{
    [Serializable]
    public class GossipVO
    {
        [JsonProperty("faction")]
        public FactionType Faction;

        [JsonProperty("created")]
        public int Created;

        [JsonProperty("tier")]
        public int Tier;

        [JsonProperty("power")]
        public bool IsPower;
    }
}
