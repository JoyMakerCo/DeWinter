using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class AccessoryInventory : MonoBehaviour {

    static AccessoryInventory instance = null;

    public static Dictionary<string, List<AccessoryVO>> accessoryInventories = new Dictionary<string, List<AccessoryVO>>();
    public static List<AccessoryVO> personalInventory = new List<AccessoryVO>();
    public static List<AccessoryVO> merchantInventory = new List<AccessoryVO>();
    public GameObject accessoryInventoryButtonPrefab;

    public static int personalInventoryMaxSize = 7; //The Max Size at Game Start
    public static int personalInventoryMaxSizeCieling = 11; //The Max Possible Size

    // Use this for initialization
    void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            print("Duplicate Game Data container self-destructing!");
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
        accessoryInventories.Add("personal", personalInventory);
        accessoryInventories.Add("merchant", merchantInventory);
        //Start by stocking the Merchant Inventory
        RestockMerchantInventory();
    }

    //Run whenever the Estate Streen Starts Up
    void RestockMerchantInventory()
    {
        Debug.Log("Stocking Merchant Accessory Inventory!");
        merchantInventory.Clear();
        merchantInventory.Add(new AccessoryVO(GameData.currentStyle));
        merchantInventory.Add(new AccessoryVO());
    }

}
