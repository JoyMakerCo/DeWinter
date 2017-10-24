﻿using System;
using System.Collections.Generic;
using Ambition;

public static class GameData
{
	public static string playerVictoryStatus;
	public static bool fTEsOn;

	public static ItemVO partyAccessory
	{
		get
		{
			ItemVO accessory;
			return AmbitionApp.GetModel<InventoryModel>().Equipped.TryGetValue(ItemConsts.ACCESSORY, out accessory)
				? accessory
				: null;
		}
		set
		{
			if (value == null)
				AmbitionApp.GetModel<InventoryModel>().Equipped.Remove(ItemConsts.ACCESSORY);
			AmbitionApp.GetModel<InventoryModel>().Equipped[ItemConsts.ACCESSORY] = value;
		}	
	}

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
