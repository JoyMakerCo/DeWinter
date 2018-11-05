using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        public GameObject GossipSortLabelButtonPrefab;

        private List<string> _sortTypes;

        InventoryModel _inventorymodel = AmbitionApp.GetModel<InventoryModel>();

        private void Awake()
        {
            AmbitionApp.Subscribe(InventoryMessages.PEDDLE_GOSSIP, PopulateInventory); //Gotta wipe the slate once the item's been moved
            AmbitionApp.Subscribe(InventoryMessages.SELL_GOSSIP, PopulateInventory);
            AmbitionApp.Subscribe<string>(InventoryMessages.SORT_INVENTORY, SortInventory);
            _inventorymodel.GossipSoldOrPeddled = 0; //This is used for determining if the player was caught selling gossip 
            SetSortTypes();
            HideGossipSortList();
            SortInventory("Tier"); //Contains Populate Inventory in it already
        }

        private void OnDestroy()
        {
            AmbitionApp.Unsubscribe(InventoryMessages.PEDDLE_GOSSIP, PopulateInventory);
            AmbitionApp.Unsubscribe(InventoryMessages.SELL_ITEM, PopulateInventory);
            AmbitionApp.Unsubscribe<string>(InventoryMessages.SORT_INVENTORY, SortInventory);
        }
        
        void PopulateInventory()
        {
            DestroyInventoryChildren(); //Gotta start fresh
            List<Gossip> gossipList = _inventorymodel.GossipItems;
            foreach (Gossip g in gossipList)
            {
                GameObject gossipButton = Instantiate(GossipButtonPrefab, GossipListContent.transform);
                GossipButtonMediator gossipButtonMediator = gossipButton.GetComponent<GossipButtonMediator>();
                gossipButtonMediator.SetItem(g);
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
            HideGossipSortList();
            GossipSortText.text = sort;
            List<Gossip> sortList;
            switch (sort)
            {
                case "Tier":
                    sortList = _inventorymodel.GossipItems.OrderByDescending(Gossip => Gossip.Tier()).ToList();
                    break;
                case "Faction":
                    sortList = _inventorymodel.GossipItems.OrderBy(Gossip => Gossip.Faction).ToList();
                    break;
                case "Freshness":
                    sortList = _inventorymodel.GossipItems.OrderBy(Gossip => Gossip.Freshness).ToList();
                    break;
                default:
                    sortList = _inventorymodel.GossipItems.OrderBy(Gossip => Gossip.Tier()).ToList();
                    break;
            }
            _inventorymodel.GossipItems = sortList;
            PopulateInventory();
        }
    }
}
