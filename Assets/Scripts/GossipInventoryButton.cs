using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class GossipInventoryButton : MonoBehaviour {
    public int gossipID;
    private Text myDescriptionText;
    private Text myFreshnessText;
    private Outline myOutline; // This is for highlighting buttons

    GossipInventoryList gossipInventoryList;

    void Start()
    {
        myDescriptionText = this.transform.Find("DescriptionText").GetComponent<Text>();
        myFreshnessText = this.transform.Find("FreshnessText").GetComponent<Text>();
        myOutline = this.GetComponent<Outline>();
        gossipInventoryList = this.transform.parent.GetComponent<GossipInventoryList>();
    }

    void Update()
    {
        DisplayGossipStats(gossipID);
        if (gossipInventoryList.selectedGossipItem == gossipID)
        {
            myOutline.effectColor = Color.yellow;
        }
        else
        {
            myOutline.effectColor = Color.clear;
        }
    }

    public void DisplayGossipStats(int gID)
    {
		Gossip g = GameData.gossipInventory.ElementAtOrDefault(gID);
        if (g != null)
        {
            myDescriptionText.text = g.Name();
            myFreshnessText.text = g.freshness + "/10";
        }
    }

    public void SetInventoryItem()
    {
        Debug.Log("Selected Inventory Item: " + gossipID.ToString());
        gossipInventoryList.selectedGossipItem = gossipID;
    }
}
