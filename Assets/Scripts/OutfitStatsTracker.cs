using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OutfitStatsTracker : MonoBehaviour {

    public string inventoryType;

    private Text outfitNameText;
    private Text outfitCostText;
    private Text outfitSaleButtonText;
    private Image outfitSaleButton;

    private Slider modestySlider;
    private Slider luxurySlider;
    private Slider noveltySlider;

    private Image modestySliderHandle;
    private Image luxurySliderHandle;
    private Image noveltySliderHandle;

    //Inventory List
    OutfitInventoryList outfitInventoryList;

    // Use this for initialization
    void Start () {
        //Text
        outfitNameText = this.transform.Find("OutfitNameText").GetComponent<Text>();
        outfitCostText = this.transform.Find("OutfitPriceText").GetComponent<Text>();
        outfitSaleButtonText = this.transform.Find("SellOutfitButton").Find("Text").GetComponent<Text>();

        //Button Images
        outfitSaleButton = this.transform.Find("SellOutfitButton").GetComponent<Image>();

        //Sliders
        modestySlider = this.transform.Find("ModestyText").Find("Slider").GetComponent<Slider>();
        luxurySlider = this.transform.Find("LuxuryText").Find("Slider").GetComponent<Slider>();
        noveltySlider = this.transform.Find("NoveltyText").Find("Slider").GetComponent<Slider>();
        
        //Slider Handles
        modestySliderHandle = modestySlider.transform.Find("Handle Slide Area").Find("Handle").GetComponent<Image>();
        luxurySliderHandle = luxurySlider.transform.Find("Handle Slide Area").Find("Handle").GetComponent<Image>();
        noveltySliderHandle = noveltySlider.transform.Find("Handle Slide Area").Find("Handle").GetComponent<Image>();
        
        //List
        outfitInventoryList = this.transform.parent.Find("OutfitListPanel").Find("GridWithElements").GetComponent<OutfitInventoryList>();
    }
	
	// Update is called once per frame
	void Update () {
        if (inventoryType == "personal")
        {
            if (outfitInventoryList.selectedInventoryOutfit != -1)
            {
                //Text
                Outfit displayOutfit = OutfitInventory.outfitInventories[inventoryType][outfitInventoryList.selectedInventoryOutfit];
                outfitNameText.text = displayOutfit.Name();
                outfitCostText.text = displayOutfit.OutfitPrice(inventoryType).ToString("£" + "#,##0");
                outfitSaleButtonText.text = "Sell for " + displayOutfit.OutfitPrice(inventoryType).ToString("£" + "#,##0");
                //Sliders
                modestySlider.value = displayOutfit.modesty;
                luxurySlider.value = displayOutfit.luxury;
                noveltySlider.value = displayOutfit.novelty;
                //Make Sure Slider Handles are Visible
                modestySliderHandle.color = Color.white;
                luxurySliderHandle.color = Color.white;
                noveltySliderHandle.color = Color.white;
            } else
            {
                outfitNameText.text = "";
                outfitCostText.text = "";
                outfitSaleButtonText.text = "Sell";
                //Make Sure Slider Handles are Invisible
                modestySliderHandle.color = Color.clear;
                luxurySliderHandle.color = Color.clear;
                noveltySliderHandle.color = Color.clear;
            }
        }  else if (inventoryType == "merchant")
        {
            if (outfitInventoryList.selectedInventoryOutfit != -1)
            {
                //Text
                Outfit displayOutfit = OutfitInventory.outfitInventories[inventoryType][outfitInventoryList.selectedInventoryOutfit];
                outfitNameText.text = displayOutfit.Name();
                outfitCostText.text = displayOutfit.OutfitPrice(inventoryType).ToString("£" + "#,##0");
                outfitSaleButtonText.text = "Buy for " + displayOutfit.OutfitPrice(inventoryType).ToString("£" + "#,##0");
                //Sliders
                modestySlider.value = displayOutfit.modesty;
                luxurySlider.value = displayOutfit.luxury;
                noveltySlider.value = displayOutfit.novelty;
                //Make Sure Slider Handles are Visible
                modestySliderHandle.color = Color.white;
                luxurySliderHandle.color = Color.white;
                noveltySliderHandle.color = Color.white;
            } else
            {
                outfitNameText.text = "";
                outfitCostText.text = "";
                outfitSaleButtonText.text = "Buy";
                //Make Sure Slider Handles are Invisible
                modestySliderHandle.color = Color.clear;
                luxurySliderHandle.color = Color.clear;
                noveltySliderHandle.color = Color.clear;
            }
        } else if (inventoryType == "select") // Used in the Party Loadout Screen for Selecting Outfits
        {
            outfitSaleButton.color = Color.clear;
            outfitSaleButtonText.text = "";
            outfitSaleButtonText.color = Color.clear;
            if (outfitInventoryList.selectedInventoryOutfit != -1) //If an Outfit is Selected
            {
                //Text
                Outfit displayOutfit = OutfitInventory.outfitInventories["personal"][outfitInventoryList.selectedInventoryOutfit];
                outfitNameText.text = displayOutfit.Name();
                outfitCostText.text = displayOutfit.OutfitPrice("personal").ToString("£" + "#,##0"); //Uses Personal Inventory Prices
                //Sliders
                modestySlider.value = displayOutfit.modesty;
                luxurySlider.value = displayOutfit.luxury;
                noveltySlider.value = displayOutfit.novelty;
                //Make Sure Slider Handles are Visible
                modestySliderHandle.color = Color.white;
                luxurySliderHandle.color = Color.white;
                noveltySliderHandle.color = Color.white;
            }
            else
            {
                outfitNameText.text = "Select an Outfit!";
                outfitCostText.text = "";
                //Make Sure Slider Handles are Invisible
                modestySliderHandle.color = Color.clear;
                luxurySliderHandle.color = Color.clear;
                noveltySliderHandle.color = Color.clear;
            }
        }
	}
}
