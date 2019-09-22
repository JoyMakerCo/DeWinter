using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class SellOrPeddleGossip : MonoBehaviour
    {
        public Text GossipValue;
        private ItemVO _gossip = null;

        private void Awake()
        {
            BlankStats();
            AmbitionApp.Subscribe<ItemVO>(InventoryMessages.DISPLAY_GOSSIP, DisplayGossip);
            AmbitionApp.Subscribe(InventoryMessages.SELL_GOSSIP, BlankStats);
            AmbitionApp.Subscribe(InventoryMessages.PEDDLE_GOSSIP, BlankStats);
        }

        private void OnDestroy()
        {
            AmbitionApp.Unsubscribe<ItemVO>(InventoryMessages.DISPLAY_GOSSIP, DisplayGossip);
            AmbitionApp.Unsubscribe(InventoryMessages.SELL_GOSSIP, BlankStats);
            AmbitionApp.Unsubscribe(InventoryMessages.PEDDLE_GOSSIP, BlankStats);
        }

        public void SellGossipPopUp() //This has to be handled in a pop-up because the player has to evaluate the odds and confirm their decision
        {
            if(_gossip != null) AmbitionApp.OpenDialog("SELL_GOSSIP", _gossip);      
        }

        public void PeddleInfluencePopUp() //This has to be handled in a pop-up because the player has to evaluate the odds and confirm their decision
        {
            if (_gossip != null) AmbitionApp.OpenDialog("PEDDLE_INFLUENCE", _gossip);
        }

        public ItemVO Gossip() => _gossip;

        void DisplayGossip(ItemVO gossip)
        {
            System.DateTime today = AmbitionApp.GetModel<CalendarModel>().Today;
            int dx = (today - gossip.Created).Days;
            int price;
            _gossip = gossip;

            if (dx > 12) price = 0;
            else if (dx > 9) price = 50;
            else if (dx > 6) price = 100;
            else price = 150;
            GossipValue.text = "£" + price.ToString("### ###");
        }

        void BlankStats()
        {
            GossipValue.text = "";
            _gossip = null;
        }
    }
}

