using System;
using System.Collections.Generic;
using Ambition;

public static class GameData
{
	public static string playerVictoryStatus;
	public static bool fTEsOn;

	public static PartyVO tonightsParty
	{
		get { return AmbitionApp.GetModel<PartyModel>().Party; }
		set { AmbitionApp.GetModel<PartyModel>().Party = value; }
	}

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

	public static Dictionary<string, FactionVO> factionList
	{
		get { return AmbitionApp.GetModel<FactionModel>().Factions; }
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

	public static int playerReputationLevel
	{
		get { return AmbitionApp.GetModel<GameModel>().Level; }
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

	public static string[] femaleTitleList
	{
		get { return AmbitionApp.GetModel<PartyModel>().FemaleTitles; }
	}

	public static string[] maleTitleList
	{
		get { return AmbitionApp.GetModel<PartyModel>().MaleTitles; }
	}

	public static string[] lastNameList
	{
		get { return AmbitionApp.GetModel<PartyModel>().LastNames; }
	}

	public static string[] femaleFirstNameList
	{
		get { return AmbitionApp.GetModel<PartyModel>().FemaleNames; }
	}

	public static string[] maleFirstNameList
	{
		get { return AmbitionApp.GetModel<PartyModel>().MaleNames; }
	}

	public static Dictionary<string, ServantVO[]> servantDictionary
	{
		get { return AmbitionApp.GetModel<ServantModel>().Servants; }
	}

	public static List<ItemVO> Accessories
	{
		get { return AmbitionApp.GetModel<InventoryModel>().Inventory.FindAll(i => Array.IndexOf(i.Tags, ItemConsts.ACCESSORY) >= 0); }
	}
}