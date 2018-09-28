using System;
using System.Collections.Generic;
using Ambition;

public static class GameData
{
	public static string playerVictoryStatus;
	public static bool fTEsOn;

	public static int moneyCount
	{
		get { return AmbitionApp.GetModel<GameModel>().Livre; }
		set { AmbitionApp.GetModel<GameModel>().Livre = value; }
	}

	public static int reputationCount
	{
		get { return AmbitionApp.GetModel<GameModel>().Reputation; }
		set { AmbitionApp.GetModel<GameModel>().Reputation = value; }
	}

	public static string Allegiance
	{
		get { return AmbitionApp.GetModel<GameModel>().Allegiance; }
		set { AmbitionApp.GetModel<GameModel>().Allegiance = value; }
	}

	public static List<Gossip> gossipInventory
	{
		get { return AmbitionApp.GetModel<InventoryModel>().GossipItems; }
	}

	public static List<PierreQuest> pierreQuestInventory
	{
		get { return AmbitionApp.GetModel<QuestModel>().Quests; }
	}
}
