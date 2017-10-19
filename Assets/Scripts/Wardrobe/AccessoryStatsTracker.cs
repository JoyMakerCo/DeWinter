using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Ambition;

public class AccessoryStatsTracker : MonoBehaviour
{
    public string inventoryType;

    private Text accessoryNameText;
    private Text accessoryDescriptionText;
    private Text accessoryCostText;
    private Text accessorySaleButtonText;
    private Image accessorySaleButton;

    //Inventory List
    AccessoryInventoryList accessoryInventoryList;

    // Use this for initialization
    void Start()
    {
        //Text
        accessoryNameText = this.transform.Find("AccessoryNameText").GetComponent<Text>();
        accessoryDescriptionText = this.transform.Find("AccessoryDescriptionText").GetComponent<Text>();
        accessoryCostText = this.transform.Find("AccessoryPriceText").GetComponent<Text>();
        accessorySaleButtonText = this.transform.Find("SellAccessoryButton").Find("Text").GetComponent<Text>();

        //List
        accessoryInventoryList = this.transform.parent.Find("AccessoryListPanel").Find("GridWithElements").GetComponent<AccessoryInventoryList>();

        //Buttons (If Select Inventory Screen)
        accessorySaleButton = this.transform.Find("SellAccessoryButton").GetComponent<Image>();
    }

    void Update()
    {
    	InventoryModel m = AmbitionApp.GetModel<InventoryModel>();
        if (inventoryType == "personal")
        {
            if (accessoryInventoryList.selectedAccessory != null)
            {
                //Text            
                ItemVO displayAccessory = m.SelectedItem;
                accessoryNameText.text = displayAccessory.Name;
                accessoryDescriptionText.text = displayAccessory.Description;
                accessoryCostText.text = displayAccessory.PriceString; //Uses Personal Inventory Price
                accessorySaleButtonText.text = "Sell for " + displayAccessory.PriceString;
            }
            else
            {
                accessoryNameText.text = "";
                accessoryDescriptionText.text = "";
                accessoryCostText.text = "";
                accessorySaleButtonText.text = "Sell";
            }
        }
        else if (inventoryType == "merchant")
        {
            if (accessoryInventoryList.selectedAccessory != null)
            {
                //Text
                ItemVO displayAccessory = m.SelectedMarketItem;
                accessoryNameText.text = displayAccessory.Name;
                accessoryDescriptionText.text = displayAccessory.Description;
                accessoryCostText.text = displayAccessory.PriceString; //Uses Personal Inventory Price
                accessorySaleButtonText.text = "Buy for " + displayAccessory.PriceString;
            }
            else
            {
                accessoryNameText.text = "";
                accessoryDescriptionText.text = "";
                accessoryCostText.text = "";
                accessorySaleButtonText.text = "Buy";
            }
        }
        else if (inventoryType == "select") // Used in the Party Loadout Screen for Selecting Outfits
        {
            accessorySaleButton.color = Color.clear;
            accessorySaleButtonText.text = "";
            accessorySaleButtonText.color = Color.clear;
            if (accessoryInventoryList.selectedAccessory != null) //If an Outfit is Selected
            {
                ItemVO displayAccessory = m.SelectedItem;
                accessoryNameText.text = displayAccessory.Name;
                accessoryDescriptionText.text = displayAccessory.Description;
                accessoryCostText.text = displayAccessory.PriceString; //Uses Personal Inventory Prices
            }
            else
            {
                accessoryNameText.text = "Select an Accessory!";
                accessoryDescriptionText.text = "";
                accessoryCostText.text = "";
            }
        }
    }
}
