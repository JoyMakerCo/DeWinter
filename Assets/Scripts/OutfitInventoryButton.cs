using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using Ambition;

public class OutfitInventoryButton : MonoBehaviour {
    public OutfitVO outfit;
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
		DisplayOutfitStats(outfit, inventoryType);
        if (outfitInventoryList.selectedInventoryOutfit == outfit)
        {
            myOutline.effectColor = Color.yellow;
        }
        else
        {
            myOutline.effectColor = Color.clear;
        }       
    }

    public void DisplayOutfitStats(OutfitVO o, string inventoryType)
    {
		myDescriptionText.text = o.Name;
		o.CalculatePrice(inventoryType == "Personal");
		myPriceText.text = o.Price.ToString("£" + "#,##0");
    }

    public void SetInventoryItem()
    {
        outfitInventoryList.selectedInventoryOutfit = outfit;
        imageController.displayID = outfit.Style;
    }
}
