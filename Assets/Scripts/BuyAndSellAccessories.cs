﻿using UnityEngine;
using System.Collections;
using DeWinter;

public class BuyAndSellAccessories : MonoBehaviour {
    public GameObject screenFader; // It's for the BuyAndSell pop-up
    public string inventoryType;

    public AccessoryInventoryList personalAccessoryList;
    public AccessoryInventoryList merchantAccessoryList;

    void Start()
    {
        personalAccessoryList = GameObject.Find("PlayerInventoryDisplay").transform.Find("AccessoryDisplay").Find("AccessoryListPanel").Find("GridWithElements").GetComponent<AccessoryInventoryList>();
        merchantAccessoryList = GameObject.Find("ShopDisplay").transform.Find("AccessoryDisplay").Find("AccessoryListPanel").Find("GridWithElements").GetComponent<AccessoryInventoryList>();
    }

    public void createBuyAndSellPopUp()
    {
    	InventoryModel model = DeWinterApp.GetModel<InventoryModel>();
        if (inventoryType == "personal" && personalAccessoryList.selectedAccessory != null)
        {
            object[] objectStorage = new object[3];
            objectStorage[0] = "personal";
            objectStorage[1] = "Accessory";
            objectStorage[2] = personalAccessoryList.selectedAccessory;
            screenFader.gameObject.SendMessage("CreateBuyOrSellModal", objectStorage);
        }
        else if (inventoryType == "merchant" && merchantAccessoryList.selectedAccessory != null)
        {
            if (model.Inventory.Count < model.MaxSlots) // Will it fit in the Player's inventory?
            {
                if (DeWinterApp.GetModel<GameModel>().Livre >= merchantAccessoryList.selectedAccessory.Price) // Can they afford it?
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
                    objectStorage[0] = merchantAccessoryList.selectedAccessory.Name;
                    screenFader.gameObject.SendMessage("CreateCantAffordModal", objectStorage);
                }
            }
            else // If it won't fit then give them the "Can't Fit" Modal (hurr hurr hurr)
            {
                object[] objectStorage = new object[2];
                objectStorage[0] = merchantAccessoryList.selectedAccessory;
                objectStorage[1] = model.MaxSlots;
                screenFader.gameObject.SendMessage("CreateCantFitModal", objectStorage);
            }
        }
    }
}
