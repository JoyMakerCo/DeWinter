using System;
using UnityEngine;
using UnityEngine.UI;
namespace Ambition
{
    public class MerchantView : SceneView, ISubmitHandler
    {
        public bool IsMerchant;
        public Button BuyButton;
        public Text BuyButtonText;
        public Text BuyButtonCostText;

        private ItemVO _outfit;

        public void SetMerchantMode(bool isMerchant)
        {
            IsMerchant = isMerchant;
            BuyButtonText.text = AmbitionApp.Localize(isMerchant ? "purchase" : "sell");
            BuyButtonCostText.text = "";
            BuyButton.interactable = false;
            AmbitionApp.SendMessage(InventoryMessages.UPDATE_MERCHANT);
        }

        public void ClickBuyButton()
        {
            if (_outfit != null)
            {
                AmbitionApp.SendMessage(IsMerchant ? InventoryMessages.BUY_ITEM : InventoryMessages.SELL_ITEM, _outfit);
            }
            _outfit = null;
            BuyButton.interactable = false;
        }

        public void Submit() { }
        public void Cancel()
        {
#if UNITY_STANDALONE
            AmbitionApp.OpenDialog(DialogConsts.GAME_MENU);
#else
#endif
        }

        private void OnEnable()
        {
            AmbitionApp.Subscribe<ItemVO>(InventoryMessages.BROWSE, HandleBrowse);
            SetMerchantMode(true);
        }

        private void OnDisable()
        {
            AmbitionApp.Unsubscribe<ItemVO>(InventoryMessages.BROWSE, HandleBrowse);
        }

        private void HandleBrowse(ItemVO item)
        {
            _outfit = item?.Type == ItemType.Outfit ? item : null;
            BuyButton.interactable = (_outfit != null) 
                && ((IsMerchant && _outfit.Price <= AmbitionApp.Game.Livre)
                || (!IsMerchant && !_outfit.Permanent));
            BuyButtonCostText.text = _outfit == null ? "" : "£" + _outfit.Price;
        }
    }
}
