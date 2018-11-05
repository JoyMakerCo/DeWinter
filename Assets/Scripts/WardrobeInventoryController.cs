using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Ambition
{
    public class WardrobeInventoryController : MonoBehaviour
    {
        public GameObject AccessoryInventory;
        public GameObject AccessoryListContent; //This is where we spawn the button prefabs for Accessories
        public GameObject AccessoryButtonPrefab;

        public GameObject OutfitInventory;
        public GameObject OutfitListContent; //This is where we spawn the button prefabs for Outfits
        public GameObject OutfitButtonPrefab;

        public Text AccessoryLimitText; //Use to display the amount of inventory slots the player has, and the amount they have left, not used in the Fatima's inventory display
        public Text OutfitLimitText;

        public Text AccessoryText;
        public Shadow AccessoryTextShadow;
        public Shadow OutfitTextShadow;

        public Color EnabledTextColor;
        public Color DisabledTextColor;

        InventoryModel _inventorymodel = AmbitionApp.GetModel<InventoryModel>();
        CalendarModel _calendarmodel = AmbitionApp.GetModel<CalendarModel>();
        private List<OutfitVO> _outfitDisplayList = new List<OutfitVO>(); //This is necessary for sorting, since not all Item VOs in the inventory are Outfits or Accessories;
        private List<ItemVO> _accessoryDisplayList = new List<ItemVO>(); //This is necessary for sorting, since not all Item VOs in the inventory are Outfits or Accessories;

        //public bool PlayerInventory; //Set to true if it's the players. Set to false if this inventory belongs to Fatima.
        public enum InventoryType { Player, Shop, Loadout };
        public InventoryType InventoryOwner;
        private bool _noaccessories; // Used to disable the accessories menu if nothing is there.

        public FMODUnity.StudioEventEmitter OnClick;
        public FMODUnity.StudioEventEmitter OnDisabled;

        public Text OutfitSortText;
        public Image OutfitSortImage;
        public Image OutfitSortArrowImage;
        public Color LightSortBackgroundColor;
        public Color DarkSortBackgroundColor;
        public GameObject OutfitSortList;
        public Color LightSortTextColor;
        public Color DarkSortTextColor;

        public GameObject SortLabelButtonPrefab;

        private List<string> _sortTypes;

        private void Awake()
        {
            AmbitionApp.Subscribe(InventoryMessages.BUY_ITEM, PopulateInventory); //Gotta wipe the slate once the item's been moved
            AmbitionApp.Subscribe(InventoryMessages.SELL_ITEM, PopulateInventory);
            AmbitionApp.Subscribe<string>(InventoryMessages.SORT_INVENTORY, SortInventory);
            SetSortTypes();
            SwitchToOutfitInventory();
            MerchantCheck();
            PopulateInventory();
        }

        private void OnDestroy()
        {
            AmbitionApp.Unsubscribe(InventoryMessages.BUY_ITEM, PopulateInventory);
            AmbitionApp.Unsubscribe(InventoryMessages.SELL_ITEM, PopulateInventory);
            AmbitionApp.Unsubscribe<string>(InventoryMessages.SORT_INVENTORY, SortInventory);
        }

        public void AccessoriesTextHoverStateOn()
        {
            if (!_noaccessories) //No need to highlight if there are no items in the accessory inventory
            {
                AccessoryTextShadow.enabled = true;
            }
        }

        public void AccessoriesTextHoverStateOff()
        {
            AccessoryTextShadow.enabled = false;
        }

        public void OutfitTextHoverStateOn()
        {
            OutfitTextShadow.enabled = true;
        }

        public void OutfitTextHoverStateOff()
        {
            OutfitTextShadow.enabled = false;
        }

        public void SwitchToOutfitInventory()
        {
            OutfitInventory.transform.SetAsLastSibling();
        }

        public void SwitchToAccessoriesInventory()
        {
            if (!_noaccessories)
            {
                AccessoryInventory.transform.SetAsLastSibling();
            }
        }

        void MerchantCheck()
        {
            if (InventoryOwner == InventoryType.Shop)
            {
                print("Executing the Restock Inventory Command!");
                RestockMerchantCmd restock = new RestockMerchantCmd();
                restock.Execute(_calendarmodel.Today);
                OutfitLimitText.text = ""; //Fatima's inventory has no limits, so these blank out
                AccessoryLimitText.text = "";
            }
        }

        void PopulateInventory()
        {
            List<ItemVO> scanList;
            if (InventoryOwner == InventoryType.Shop) scanList = _inventorymodel.Market; //If it's Fatima's shop
            else scanList = _inventorymodel.Inventory;
            //Clearing the display lists out to prevent duplicates
            _outfitDisplayList.Clear();
            _accessoryDisplayList.Clear();
            foreach (ItemVO i in scanList)
            {
                OutfitVO outfitTest = i as OutfitVO; //This is to test if the item is an Outfit
                if (outfitTest != null)
                {
                    _outfitDisplayList.Add(outfitTest);
                }
                else if (i.Type == "Accessory")//To Do: Make this actually populate the list, but we first need working accessories again (I mean, they used to, sort of work)
                {
                    _accessoryDisplayList.Add(i);
                }
            }
            AccessoriesCountCheck(_accessoryDisplayList.Count);
            DisplayInventory();
        }

        void DisplayInventory()
        {
            DestroyInventoryChildren(); //Gotta start fresh
            foreach (OutfitVO o in _outfitDisplayList)
            {
                GameObject outfitButton = Instantiate(OutfitButtonPrefab, OutfitListContent.transform);
                OutfitButtonMediator outfitButtonMediator = outfitButton.GetComponent<OutfitButtonMediator>();
                outfitButtonMediator.SetItem(o);
                outfitButtonMediator.SetInventoryOwner(InventoryOwner.ToString());
            }
            //To Do: Make this actually populate the list, but we first need working accessories again (I mean, they used to, sort of work)
            /*
            foreach (ItemVO i in _accessoryDisplayList)
            {
                GameObject outfitButton = Instantiate(OutfitButtonPrefab, OutfitListContent.transform);
                OutfitButtonMediator outfitButtonMediator = outfitButton.GetComponent<OutfitButtonMediator>();
                outfitButtonMediator.SetItem(o);
                outfitButtonMediator.SetInventoryOwner(InventoryOwner.ToString());
            }*/
            if (InventoryOwner == InventoryType.Player || InventoryOwner == InventoryType.Loadout)
            {
                OutfitLimitText.text = _outfitDisplayList.Count + "/" + _inventorymodel.MaxOutfits;
                AccessoryLimitText.text = _accessoryDisplayList.Count + "/" + _inventorymodel.MaxAccessories;
            }
        }

        void AccessoriesCountCheck(int count) // This is for determining if there are no accessories, which disables that tab if there are none
        {
            if (count == 0)
            {
                _noaccessories = true;
                AccessoryText.color = DisabledTextColor;
            }
            else
            {
                _noaccessories = false;
                AccessoryText.color = EnabledTextColor;
            }
        }

        public void AccessoriesButtonClickSound()
        {
            if (_noaccessories) 
            {
                OnDisabled.Play();
            }
            else
            {
                OnClick.Play();
            }
        }
        
        public void DestroyInventoryChildren()
        {
            foreach (Transform child in AccessoryListContent.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            foreach (Transform child in OutfitListContent.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        void SetSortTypes()
        {
            _sortTypes = new List<string>();
            _sortTypes.Add("Novelty");
            _sortTypes.Add("Luxury");
            _sortTypes.Add("Modesty");
            _sortTypes.Add("Style");
        }

        public void DeployOutfitSortList()
        {
            OutfitSortArrowImage.transform.rotation = Quaternion.Euler(0, 0, 180);
            OutfitSortList.SetActive(true);
            if (OutfitSortList.transform.childCount == 0) //This stops the list from making more options if the player keeps clicking on it
            {
                foreach (string s in _sortTypes) //No duplicating what's already been selected
                {
                    if (s != OutfitSortText.text)
                    {
                        GameObject sortLabel = Instantiate(SortLabelButtonPrefab, OutfitSortList.transform);
                        SortLabelButtonMediator sortButtonMediator = sortLabel.GetComponent<SortLabelButtonMediator>();
                        sortButtonMediator.SetLabelString(s);
                    }
                }
            }
        }

        public void HideOutfitSortList()
        {
            OutfitSortArrowImage.transform.rotation = Quaternion.Euler(0, 0, 270);
            foreach (Transform child in OutfitSortList.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            OutfitSortList.SetActive(false);
        }

        public void SortListMouseOver()
        {
            OutfitSortImage.color = DarkSortBackgroundColor;
            OutfitSortText.color = LightSortTextColor;
            OutfitSortArrowImage.color = LightSortTextColor;
        }

        public void SortListMouseClick()
        {
            DeployOutfitSortList();
        }

        public void SortListMouseExit()
        {
            OutfitSortImage.color = LightSortBackgroundColor;
            OutfitSortText.color = DarkSortTextColor;
            OutfitSortArrowImage.color = DarkSortTextColor;
        }

        void SortInventory(string sort)
        {
            HideOutfitSortList();
            OutfitSortText.text = sort;
            List<OutfitVO> sortList;
            //To Do: The Part where the actual inventory list gets sorted
            switch (sort)
            {
                case "Novelty":
                    sortList = _outfitDisplayList.OrderByDescending(OutfitVO => OutfitVO.Novelty).ToList();
                    break;
                case "Modesty":
                    sortList = _outfitDisplayList.OrderByDescending(OutfitVO => OutfitVO.Modesty).ToList();
                    break;
                case "Luxury":
                    sortList = _outfitDisplayList.OrderByDescending(OutfitVO => OutfitVO.Luxury).ToList();
                    break;
                case "Style":
                    sortList = _outfitDisplayList.OrderBy(OutfitVO => OutfitVO.Style).ToList();
                    break;
                default:
                    sortList = _outfitDisplayList.OrderByDescending(OutfitVO => OutfitVO.Novelty).ToList();
                    break;
            }
            _outfitDisplayList = sortList;
            DisplayInventory();
        }
    }
}
