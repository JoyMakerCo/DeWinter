using Newtonsoft.Json;

namespace Ambition
{
    public class GuestActionVO
    {
        [JsonProperty("type")]
        public string Type;
        [JsonProperty("rounds")]
        public int Rounds=0;
        [JsonProperty("tags")]
        public string[] Tags;
        [JsonProperty("rewards")]
        public string[] Rewards;
        [JsonProperty("chance")]
        public int Chance;
        [JsonProperty("difficulty")]
        public int Difficuly;
    }
}
