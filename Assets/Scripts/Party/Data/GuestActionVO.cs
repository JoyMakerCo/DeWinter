using System;
using System.Collections.Generic;
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
        public Dictionary<string, int> Rewards;

        [JsonProperty("chance")]
        public int Chance;
        [JsonProperty("difficulty")]
        public int Difficuly;

        public GuestActionVO() {}
        public GuestActionVO(GuestActionVO action)
        {
            this.Type = action.Type;
            this.Rounds = action.Rounds;
            if (action.Tags != null)
            {
                this.Tags = new string[action.Tags.Length];
                Array.Copy(action.Tags, this.Tags, action.Tags.Length);
            }
            if (action.Rewards != null)
                this.Rewards = new Dictionary<string,int>(action.Rewards);
            this.Chance = action.Chance;
            this.Difficuly = action.Difficuly;
        }
    }
}
