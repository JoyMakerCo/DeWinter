using System;
using UnityEngine;
using UnityEngine.UI;
namespace Ambition
{
    public class ShopView : MonoBehaviour
    {
        // PUBLIC DATA //////////////////////////////
        public Button BuyBtn;
        public Text BuyText;

        // PRIVATE DATA //////////////////////////////
        private int _livre;
        private ItemVO _item;

        // PRIVATE METHODS //////////////////////////////
        private void Awake()
        {
            AmbitionApp.Subscribe<ItemVO>(InventoryMessages.EQUIP, HandleEquipItem);
            AmbitionApp.Subscribe<int>(GameConsts.LIVRE, HandleLivre);
        }

        private void OnDestroy()
        {
            AmbitionApp.Unsubscribe<ItemVO>(InventoryMessages.EQUIP, HandleEquipItem);
            AmbitionApp.Unsubscribe<int>(GameConsts.LIVRE, HandleLivre);
        }

        private void HandleEquipItem(ItemVO item)
        {
            _item = item;
            UpdateButton();
        }

        private void HandleLivre(int livre)
        {
            _livre = livre;
            UpdateButton();
        }

        private void UpdateButton()
        {
            BuyBtn.interactable = _livre >= _item?.Price;
        }

    }
}
