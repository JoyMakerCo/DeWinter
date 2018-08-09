using UnityEngine;
using System.Collections;

public class GossipInventoryList : MonoBehaviour {

    public int selectedGossipItem;
    public int gossipItemsSoldToday;
    public GameObject gossipInventoryButtonPrefab;

    // Use this for initialization
    void Start()
    {
        foreach (Gossip g in GameData.gossipInventory)
        {
            g.freshness--;
            if(g.freshness <= 0)
            {
                GameData.gossipInventory.Remove(g);
            }
        }
        GenerateInventoryButtons();
        selectedGossipItem = -1; // So nothing is selected at the start
        gossipItemsSoldToday = 0;
    }

    public void GenerateInventoryButtons()
    {
        for (int i = 0; i < GameData.gossipInventory.Count; i++)
        {
            GameObject button = GameObject.Instantiate(gossipInventoryButtonPrefab);
            GossipInventoryButton buttonStats = button.GetComponent<GossipInventoryButton>();
            buttonStats.gossipID = i;
            button.transform.SetParent(this.transform, false);
            Debug.Log("Gossip Button: " + i + " is made!");
        }
    }

    public void ClearInventoryButtons()
    {
        foreach (Transform child in this.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
