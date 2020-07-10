using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class GossipInventoryController : MonoBehaviour
    {
        public GameObject GossipListContent; //This is where we spawn the button prefabs for Outfits
        public GameObject GossipButtonPrefab;

        public Text GossipSortText;
        public Image GossipSortImage;
        public Image GossipSortArrowImage;
        public Color LightSortBackgroundColor;
        public Color DarkSortBackgroundColor;
        public GameObject GossipSortList;
        public Color LightSortTextColor;
        public Color DarkSortTextColor;
        public IComparer SortByID;
        public IComparer SortByDate;
        public IComparer SortByTier;

        public GameObject GossipSortLabelButtonPrefab;

        private List<string> _sortTypes;

        private void Awake()
        {
            AmbitionApp.Subscribe<ItemVO>(InventoryMessages.PEDDLE_GOSSIP, PopulateInventory); //Gotta wipe the slate once the item's been moved
            AmbitionApp.Subscribe<ItemVO>(InventoryMessages.SELL_GOSSIP, PopulateInventory);
            AmbitionApp.Subscribe<string>(InventoryMessages.SORT_INVENTORY, SortInventory);
            SetSortTypes();
            HideGossipSortList();
            SortInventory("Tier"); //Contains Populate Inventory in it already
        }

        private void OnDestroy()
        {
            AmbitionApp.Unsubscribe<ItemVO>(InventoryMessages.PEDDLE_GOSSIP, PopulateInventory);
            AmbitionApp.Unsubscribe<ItemVO>(InventoryMessages.SELL_GOSSIP, PopulateInventory);
            AmbitionApp.Unsubscribe<string>(InventoryMessages.SORT_INVENTORY, SortInventory);
        }
        
        void PopulateInventory(ItemVO soldItem)
        {
            Debug.Log("GossipInventoryController.PopulateInventory");
            DestroyInventoryChildren(); //Gotta start fresh
            foreach (ItemVO item in AmbitionApp.GetModel<InventoryModel>().Inventory)
            {
                if (item.Type == ItemType.Gossip)
                {
                    GameObject gossipButton = Instantiate(GossipButtonPrefab, GossipListContent.transform);
                    GossipButtonMediator gossipButtonMediator = gossipButton.GetComponent<GossipButtonMediator>();
                    gossipButtonMediator.SetItem(item);
                }
            }
        }

        public void DestroyInventoryChildren()
        {
            foreach (Transform child in GossipListContent.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        void SetSortTypes()
        {
            _sortTypes = new List<string>();
            _sortTypes.Add("Tier");
            _sortTypes.Add("Faction");
            _sortTypes.Add("Freshness");
        }

        public void DeployGossipSortList()
        {
            GossipSortArrowImage.transform.rotation = Quaternion.Euler(0, 0, 180);
            GossipSortList.SetActive(true);
            if (GossipSortList.transform.childCount == 0) //This stops the list from making more options if the player keeps clicking on it
            {
                foreach (string s in _sortTypes) //No duplicating what's already been selected
                {
                    if(s != GossipSortText.text)
                    {
                        GameObject sortLabel = Instantiate(GossipSortLabelButtonPrefab, GossipSortList.transform);
                        SortLabelButtonMediator sortButtonMediator = sortLabel.GetComponent<SortLabelButtonMediator>();
                        sortButtonMediator.SetLabelString(s);
                    }
                }
            }
        }

        public void HideGossipSortList()
        {
            GossipSortArrowImage.transform.rotation = Quaternion.Euler(0, 0, 270);
            foreach (Transform child in GossipSortList.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            GossipSortList.SetActive(false);
        }

        public void SortListMouseOver()
        {
            GossipSortImage.color = DarkSortBackgroundColor;
            GossipSortText.color = LightSortTextColor;
            GossipSortArrowImage.color = LightSortTextColor;
        }

        public void SortListMouseClick()
        {
            DeployGossipSortList();
        }

        public void SortListMouseExit()
        {
            GossipSortImage.color = LightSortBackgroundColor;
            GossipSortText.color = DarkSortTextColor;
            GossipSortArrowImage.color = DarkSortTextColor;
        }

        void SortInventory(string sort)
        {
            InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
            HideGossipSortList();
            GossipSortText.text = sort;
            switch (sort)
            {
                case "Tier": inventory.Inventory.Sort((a, b) => a.Price.CompareTo(b.Price)); break;
                case "Freshness": inventory.Inventory.Sort((a, b) => a.Created.CompareTo(b.Created)); break;
                default: inventory.Inventory.Sort((a, b) => a.ID.CompareTo(b.ID)); break;
            }
            PopulateInventory(null);
        }
    }
}
