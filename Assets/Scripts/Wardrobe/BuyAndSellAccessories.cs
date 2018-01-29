using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Ambition;

public class BuyAndSellAccessories : MonoBehaviour {
    public GameObject screenFader; // It's for the BuyAndSell pop-up
    public string inventoryType;

    public AccessoryInventoryList personalAccessoryList;
    public AccessoryInventoryList merchantAccessoryList;
       
    public void createBuyAndSellPopUp()
    {
    	InventoryModel model = AmbitionApp.GetModel<InventoryModel>();
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
                if (AmbitionApp.GetModel<GameModel>().Livre >= merchantAccessoryList.selectedAccessory.Price) // Can they afford it?
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
				Dictionary<string, string> subs = new Dictionary<string, string>()
					{{"$ITEM",merchantAccessoryList.selectedAccessory.Name},
					{"$CAPACITY",AmbitionApp.GetModel<InventoryModel>().NumOutfits.ToString()}};
				AmbitionApp.OpenMessageDialog(DialogConsts.CANT_BUY_DIALOG, subs);
            }
        }
    }
}