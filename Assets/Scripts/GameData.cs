using System;
using System.Collections.Generic;
using DeWinter;

public static class GameData
{
	public static string playerVictoryStatus;
	public static bool fTEsOn;

	public static Party tonightsParty
	{
		get { return DeWinterApp.GetModel<PartyModel>().Party; }
		set { DeWinterApp.GetModel<PartyModel>().Party = value; }
	}

	public static ItemVO partyAccessory
	{
		get
		{
			ItemVO accessory;
			return DeWinterApp.GetModel<InventoryModel>().Equipped.TryGetValue(ItemConsts.ACCESSORY, out accessory)
				? accessory
				: null;
		}
		set
		{
			if (value == null)
				DeWinterApp.GetModel<InventoryModel>().Equipped.Remove(ItemConsts.ACCESSORY);
			DeWinterApp.GetModel<InventoryModel>().Equipped[ItemConsts.ACCESSORY] = value;
		}	
	}

	public static Dictionary<string, FactionVO> factionList
	{
		get { return DeWinterApp.GetModel<FactionModel>().Factions; }
	}

	public static int moneyCount
	{
		get { return DeWinterApp.GetModel<GameModel>().Livre; }
		set { DeWinterApp.GetModel<GameModel>().Livre = value; }
	}

	public static int reputationCount
	{
		get { return DeWinterApp.GetModel<GameModel>().Reputation; }
		set { DeWinterApp.GetModel<GameModel>().Reputation = value; }
	}

	public static int playerReputationLevel
	{
		get { return DeWinterApp.GetModel<GameModel>().ReputationLevel; }
	}

	public static string Allegiance
	{
		get { return DeWinterApp.GetModel<GameModel>().Allegiance; }
		set { DeWinterApp.GetModel<GameModel>().Allegiance = value; }
	}

	public static Disposition[] dispositionList
	{
		get { return DeWinterApp.GetModel<PartyModel>().Dispositions; }
	}

	public static List<Gossip> gossipInventory
	{
		get { return DeWinterApp.GetModel<InventoryModel>().GossipItems; }
	}

	public static List<PierreQuest> pierreQuestInventory
	{
		get { return DeWinterApp.GetModel<QuestModel>().Quests; }
	}

	public static string[] femaleTitleList
	{
		get { return DeWinterApp.GetModel<PartyModel>().FemaleTitles; }
	}

	public static string[] maleTitleList
	{
		get { return DeWinterApp.GetModel<PartyModel>().MaleTitles; }
	}

	public static string[] lastNameList
	{
		get { return DeWinterApp.GetModel<PartyModel>().LastNames; }
	}

	public static string[] femaleFirstNameList
	{
		get { return DeWinterApp.GetModel<PartyModel>().FemaleNames; }
	}

	public static string[] maleFirstNameList
	{
		get { return DeWinterApp.GetModel<PartyModel>().MaleNames; }
	}

	public static Dictionary<string, ServantVO[]> servantDictionary
	{
		get { return DeWinterApp.GetModel<ServantModel>().Servants; }
	}

	public static List<ItemVO> Accessories
	{
		get { return DeWinterApp.GetModel<InventoryModel>().Inventory.FindAll(i => Array.IndexOf(i.Tags, ItemConsts.ACCESSORY) >= 0); }
	}
}