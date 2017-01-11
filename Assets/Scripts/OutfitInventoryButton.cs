using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class OutfitInventoryButton : MonoBehaviour {
    public int outfitID;
    public string inventoryType;
    private Text myDescriptionText;
    private Text myPriceText;
    private Outline myOutline; // This is for highlighting buttons

    public WardrobeImageController imageController;
    OutfitInventoryList outfitInventoryList;
 
    void Start()
    {
        myDescriptionText = this.transform.Find("DescriptionText").GetComponent<Text>();
        myPriceText = this.transform.Find("PriceText").GetComponent<Text>();
        myOutline = this.GetComponent<Outline>();
        outfitInventoryList = this.transform.parent.GetComponent<OutfitInventoryList>();
    }

    void Update()
    {
        DisplayOutfitStats(inventoryType, outfitID);
        if (outfitInventoryList.selectedInventoryOutfit == outfitID)
        {
            myOutline.effectColor = Color.yellow;
        }
        else
        {
            myOutline.effectColor = Color.clear;
        }       
    }

    public void DisplayOutfitStats(string inv, int oID)
    {
        if(OutfitInventory.outfitInventories[inv].ElementAtOrDefault(oID) != null)
        {
            myDescriptionText.text = OutfitInventory.outfitInventories[inv][oID].Name();
            if (inv == "personal")
            {
                myPriceText.text = OutfitInventory.outfitInventories[inv][oID].OutfitPrice(inv).ToString("£" + "#,##0");
            } 
            if (inv == "merchant")
            {
                myPriceText.text = OutfitInventory.outfitInventories[inv][oID].OutfitPrice(inv).ToString("£" + "#,##0");
            }    
        } 
    }

    public void SetInventoryItem()
    {
        Debug.Log("Selected Inventory Item: " + outfitID.ToString());
        outfitInventoryList.selectedInventoryOutfit = outfitID;
        imageController.displayID = OutfitInventory.outfitInventories[inventoryType][outfitID].imageID;
        GameData.partyOutfitID = outfitID;
    }
}
