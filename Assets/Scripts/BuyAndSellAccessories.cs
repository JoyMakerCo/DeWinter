using UnityEngine;
using System.Collections;

public class BuyAndSellAccessories : MonoBehaviour {
    public GameObject screenFader; // It's for the BuyAndSell pop-up
    public string inventoryType;

    public AccessoryInventoryList personalAccessoryList;
    public AccessoryInventoryList merchantAccessoryList;
       
    public void createBuyAndSellPopUp()
    {
        if (inventoryType == "personal" && personalAccessoryList.selectedAccessory != -1)
        {
            object[] objectStorage = new object[3];
            objectStorage[0] = "personal";
            objectStorage[1] = "Accessory";
            objectStorage[2] = personalAccessoryList.selectedAccessory;
            screenFader.gameObject.SendMessage("CreateBuyOrSellModal", objectStorage);
        }
        else if (inventoryType == "merchant" && merchantAccessoryList.selectedAccessory != -1)
        {
            if (AccessoryInventory.personalInventory.Count < AccessoryInventory.personalInventoryMaxSize) // Will it fit in the Player's inventory?
            {
                if (GameData.moneyCount >= AccessoryInventory.merchantInventory[merchantAccessoryList.selectedAccessory].Price(inventoryType)) // Can they afford it?
                {
                    object[] objectStorage = new object[3];
                    objectStorage[0] = "merchant";
                    objectStorage[1] = "Accessory";
                    objectStorage[2] = merchantAccessoryList.selectedAccessory;
                    screenFader.gameObject.SendMessage("CreateBuyOrSellModal", objectStorage);
                }
                else //If the Player can't afford it give them the Can't Afford Modal
                {
                    object[] objectStorage = new object[1];
                    objectStorage[0] = AccessoryInventory.merchantInventory[merchantAccessoryList.selectedAccessory].Name();
                    screenFader.gameObject.SendMessage("CreateCantAffordModal", objectStorage);
                }
            }
            else // If it won't fit then give them the "Can't Fit" Modal (hurr hurr hurr)
            {
                object[] objectStorage = new object[2];
                objectStorage[0] = merchantAccessoryList.selectedAccessory;
                objectStorage[1] = AccessoryInventory.personalInventoryMaxSize;
                screenFader.gameObject.SendMessage("CreateCantFitModal", objectStorage);
            }
        }
    }
}
