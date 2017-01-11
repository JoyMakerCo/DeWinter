using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class AccessoryInventoryButton : MonoBehaviour {
    public int accessoryID;
    public string inventoryType;
    private Text myDescriptionText;
    private Text myPriceText;
    private Outline myOutline; // This is for highlighting buttons

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
        DisplayOutfitStats(inventoryType, accessoryID);
        if (accessoryInventoryList.selectedAccessory == accessoryID)
        {
            myOutline.effectColor = Color.yellow;
        }
        else
        {
            myOutline.effectColor = Color.clear;
        }
    }

    public void DisplayOutfitStats(string inv, int aID)
    {
        if (AccessoryInventory.accessoryInventories[inv].ElementAtOrDefault(aID) != null)
        {
            myDescriptionText.text = AccessoryInventory.accessoryInventories[inv][aID].Name();
            if (inv == "personal")
            {
                myPriceText.text = AccessoryInventory.accessoryInventories[inv][aID].Price(inventoryType).ToString("£" + "#,##0");
            }
            if (inv == "merchant")
            {
                myPriceText.text = AccessoryInventory.accessoryInventories[inv][aID].Price(inventoryType).ToString("£" + "#,##0");
            }
        }
    }

    public void SetInventoryItem()
    {
        Debug.Log("Selected Inventory Item: " + accessoryID.ToString());
        accessoryInventoryList.selectedAccessory = accessoryID;
        GameData.partyAccessoryID = accessoryID;
        //imageController.displayID = OutfitInventory.outfitInventories[inventoryType][outfitID].imageID;
    }
}
