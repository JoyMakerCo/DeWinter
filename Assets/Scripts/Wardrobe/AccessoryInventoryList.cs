using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Ambition;

public class AccessoryInventoryList : MonoBehaviour
{
   	public string inventoryType;
    public ItemVO selectedAccessory;
    public GameObject accessoryInventoryButtonPrefab;
    //public WardrobeImageController imageController;

    // Use this for initialization
    void Start()
    {
        GenerateInventoryButtons();
        selectedAccessory = null; // So nothing is selected at the start
    }

    public void GenerateInventoryButtons()
    {
		List<ItemVO> accessories;
		if (inventoryType == "personal")
			accessories = AmbitionApp.GetModel<InventoryModel>().Inventory.FindAll(i => i.Tags.Contains(ItemConsts.ACCESSORY));
		else
			accessories = AmbitionApp.GetModel<InventoryModel>().Market.FindAll(i => i.Tags.Contains(ItemConsts.ACCESSORY));

		if (accessories != null)
		{
			foreach (ItemVO accessory in accessories)
	        {
	            GameObject button = GameObject.Instantiate(accessoryInventoryButtonPrefab);
	            AccessoryInventoryButton buttonStats = button.GetComponent<AccessoryInventoryButton>();
	            buttonStats.accessory = accessory;
	            buttonStats.inventoryType = inventoryType;
	            button.transform.SetParent(this.transform, false);
	            //buttonStats.imageController = imageController;
	            Debug.Log("Generating Accessory Inventory Button, Type: " + inventoryType + ", Item: " + accessory.Name);
	        }
        }
    }

    public void ClearInventoryButtons()
    {
        foreach (Transform child in this.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void SelectPartyAccessory()
    {
        GameData.partyAccessory = selectedAccessory;
        Debug.Log("Party Accessory Set!");
    }
}
