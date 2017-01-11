using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GossipStatsTracker : MonoBehaviour {

    private Text gossipNameText;
    private Text gossipFlavorText;
    private Text gossipLivreText;
    private Text gossipPowerText;
    private Text gossipAllegianceText;

    private Slider freshnessSlider;

    private Image freshnessSliderHandle;

    //Inventory List
    public GossipInventoryList gossipInventoryList;

    // Use this for initialization
    void Start()
    {
        //Text
        gossipNameText = this.transform.Find("GossipNameText").GetComponent<Text>();
        gossipFlavorText = this.transform.Find("GossipFlavorText").GetComponent<Text>();
        gossipLivreText = this.transform.Find("SellForLivreButton").Find("Text").GetComponent<Text>();
        gossipPowerText = this.transform.Find("PropogandaForPowerButton").Find("Text").GetComponent<Text>();
        gossipAllegianceText = this.transform.Find("LeakForAllegianceButton").Find("Text").GetComponent<Text>();
        //Sliders
        freshnessSlider = this.transform.Find("FreshnessText").Find("Slider").GetComponent<Slider>();
        //Slider Handles
        freshnessSliderHandle = freshnessSlider.transform.Find("Handle Slide Area").Find("Handle").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gossipInventoryList.selectedGossipItem != -1)
        {
            //Text
            Gossip displayGossip = GameData.gossipInventory[gossipInventoryList.selectedGossipItem];
            gossipNameText.text = displayGossip.Name();
            gossipFlavorText.text = displayGossip.flavorText;
            //Sliders
            freshnessSlider.value = displayGossip.freshness;
            //Make Sure Slider Handles are Visible
            freshnessSliderHandle.color = Color.white;
         } else
         {
            gossipNameText.text = "";
            gossipFlavorText.text = "";
            //Make Sure Slider Handles are Invisible
            freshnessSliderHandle.color = Color.clear;
         }
    }
}