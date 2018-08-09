using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using Ambition;

public class AccessoryInventoryButton : MonoBehaviour {
	public ItemVO accessory;

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
        DisplayOutfitStats();
		myOutline.effectColor = (accessoryInventoryList.selectedAccessory == accessory)
        	? Color.yellow
			: Color.clear;
    }

    public void DisplayOutfitStats()
    {
		if (accessory != null)
        {
            myDescriptionText.text = accessory.Name;
			myPriceText.text = accessory.PriceString;
        }
    }

    public void SetInventoryItem()
    {
        Debug.Log("Selected Inventory Item: " + accessory.Name);
		accessoryInventoryList.selectedAccessory = accessory;
//TODO: Display Image
    }
}
