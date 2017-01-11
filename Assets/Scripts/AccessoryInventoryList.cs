using UnityEngine;
using System.Collections;

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
        for (int i = 0; i < AccessoryInventory.accessoryInventories[inventoryType].Count; i++)
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

    public void ClearInventoryButtons()
    {
        foreach (Transform child in this.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void SelectPartyAccessory()
    {
        GameData.partyAccessoryID = selectedAccessory;
        Debug.Log("Party Accessory Set!");
    }
}
