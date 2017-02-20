using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DeWinter;

public class AccessoryInventoryList : MonoBehaviour {

    public string inventoryType;
    public int selectedAccessory;
    public GameObject accessoryInventoryButtonPrefab;
    //public WardrobeImageController imageController;

    // Use this for initialization
    void Start()
    {
        GenerateInventoryButtons();
        selectedAccessory = -1; // So nothing is selected at the start
    }

    public void GenerateInventoryButtons()
    {
		List<ItemVO> accessories = DeWinterApp.GetModel<InventoryModel>().Inventory[inventoryType];
        for (int i = 0; i < accessories.Count; i++)
        {
        	if (accessories[i].Type == "Accessory")
        	{
	            GameObject button = GameObject.Instantiate(accessoryInventoryButtonPrefab);
	            AccessoryInventoryButton buttonStats = button.GetComponent<AccessoryInventoryButton>();
	            buttonStats.accessoryID = i;
	            buttonStats.inventoryType = inventoryType;
	            button.transform.SetParent(this.transform, false);
	            //buttonStats.imageController = imageController;
	            Debug.Log("Generating Accessory Inventory Button, Type: " + inventoryType + ", Number: " + i);
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
        DeWinterApp.GetModel<InventoryModel>().partyAccessoryID = selectedAccessory;
        Debug.Log("Party Accessory Set!");
    }
}
