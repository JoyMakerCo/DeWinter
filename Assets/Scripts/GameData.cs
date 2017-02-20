using System;
using System.Collections.Generic;
using DeWinter;

public static class GameData
{
	public static Party tonightsParty
	{
		get { return DeWinterApp.GetModel<PartyModel>().Party; }
	}

	public static int partyAccessoryID
	{
		get { return DeWinterApp.GetModel<InventoryModel>().partyAccessoryID; }
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

	public static Disposition[] dispositionList
	{
		get { return DeWinterApp.GetModel<PartyModel>().Dispositions; }
	}
}