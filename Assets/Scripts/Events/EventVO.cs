using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DeWinter
{
	public class EventVO
	{
		// Display Name of the event. Not a unique key.
		[JsonProperty("name")]
	    public string Name;

	    // ID of the Event. Unique Key.
		[JsonProperty("id")]
		public string EventID;

		// Display text of the event.
		[JsonProperty("description")]
		public string Description;

		// Type of the event. Currently includes Party, Night, or Intro
		[JsonProperty("type")]
		public string Type;

		// Chance for random selection. 
		[JsonProperty("weight")]
	    public int Weight=0;

	    // Rewards for completion of this event.
		[JsonProperty("rewards")]
		public RewardVO[] Rewards=null;

		// IDs of subsequent events dependant on this event.
		[JsonProperty("options")]
		public Dictionary<string, string> Options;

		// Determines whether the next stage is determined or random.
		[JsonProperty("random")]
		public bool IsRandom=false;
	}
}

//TODO: Rewards for Player Rep, Faction Rep, Livre, Enemies, Gossip
//	        GameData.reputationCount += eventStages[currentStage].stageRepChange;
//	        GameData.moneyCount += eventStages[currentStage].stageMoneyChange;
//	        if (eventStages[currentStage].stageEnemyAdd != null)
//	        {
//	            EnemyInventory.AddEnemy(eventStages[currentStage].stageEnemyAdd);
//	        }