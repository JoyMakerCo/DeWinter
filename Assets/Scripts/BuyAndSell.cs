using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using DeWinter;

public class BuyAndSell : MonoBehaviour {
    public GameObject screenFader; // It's for the BuyAndSell pop-up
    public string inventoryType;

    public OutfitInventoryList personalInventoryList;
    public OutfitInventoryList merchantInventoryList;

    public void createBuyAndSellPopUp()
    {
        if (inventoryType == "personal" && personalInventoryList.selectedInventoryOutfit != null) 
        {
            object[] objectStorage = new object[3];
            objectStorage[0] = "personal";
            objectStorage[1] = "Outfit";
            objectStorage[2] = personalInventoryList.selectedInventoryOutfit;
            screenFader.gameObject.SendMessage("CreateBuyOrSellModal", objectStorage);
        } else if (inventoryType == "merchant" && merchantInventoryList.selectedInventoryOutfit != null)
        {
            if (OutfitInventory.personalInventory.Count < OutfitInventory.personalInventoryMaxSize) // Will it fit in the Player's inventory?
            {
                if (DeWinterApp.GetModel<GameModel>().Livre >= merchantInventoryList.selectedInventoryOutfit.OutfitPrice(inventoryType)) // Can they afford it?
                {
                    object[] objectStorage = new object[3];
                    objectStorage[0] = "merchant";
                    objectStorage[1] = "Outfit";
                    objectStorage[2] = merchantInventoryList.selectedInventoryOutfit;
                    screenFader.gameObject.SendMessage("CreateBuyOrSellModal", objectStorage);
                } else //If the Player can't afford it give them the Can't Afford Modal
                {
                    object[] objectStorage = new object[1];
                    objectStorage[0] = merchantInventoryList.selectedInventoryOutfit.Name();
                    screenFader.gameObject.SendMessage("CreateCantAffordModal", objectStorage);
                }
            } else // If it won't fit then give them the "Can't Fit" Modal (hurr hurr hurr)
            {
                object[] objectStorage = new object[2];
                objectStorage[0] = merchantInventoryList.selectedInventoryOutfit;
                objectStorage[1] = OutfitInventory.personalInventoryMaxSize;
                screenFader.gameObject.SendMessage("CreateCantFitModal", objectStorage);
            }           
        }
    }
}
