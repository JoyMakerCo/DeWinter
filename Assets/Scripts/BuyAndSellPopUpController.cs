using UnityEngine;
using System.Collections;
using DeWinter;

public class BuyAndSellPopUpController : MonoBehaviour
{
    public string inventoryType;
    public string itemType;
    public int inventoryNumber;
    public ItemVO item;
    OutfitInventoryList personalOutfitInventoryList;
    OutfitInventoryList merchantOutfitInventoryList;
    OutfitInventory outfitInventory;
    AccessoryInventoryList personalAccessoryInventoryList;
    AccessoryInventoryList merchantAccessoryInventoryList;
    WardrobeImageController wardrobeImageController;
    private InventoryModel _inventoryModel;
    private GameModel _gameModel;

    void Start()
    {
		_inventoryModel = DeWinterApp.GetModel<InventoryModel>();
		_gameModel = DeWinterApp.GetModel<GameModel>();

        outfitInventory = GameObject.Find("OutfitInventory").GetComponent<OutfitInventory>();
        personalOutfitInventoryList = GameObject.Find("PlayerInventoryDisplay").transform.Find("OutfitDisplay").Find("OutfitListPanel").Find("GridWithElements").GetComponent<OutfitInventoryList>();
        merchantOutfitInventoryList = GameObject.Find("ShopDisplay").transform.Find("OutfitDisplay").Find("OutfitListPanel").Find("GridWithElements").GetComponent<OutfitInventoryList>();
        personalAccessoryInventoryList = GameObject.Find("PlayerInventoryDisplay").transform.Find("AccessoryDisplay").Find("AccessoryListPanel").Find("GridWithElements").GetComponent<AccessoryInventoryList>();
        merchantAccessoryInventoryList = GameObject.Find("ShopDisplay").transform.Find("AccessoryDisplay").Find("AccessoryListPanel").Find("GridWithElements").GetComponent<AccessoryInventoryList>();
        wardrobeImageController = GameObject.Find("ItemImageOutline").GetComponent<WardrobeImageController>();
    }

    public void BuyOrSell()
    {
        if (itemType == "Outfit")
        {
// TODO: Outfits and Gossip turned into ItemVO
            if (inventoryType == "personal") //Selling Things
            {
                //Add Money to our Account. Sold Items sell at 50% of their purchase price.
				_gameModel.Livre += OutfitInventory.outfitInventories[inventoryType][inventoryNumber].OutfitPrice(inventoryType);
                Debug.Log("Outfit Sold for " + OutfitInventory.outfitInventories[inventoryType][inventoryNumber].OutfitPrice(inventoryType));

                //Remove the Sold Item from the Personal Inventory
                OutfitInventory.outfitInventories[inventoryType].RemoveAt(inventoryNumber);

                //If that item was worn last at a party then reset the Last Party Outfit ID, so an item with its ID doesn't get a wrongful double Novelty hit
				if (inventoryNumber == _inventoryModel.lastPartyOutfitID)
                {
                	_inventoryModel.lastPartyOutfitID = -1;
                }
            }
            else if (inventoryType == "merchant") //Buying Things
            {
                //Remove Money from our account
				_gameModel.Livre -= OutfitInventory.outfitInventories[inventoryType][inventoryNumber].OutfitPrice(inventoryType);
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
	        	DeWinterApp.SendCommand<SellItemCmd, ItemVO>(item);

                //If that item was worn last at a party then reset the Last Party Outfit ID, so an item with its ID doesn't get a wrongful double Novelty hit
                if (inventoryNumber == _inventoryModel.lastPartyOutfitID)
                {
					_inventoryModel.lastPartyOutfitID = -1;
                }
            }
            else if (inventoryType == "merchant") //Buying Things
            {
				DeWinterApp.SendCommand<BuyItemCmd, ItemVO>(item);
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