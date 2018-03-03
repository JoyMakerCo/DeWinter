using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Ambition;
using Newtonsoft.Json;

public class EventStage
{
	[JsonProperty("description")]
    public string Description;

	[JsonProperty("options")]
    public EventOption[] Options;

	[JsonProperty("rewards")]
    public CommodityVO[] Rewards;

    public EventStage(string desc, CommodityVO[] rewards, params EventOption[] options)
    {
        Description = desc;
        Rewards = rewards ?? new CommodityVO[0];
        Options = options;
    }

    public EventStage(string desc, CommodityVO reward, params EventOption[] options)
    {
        Description = desc;
        Rewards = new CommodityVO[]{ reward };
        Options = options;
    }

    public EventStage(string desc, params EventOption[] options)
    {
        Description = desc;
		Rewards = new CommodityVO[0];
        Options = options;
    }
}