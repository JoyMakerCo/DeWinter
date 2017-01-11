using UnityEngine;
using System.Collections;

public class OutfitInventoryList : MonoBehaviour {

    public string inventoryType;
    public int selectedInventoryOutfit;
    public GameObject outfitInventoryButtonPrefab;
    public WardrobeImageController imageController;

    // Use this for initialization
    void Start () {
        GenerateInventoryButtons();
        selectedInventoryOutfit = -1; // So nothing is selected at the start
    }

    public void GenerateInventoryButtons()
    {
        for (int i = 0; i < OutfitInventory.outfitInventories[inventoryType].Count; i++)
        {
            GameObject button = GameObject.Instantiate(outfitInventoryButtonPrefab);
            OutfitInventoryButton buttonStats = button.GetComponent<OutfitInventoryButton>();
            buttonStats.outfitID = i;
            buttonStats.inventoryType = inventoryType;
            button.transform.SetParent(this.transform, false);
            buttonStats.imageController = imageController;
            //Debug.Log(inventoryType + " Outfit Button: " + i + " is made!");
        }
    }

    public void ClearInventoryButtons()
    {
        foreach (Transform child in this.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void SelectPartyOutfit()
    {
        GameData.partyOutfitID = selectedInventoryOutfit;

    }
}
