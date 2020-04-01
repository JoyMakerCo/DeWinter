using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class BuyAndSellItems : MonoBehaviour
    {
        public Text ItemCostText; //Switches between 'Cost' and 'Value', depending on whether the player is buying or selling
        public Text ItemPriceText;
        public Text BuyOrSellItemText;

        public string CostString;
        public string ValueString;

        public string BuyOutfitString;
        public string SellOutfitString;
        public string BuyAccessoryString;
        public string SellAccessoryString;

        public Button buyButton;

        public Color DisabledTextColor;
        public Color EnabledTextColor;

        private ItemVO _item; //The Buy Or Sell Item command has to be activated via the Event Trigger system, so the item has to be stored here

        void Awake()
        {
            AmbitionApp.Subscribe<ItemVO>(InventoryMessages.DISPLAY_ITEM, HandleItemDisplay);
            BlankStats();
        }

        void OnDestroy()
        {
            AmbitionApp.Unsubscribe<ItemVO>(InventoryMessages.DISPLAY_ITEM, HandleItemDisplay);
        }

        void HandleItemDisplay(ItemVO item)
        {
            Debug.Log("BuyAndSellItems.HandleItemDisplay");
            if (item?.Type != ItemType.Outfit) return;

                        Debug.Log("BuyAndSellItems.HandleItemDisplay - outfit confirmed");

            InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
            List<ItemVO> items;
            _item = item;


            if (inventory.Market.TryGetValue(_item.Type, out items) && items.Contains(_item)) //If the item is in Fatima's shop
            {
                            Debug.Log("BuyAndSellItems.HandleItemDisplay - market item");

                ItemCostText.text = CostString;
                BuyOrSellItemText.text = BuyOutfitString;
                ItemPriceText.text = _item.Price.ToString("£" + "#,##0");

                if (_item.Price <= AmbitionApp.GetModel<GameModel>().Livre)
                {
                    buyButton.interactable = true;
                    BuyOrSellItemText.color = EnabledTextColor;
                }
                else
                {
                    buyButton.interactable = false;
                    BuyOrSellItemText.color = DisabledTextColor;
                }

            } else //If the item is not in Fatima's shop
            {
                                            Debug.Log("BuyAndSellItems.HandleItemDisplay - player item");

                ItemCostText.text = ValueString;
                BuyOrSellItemText.text = SellOutfitString;
                ItemPriceText.text = (_item.Price*inventory.SellbackMultiplier).ToString("£" + "#,##0");
                buyButton.interactable = true;
                BuyOrSellItemText.color = EnabledTextColor;
            }
        }

        void BlankStats()
        {
            ItemPriceText.text = "£0";
            ItemCostText.text = ValueString;
            _item = null;
            buyButton.interactable = false;
            BuyOrSellItemText.color = DisabledTextColor;
        }

        public void BuyOrSellItem()
        {
            InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
            List<ItemVO> items;
            if (inventory.Market == null)
            {
                Debug.LogError("BuyAndSellItems.BuyOrSellItem - inventory.Market is null");
                return;
            }
            if (inventory.Market.TryGetValue(_item.Type, out items) && items.Contains(_item)) //If buying the item
            {
                AmbitionApp.SendMessage(InventoryMessages.BUY_ITEM, _item); // Some methods require the Item VO, some don't
                AmbitionApp.SendMessage(InventoryMessages.BUY_ITEM); //This felt cleaner than making methods that took unnecessary variables just to satisfy the message conditions
            } else //If selling the item
            {
                AmbitionApp.SendMessage(InventoryMessages.SELL_ITEM, _item); // Some methods require the Item VO, some don't.
                AmbitionApp.SendMessage(InventoryMessages.SELL_ITEM);
            }
            BlankStats(); //Gotta reset all the values after the purchase has been made
        }
    }
}

