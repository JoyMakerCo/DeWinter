using UnityEngine;
using System.Collections;
using Ambition;

public class OutfitInventoryList : MonoBehaviour {

    public string inventoryType;
    public Outfit selectedInventoryOutfit;
    public GameObject outfitInventoryButtonPrefab;
    public WardrobeImageController imageController;

    // Use this for initialization
    void Start () {
        GenerateInventoryButtons();
        selectedInventoryOutfit = null; // So nothing is selected at the start
    }

    public void GenerateInventoryButtons()
    {
        foreach (Outfit o in OutfitInventory.outfitInventories[inventoryType])
        {
            GameObject button = GameObject.Instantiate(outfitInventoryButtonPrefab);
            OutfitInventoryButton buttonStats = button.GetComponent<OutfitInventoryButton>();
            buttonStats.outfit = o;
            buttonStats.inventoryType = inventoryType;
            button.transform.SetParent(this.transform, false);
            buttonStats.imageController = imageController;
            Debug.Log(inventoryType + " Outfit Button: " + o.Name() + " is made!");
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
        OutfitInventory.PartyOutfit = selectedInventoryOutfit;
    }
}
