using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

// TODO: Feed this to the inventory model
public static class OutfitInventory
{
    public static Dictionary<string, List<Outfit>> outfitInventories = new Dictionary<string, List<Outfit>>();
    public static List<Outfit> personalInventory = new List<Outfit>();
    public static List<Outfit> merchantInventory = new List<Outfit>();

    public static int personalInventoryMaxSize = 5; //The Max Size at Game Start
    public static int personalInventoryMaxSizeCieling = 9; //The Max Possible Size

    //Used for seeing if the same Outfit was used twice in a row
	public static Outfit LastPartyOutfit=null;
	public static Outfit PartyOutfit;

	//This is called ONCE at the Start Screen when you push the Start Button
	public static void StockInventory()
    {
        //Put the two Lists in the Dictionary
        if (!outfitInventories.ContainsKey("personal"))
        {
			//Put the Player's starting Outfits in the Personal List
			personalInventory.Add( new Outfit(100, 0, 0, "Frankish"));
	        outfitInventories.Add("personal", personalInventory);
       	}
		if (!outfitInventories.ContainsKey("merchant"))
		{
	        outfitInventories.Add("merchant", merchantInventory);
	    }
    }

    //Run whenever the Estate Streen Starts Up
	public static void RestockMerchantInventory()
    {
        merchantInventory.Clear();
        //Checking for Faction Benefit. If the Player is level 2+ with the Bourgeoisie Faction then the Merchant stocks additional wares
        if (GameData.factionList["Bourgeoisie"].ReputationLevel >= 3)
        {
            outfitInventories["merchant"].Add(new Outfit(GameData.currentStyle));
            outfitInventories["merchant"].Add(new Outfit());
            outfitInventories["merchant"].Add(new Outfit());
            outfitInventories["merchant"].Add(new Outfit());
        } else
        {
            outfitInventories["merchant"].Add(new Outfit(GameData.currentStyle));
            outfitInventories["merchant"].Add(new Outfit());
            outfitInventories["merchant"].Add(new Outfit());
        }
    }
}