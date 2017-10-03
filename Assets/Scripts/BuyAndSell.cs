using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using Ambition;

public class BuyAndSell : MonoBehaviour {
    public GameObject screenFader; // It's for the BuyAndSell pop-up
    public string inventoryType;

    public OutfitInventoryList personalInventoryList;
    public OutfitInventoryList merchantInventoryList;

    public void createBuyAndSellPopUp()
    {
		object[] objectStorage = new object[3];
        objectStorage[0] = inventoryType;
        objectStorage[1] = "Outfit";
		objectStorage[2] = inventoryType == ItemConsts.PERSONAL ? personalInventoryList.selectedInventoryOutfit : merchantInventoryList.selectedInventoryOutfit;

		if (inventoryType == ItemConsts.PERSONAL && personalInventoryList.selectedInventoryOutfit != null) 
        {
            screenFader.gameObject.SendMessage("CreateBuyOrSellModal", objectStorage);
        } else if (inventoryType == ItemConsts.MERCHANT && merchantInventoryList.selectedInventoryOutfit != null)
        {
        	InventoryModel model = AmbitionApp.GetModel<InventoryModel>();
   			if (model.Inventory.FindAll(i=>i.Type == ItemConsts.OUTFIT).Count < model.NumOutfits) // Will it fit in the Player's inventory?
            {
                if (AmbitionApp.GetModel<GameModel>().Livre >= merchantInventoryList.selectedInventoryOutfit.Price) // Can they afford it?
                {
                    screenFader.gameObject.SendMessage("CreateBuyOrSellModal", objectStorage);
                } else //If the Player can't afford it give them the Can't Afford Modal
                {
                    objectStorage[0] = merchantInventoryList.selectedInventoryOutfit.Name;
                    screenFader.gameObject.SendMessage("CreateCantAffordModal", objectStorage);
                }
            } else // If it won't fit then give them the "Can't Fit" Modal (hurr hurr hurr)
            {
            	Dictionary<string, string> subs = new Dictionary<string, string>(){
					{"$ITEM",merchantInventoryList.selectedInventoryOutfit.Name},
					{"$CAPACITY", model.NumOutfits.ToString()}};
				AmbitionApp.OpenMessageDialog(DialogConsts.CANT_BUY_DIALOG, subs);
            }           
        }
    }
}
