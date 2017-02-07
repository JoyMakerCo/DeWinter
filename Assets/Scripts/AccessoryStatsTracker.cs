using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AccessoryStatsTracker : MonoBehaviour {

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
        if (inventoryType == "personal")
        {
            if (accessoryInventoryList.selectedAccessory != -1)
            {
                //Text
                AccessoryVO displayAccessory = AccessoryInventory.accessoryInventories[inventoryType][accessoryInventoryList.selectedAccessory];
                accessoryNameText.text = displayAccessory.Name();
                accessoryDescriptionText.text = displayAccessory.Description();
                accessoryCostText.text = displayAccessory.Price(inventoryType).ToString("£" + "#,##0"); //Uses Personal Inventory Price
                accessorySaleButtonText.text = "Sell for " + displayAccessory.Price(inventoryType).ToString("£" + "#,##0");
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
            if (accessoryInventoryList.selectedAccessory != -1)
            {
                //Text
                AccessoryVO displayAccessory = AccessoryInventory.accessoryInventories[inventoryType][accessoryInventoryList.selectedAccessory];
                accessoryNameText.text = displayAccessory.Name();
                accessoryDescriptionText.text = displayAccessory.Description();
                accessoryCostText.text = displayAccessory.Price(inventoryType).ToString("£" + "#,##0"); //Uses Personal Inventory Price
                accessorySaleButtonText.text = "Buy for " + displayAccessory.Price(inventoryType).ToString("£" + "#,##0");
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
            if (accessoryInventoryList.selectedAccessory != -1) //If an Outfit is Selected
            {
                AccessoryVO displayAccessory = AccessoryInventory.accessoryInventories["personal"][accessoryInventoryList.selectedAccessory];
                accessoryNameText.text = displayAccessory.Name();
                accessoryDescriptionText.text = displayAccessory.Description();
                accessoryCostText.text = displayAccessory.Price(inventoryType).ToString("£" + "#,##0"); //Uses Personal Inventory Prices
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
