using UnityEngine;
using System.Collections;
using DeWinter;

public class BuyAndSellPopUpController : MonoBehaviour
{
    public string inventoryType;
    public string itemType;
    public Outfit outfit;
    public ItemVO accessory;
    OutfitInventoryList personalOutfitInventoryList;
    OutfitInventoryList merchantOutfitInventoryList;
    AccessoryInventoryList personalAccessoryInventoryList;
    AccessoryInventoryList merchantAccessoryInventoryList;
    WardrobeImageController wardrobeImageController;
    private InventoryModel _inventoryModel;
    private GameModel _gameModel;

    void Start()
    {
		_inventoryModel = DeWinterApp.GetModel<InventoryModel>();
		_gameModel = DeWinterApp.GetModel<GameModel>();

        personalOutfitInventoryList = GameObject.Find("PlayerInventoryDisplay").transform.Find("OutfitDisplay").Find("OutfitListPanel").Find("GridWithElements").GetComponent<OutfitInventoryList>();
        merchantOutfitInventoryList = GameObject.Find("ShopDisplay").transform.Find("OutfitDisplay").Find("OutfitListPanel").Find("GridWithElements").GetComponent<OutfitInventoryList>();
        personalAccessoryInventoryList = GameObject.Find("PlayerInventoryDisplay").transform.Find("AccessoryDisplay").Find("AccessoryListPanel").Find("GridWithElements").GetComponent<AccessoryInventoryList>();
        merchantAccessoryInventoryList = GameObject.Find("ShopDisplay").transform.Find("AccessoryDisplay").Find("AccessoryListPanel").Find("GridWithElements").GetComponent<AccessoryInventoryList>();
        wardrobeImageController = GameObject.Find("ItemImageOutline").GetComponent<WardrobeImageController>();
    }


    // TODO: COmmandify
    public void BuyOrSell()
    {
        if (itemType == "Outfit")
        {
// TODO: Outfits and Gossip turned into ItemVO
            if (inventoryType == "personal") //Selling Things
            {
                //Add Money to our Account. Sold Items sell at 50% of their purchase price.
				_gameModel.Livre += outfit.OutfitPrice(inventoryType);
                Debug.Log("Outfit Sold for " + outfit.OutfitPrice(inventoryType));

                //Remove the Sold Item from the Personal Inventory
                OutfitInventory.outfitInventories[inventoryType].Remove(outfit);

                //If that item was worn last at a party then reset the Last Party Outfit ID, so an item with its ID doesn't get a wrongful double Novelty hit
				if (outfit == OutfitInventory.LastPartyOutfit)
                {
					OutfitInventory.LastPartyOutfit = null;
                }
            }
            else if (inventoryType == "merchant") //Buying Things
            {
                //Remove Money from our account
				_gameModel.Livre -= outfit.OutfitPrice(inventoryType);
                Debug.Log("Outfit Bought for" + outfit.OutfitPrice(inventoryType).ToString());

                //Add the Sold Item to the Personal Inventory
                OutfitInventory.outfitInventories["personal"].Add(outfit);

                //Remove the Sold Item from the Merchant Inventory
                OutfitInventory.outfitInventories[inventoryType].Remove(outfit);
            }
            personalOutfitInventoryList.selectedInventoryOutfit = null;
            merchantOutfitInventoryList.selectedInventoryOutfit = null;
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
	        	DeWinterApp.SendMessage<ItemVO>(InventoryConsts.SELL_ITEM, accessory);

                //If that item was worn last at a party then reset the Last Party Outfit ID, so an item with its ID doesn't get a wrongful double Novelty hit
                ItemVO lastAccssory; 
				if (_inventoryModel.LastEquipped.TryGetValue(ItemConsts.ACCESSORY, out lastAccssory) && lastAccssory == accessory)
                {
					_inventoryModel.LastEquipped.Remove(ItemConsts.ACCESSORY);
                }
            }
            else if (inventoryType == "merchant") //Buying Things
            {
				DeWinterApp.SendMessage<ItemVO>(InventoryConsts.BUY_ITEM, accessory);
            }
            personalAccessoryInventoryList.selectedAccessory = null;
            merchantAccessoryInventoryList.selectedAccessory = null;
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