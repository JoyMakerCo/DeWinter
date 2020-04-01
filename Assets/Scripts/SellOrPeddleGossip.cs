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
            BlankStats(null);
            AmbitionApp.Subscribe<ItemVO>(InventoryMessages.DISPLAY_GOSSIP, DisplayGossip);
            AmbitionApp.Subscribe<ItemVO>(InventoryMessages.SELL_GOSSIP, BlankStats);
            AmbitionApp.Subscribe<ItemVO>(InventoryMessages.PEDDLE_GOSSIP, BlankStats);
        }

        private void OnDestroy()
        {
            AmbitionApp.Unsubscribe<ItemVO>(InventoryMessages.DISPLAY_GOSSIP, DisplayGossip);
            AmbitionApp.Unsubscribe<ItemVO>(InventoryMessages.SELL_GOSSIP, BlankStats);
            AmbitionApp.Unsubscribe<ItemVO>(InventoryMessages.PEDDLE_GOSSIP, BlankStats);
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
            Debug.Log("SellOrPeddleGossip.DisplayGossip (nop)");
            _gossip = gossip;

            // Is this even necessary? GossipItemStatsMediator has it covered.
            GossipValue.text = "£ " + GossipWrapperVO.GetValue( gossip ).ToString("### ###");
        }

        void BlankStats( ItemVO gossip )
        {
            if (_gossip != gossip)
            {
                Debug.LogWarning("SellOrPeddleGossip.BlankStats - mismatched gossip item");
            }
            _gossip = null;
            // Is this even necessary? GossipItemStatsMediator has it covered.
            GossipValue.text = "";
        }
    }
}