using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Ambition;

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
	void MUpdate () {
        if (inventoryType == ItemConsts.PERSONAL)
        {
            if (outfitInventoryList.selectedInventoryOutfit != null)
            {
                //Text
                OutfitVO displayOutfit = outfitInventoryList.selectedInventoryOutfit;
                outfitNameText.text = displayOutfit.Name;
                displayOutfit.CalculatePrice(true);
                outfitCostText.text = displayOutfit.Price.ToString("£" + "#,##0");
                outfitSaleButtonText.text = "Sell for " + displayOutfit.Price.ToString("£" + "#,##0");
                //Sliders
                modestySlider.value = displayOutfit.Modesty;
                luxurySlider.value = displayOutfit.Luxury;
                noveltySlider.value = displayOutfit.Novelty;
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
        }  else if (inventoryType == ItemConsts.MERCHANT)
        {
            if (outfitInventoryList.selectedInventoryOutfit != null)
            {
                //Text
                OutfitVO displayOutfit = outfitInventoryList.selectedInventoryOutfit;
                outfitNameText.text = displayOutfit.Name;
                displayOutfit.CalculatePrice(false);
                outfitCostText.text = displayOutfit.Price.ToString("£" + "#,##0");
                outfitSaleButtonText.text = "Buy for " + displayOutfit.Price.ToString("£" + "#,##0");
                //Sliders
                modestySlider.value = displayOutfit.Modesty;
                luxurySlider.value = displayOutfit.Luxury;
                noveltySlider.value = displayOutfit.Novelty;
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
            if (outfitInventoryList.selectedInventoryOutfit != null) //If an Outfit is Selected
            {
                //Text
                OutfitVO displayOutfit = outfitInventoryList.selectedInventoryOutfit;
                outfitNameText.text = displayOutfit.Name;
				displayOutfit.CalculatePrice(true);
                outfitCostText.text = displayOutfit.Price.ToString("£" + "#,##0"); //Uses Personal Inventory Prices
                //Sliders
                modestySlider.value = displayOutfit.Modesty;
                luxurySlider.value = displayOutfit.Luxury;
                noveltySlider.value = displayOutfit.Novelty;
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
