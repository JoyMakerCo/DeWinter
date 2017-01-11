using UnityEngine;
using System.Collections;

public class BuyAndSellPopUpController : MonoBehaviour {
    public string inventoryType;
    public string itemType;
    public int inventoryNumber;
    OutfitInventoryList personalOutfitInventoryList;
    OutfitInventoryList merchantOutfitInventoryList;
    OutfitInventory outfitInventory;
    AccessoryInventoryList personalAccessoryInventoryList;
    AccessoryInventoryList merchantAccessoryInventoryList;
    AccessoryInventory accessoryInventory;
    WardrobeImageController wardrobeImageController;

    void Start()
    {
        outfitInventory = GameObject.Find("OutfitInventory").GetComponent<OutfitInventory>();
        personalOutfitInventoryList = GameObject.Find("PlayerInventoryDisplay").transform.Find("OutfitDisplay").Find("OutfitListPanel").Find("GridWithElements").GetComponent<OutfitInventoryList>();
        merchantOutfitInventoryList = GameObject.Find("ShopDisplay").transform.Find("OutfitDisplay").Find("OutfitListPanel").Find("GridWithElements").GetComponent<OutfitInventoryList>();
        accessoryInventory = GameObject.Find("AccessoryInventory").GetComponent<AccessoryInventory>();
        personalAccessoryInventoryList = GameObject.Find("PlayerInventoryDisplay").transform.Find("AccessoryDisplay").Find("AccessoryListPanel").Find("GridWithElements").GetComponent<AccessoryInventoryList>();
        merchantAccessoryInventoryList = GameObject.Find("ShopDisplay").transform.Find("AccessoryDisplay").Find("AccessoryListPanel").Find("GridWithElements").GetComponent<AccessoryInventoryList>();
        wardrobeImageController = GameObject.Find("ItemImageOutline").GetComponent<WardrobeImageController>();
    }

    public void BuyOrSell()
    {
        if (itemType == "Outfit")
        {
            if (inventoryType == "personal") //Selling Things
            {
                //Add Money to our Account. Sold Items sell at 50% of their purchase price.
                GameData.moneyCount += OutfitInventory.outfitInventories[inventoryType][inventoryNumber].OutfitPrice(inventoryType);
                Debug.Log("Outfit Sold for " + OutfitInventory.outfitInventories[inventoryType][inventoryNumber].OutfitPrice(inventoryType));

                //Remove the Sold Item from the Personal Inventory
                OutfitInventory.outfitInventories[inventoryType].RemoveAt(inventoryNumber);

                //If that item was worn last at a party then reset the Last Party Outfit ID, so an item with its ID doesn't get a wrongful double Novelty hit
                if (inventoryNumber == GameData.lastPartyOutfitID)
                {
                    GameData.lastPartyOutfitID = -1;
                }
            }
            else if (inventoryType == "merchant") //Buying Things
            {
                //Remove Money from our account
                GameData.moneyCount -= OutfitInventory.outfitInventories[inventoryType][inventoryNumber].OutfitPrice(inventoryType);
                Debug.Log("Outfit Bought for" + OutfitInventory.outfitInventories[inventoryType][inventoryNumber].OutfitPrice(inventoryType));

                //Add the Sold Item to the Personal Inventory
                OutfitInventory.outfitInventories["personal"].Add(OutfitInventory.outfitInventories[inventoryType][inventoryNumber]);

                //Remove the Sold Item from the Merchant Inventory
                OutfitInventory.outfitInventories[inventoryType].RemoveAt(inventoryNumber);
            }
            personalOutfitInventoryList.selectedInventoryOutfit = -1;
            merchantOutfitInventoryList.selectedInventoryOutfit = -1;
            //Clear and Create New Buttons
            personalOutfitInventoryList.ClearInventoryButtons();
            merchantOutfitInventoryList.ClearInventoryButtons();
            personalOutfitInventoryList.GenerateInventoryButtons();
            merchantOutfitInventoryList.GenerateInventoryButtons();
        }
        else if (itemType == "Accessory")
        {
            if (inventoryType == "personal") //Selling Things
            {
                //Add Money to our Account. Sold Items sell at 50% of their purchase price.
                GameData.moneyCount += AccessoryInventory.accessoryInventories[inventoryType][inventoryNumber].Price(inventoryType);
                Debug.Log("Accessory Sold for" + AccessoryInventory.accessoryInventories[inventoryType][inventoryNumber].Price(inventoryType));

                //Remove the Sold Item from the Personal Inventory
                AccessoryInventory.accessoryInventories[inventoryType].RemoveAt(inventoryNumber);

                //If that item was worn last at a party then reset the Last Party Outfit ID, so an item with its ID doesn't get a wrongful double Novelty hit
                if (inventoryNumber == GameData.lastPartyOutfitID)
                {
                    GameData.lastPartyOutfitID = -1;
                }
            }
            else if (inventoryType == "merchant") //Buying Things
            {
                //Remove Money from our account
                GameData.moneyCount -= AccessoryInventory.accessoryInventories[inventoryType][inventoryNumber].Price(inventoryType);
                Debug.Log("Accessory Bought for" + AccessoryInventory.accessoryInventories[inventoryType][inventoryNumber].Price(inventoryType));

                //Add the Sold Item to the Personal Inventory
                AccessoryInventory.accessoryInventories["personal"].Add(AccessoryInventory.accessoryInventories[inventoryType][inventoryNumber]);

                //Remove the Sold Item from the Merchant Inventory
                AccessoryInventory.accessoryInventories[inventoryType].RemoveAt(inventoryNumber);
            }
            personalAccessoryInventoryList.selectedAccessory = -1;
            merchantAccessoryInventoryList.selectedAccessory = -1;
            //Clear and Create New Buttons
            personalAccessoryInventoryList.ClearInventoryButtons();
            merchantAccessoryInventoryList.ClearInventoryButtons();
            personalAccessoryInventoryList.GenerateInventoryButtons();
            merchantAccessoryInventoryList.GenerateInventoryButtons();

        }
        //Reset the Displays
        wardrobeImageController.displayID = 0;
    }
}

