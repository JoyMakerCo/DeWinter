using System;
using System.Linq;
using System.Collections.Generic;
using Ambition;
using Newtonsoft.Json;

[Serializable]
public class QuestVO
{
    [JsonProperty("faction")]
    public FactionType Faction;

    [JsonProperty("created")]
    public int Created;

    [JsonProperty("due")]
    public int Due;

    [JsonProperty("reward")]
    public CommodityVO Reward;
}