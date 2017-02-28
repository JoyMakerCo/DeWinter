using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class OutfitInventory : MonoBehaviour
{
    static OutfitInventory instance = null;

    public static Dictionary<string, List<Outfit>> outfitInventories = new Dictionary<string, List<Outfit>>();
    public static List<Outfit> personalInventory = new List<Outfit>();
    public static List<Outfit> merchantInventory = new List<Outfit>();
    public GameObject outfitInventoryButtonPrefab;

    public static int personalInventoryMaxSize = 5; //The Max Size at Game Start
    public static int personalInventoryMaxSizeCieling = 9; //The Max Possible Size

    //Used for seeing if the same Outfit was used twice in a row
	public static Outfit LastPartyOutfit=null;
	public static Outfit PartyOutfit;

    // Use this for initialization
    void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            print("Duplicate Outfit Inventory container self-destructing!");
        }
        else
        {
            instance = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }

        if (SceneManager.GetActiveScene().name == "Game_Estate")
        {
            RestockMerchantInventory();
        }
    }
	
	//This is called ONCE at the Start Screen when you push the Start Button
	public void StockInventory()
    {
        //Put the two Lists in the Dictionary
        outfitInventories.Add("personal", personalInventory);
        outfitInventories.Add("merchant", merchantInventory);

        //Put the Player's starting Outfits in the Personal List
        outfitInventories["personal"].Add( new Outfit(100, 0, 0, "Frankish"));
    }

    //Run whenever the Estate Streen Starts Up
    void RestockMerchantInventory()
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
