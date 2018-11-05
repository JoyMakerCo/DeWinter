using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class SellOrPeddleGossip : MonoBehaviour
    {
        public Text GossipValue;
        private Gossip _gossip;

        private void Awake()
        {
            _gossip = null;
            BlankStats();
            AmbitionApp.Subscribe<Gossip>(InventoryMessages.DISPLAY_GOSSIP, DisplayGossip);
            AmbitionApp.Subscribe(InventoryMessages.SELL_GOSSIP, BlankStats);
            AmbitionApp.Subscribe(InventoryMessages.PEDDLE_GOSSIP, BlankStats);
        }

        private void OnDestroy()
        {
            AmbitionApp.Unsubscribe<Gossip>(InventoryMessages.DISPLAY_GOSSIP, DisplayGossip);
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

        public Gossip Gossip()
        {
            return _gossip;
        }

        void DisplayGossip(Gossip gossip)
        {
            _gossip = gossip;
            GossipValue.text = "£" + _gossip.LivreValue().ToString("### ###");
        }

        void BlankStats()
        {
            GossipValue.text = "";
            _gossip = null;
        }
    }
}

