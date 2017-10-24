using UnityEngine;
using System.Collections;
using Ambition;

public class BuyAndSellPopUpController : MonoBehaviour
{
    public string inventoryType;
    public string itemType;
    public OutfitVO outfit;
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
		_inventoryModel = AmbitionApp.GetModel<InventoryModel>();
		_gameModel = AmbitionApp.GetModel<GameModel>();

        personalOutfitInventoryList = GameObject.Find("PlayerInventoryDisplay").transform.Find("OutfitDisplay").Find("OutfitListPanel").Find("GridWithElements").GetComponent<OutfitInventoryList>();
        merchantOutfitInventoryList = GameObject.Find("ShopDisplay").transform.Find("OutfitDisplay").Find("OutfitListPanel").Find("GridWithElements").GetComponent<OutfitInventoryList>();
        personalAccessoryInventoryList = GameObject.Find("PlayerInventoryDisplay").transform.Find("AccessoryDisplay").Find("AccessoryListPanel").Find("GridWithElements").GetComponent<AccessoryInventoryList>();
        merchantAccessoryInventoryList = GameObject.Find("ShopDisplay").transform.Find("AccessoryDisplay").Find("AccessoryListPanel").Find("GridWithElements").GetComponent<AccessoryInventoryList>();
        wardrobeImageController = GameObject.Find("ItemImageOutline").GetComponent<WardrobeImageController>();
    }


    // TODO: COmmandify
    public void BuyOrSell()
    {
    	InventoryModel model = AmbitionApp.GetModel<InventoryModel>();
        if (itemType == "Outfit")
        {
// TODO: Outfits and Gossip turned into ItemVO
            if (inventoryType == ItemConsts.PERSONAL) //Selling Things
            {
            	GameModel gm = AmbitionApp.GetModel<GameModel>();
                //Add Money to our Account. Sold Items sell at 50% of their purchase price.
                outfit.CalculatePrice(true);
				_gameModel.Livre += outfit.Price;
				Debug.Log("Outfit Sold for " + outfit.Price.ToString());

                //Remove the Sold Item from the Personal Inventory
				model.Inventory.Remove(outfit);

                //If that item was worn last at a party then reset the Last Party Outfit ID, so an item with its ID doesn't get a wrongful double Novelty hit
				if (outfit == gm.LastOutfit)
                {
					gm.LastOutfit = null;
                }
            }
            else
            {
                //Remove Money from our account
                outfit.CalculatePrice(false);
				_gameModel.Livre -= outfit.Price;
				Debug.Log("Outfit Bought for" + outfit.Price.ToString());

                //Add the Sold Item to the Personal Inventory
                model.Inventory.Add(outfit);
				model.Market.Remove(outfit);
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
            if (inventoryType == ItemConsts.PERSONAL) //Selling Things
            {
	        	AmbitionApp.SendMessage<ItemVO>(InventoryMessages.SELL_ITEM, accessory);

                //If that item was worn last at a party then reset the Last Party Outfit ID, so an item with its ID doesn't get a wrongful double Novelty hit
                ItemVO lastAccssory; 
				if (_inventoryModel.LastEquipped.TryGetValue(ItemConsts.ACCESSORY, out lastAccssory) && lastAccssory == accessory)
                {
					_inventoryModel.LastEquipped.Remove(ItemConsts.ACCESSORY);
                }
            }
            else
            {
				AmbitionApp.SendMessage<ItemVO>(InventoryMessages.BUY_ITEM, accessory);
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
        wardrobeImageController.displayID = null;
    }
}