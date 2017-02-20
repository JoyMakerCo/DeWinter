using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using DeWinter;

public class AccessoryInventoryButton : MonoBehaviour {
    public int accessoryID;
    public string inventoryType;
    private Text myDescriptionText;
    private Text myPriceText;
    private Outline myOutline; // This is for highlighting buttons
    private ItemVO _accessory;

    //public WardrobeImageController imageController;
    AccessoryInventoryList accessoryInventoryList;

    void Start()
    {
        myDescriptionText = this.transform.Find("DescriptionText").GetComponent<Text>();
        myPriceText = this.transform.Find("PriceText").GetComponent<Text>();
        myOutline = this.GetComponent<Outline>();
        accessoryInventoryList = this.transform.parent.GetComponent<AccessoryInventoryList>();
    }

    void Update()
    {
        DisplayOutfitStats();
		myOutline.effectColor = (accessoryInventoryList.selectedAccessory == accessoryID)
        	? Color.yellow
			: Color.clear;
    }

    public void DisplayOutfitStats()
    {
		if (_accessory != null)
        {
            myDescriptionText.text = _accessory.Name;
			myPriceText.text = (inventoryType == "merchant")
				? _accessory.PriceString
				: _accessory.SellPriceString;
        }
    }

    public void SetInventoryItem()
    {
        Debug.Log("Selected Inventory Item: " + accessoryID.ToString());
		DeWinterApp.GetModel<InventoryModel>().partyAccessoryID = accessoryID;
		accessoryInventoryList.selectedAccessory = accessoryID;
        //imageController.displayID = OutfitInventory.outfitInventories[inventoryType][outfitID].imageID;
    }
}
