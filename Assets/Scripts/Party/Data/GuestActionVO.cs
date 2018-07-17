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
        public int Rounds;
        public int StartRound;
        public int EndRound
        {
            get { return StartRound + Rounds;  }
        }
        [JsonProperty("tags")]
        public string[] Tags;
        [JsonProperty("values")]
        public Dictionary<string, int> Values;

        [JsonProperty("chance")]
        public int Chance;
        [JsonProperty("difficulty")]
        public int Difficulty;

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
            if (action.Values != null)
                this.Values = new Dictionary<string,int>(action.Values);
            this.Chance = action.Chance;
            this.Difficulty = action.Difficulty;
        }
    }
}
