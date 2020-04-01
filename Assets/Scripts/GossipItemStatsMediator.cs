using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Ambition
{
    public class GossipItemStatsMediator : MonoBehaviour
    {
        public Image GossipFactionSymbol;
        public Text GossipNameText;
        public Text GossipDescriptionText;
        public Text GossipEffectText;
        public Slider FreshnessSlider;
        public Text GossipValue;

        public Sprite CrownSymbol;
        public Sprite ChurchSymbol;
        public Sprite MilitarySymbol;
        public Sprite BourgeoisieSymbol;
        public Sprite RevolutionSymbol;

        private Dictionary<string, Sprite> _factionSymbolList;

        // Use this for initialization
        void Awake()
        {
            BlankStats(null);
            SetUpDictionary();
            AmbitionApp.Subscribe<ItemVO>(InventoryMessages.DISPLAY_GOSSIP, HandleGossipStats);
            AmbitionApp.Subscribe<ItemVO>(InventoryMessages.PEDDLE_GOSSIP, BlankStats); //Gotta wipe the slate once the item's been moved
            AmbitionApp.Subscribe<ItemVO>(InventoryMessages.SELL_GOSSIP, BlankStats);
        }

        void OnDestroy()
        {
            AmbitionApp.Unsubscribe<ItemVO>(InventoryMessages.DISPLAY_GOSSIP, HandleGossipStats);
            AmbitionApp.Unsubscribe<ItemVO>(InventoryMessages.PEDDLE_GOSSIP, BlankStats); //Gotta wipe the slate once the item's been moved
            AmbitionApp.Unsubscribe<ItemVO>(InventoryMessages.SELL_GOSSIP, BlankStats);
        }

        void BlankStats( ItemVO soldItem )
        {
            Debug.Log("GossipItemStatsMediator.BlankStats");
            GossipFactionSymbol.color = Color.clear;
            GossipNameText.text = "";
            GossipDescriptionText.text = "";
            GossipEffectText.text = "";
            FreshnessSlider.value = 0;
            GossipValue.text = "";
        }

        void HandleGossipStats(ItemVO gossip)
        {
            Debug.Log("GossipItemStatsMediator.HandleGossipStats");
            Sprite factionSprite;
            //System.DateTime today = AmbitionApp.GetModel<CalendarModel>().Today;
            _factionSymbolList.TryGetValue(gossip.ID.ToString(), out factionSprite);
            GossipFactionSymbol.color = Color.white;
            GossipFactionSymbol.sprite = factionSprite;
            GossipNameText.text = gossip.Name;
            GossipDescriptionText.text = GossipWrapperVO.GetDescription(gossip); //To Do: Make gossip items generate description text properly
            //GossipEffectText.text = gossip.PoliticalEffectDescriptionText();
            FreshnessSlider.value = GossipWrapperVO.GetRelevanceSliderValue(gossip);
            GossipValue.text = "£ " + GossipWrapperVO.GetValue( gossip ).ToString("### ###");
        }

        private void SetUpDictionary()
        {
            _factionSymbolList = new Dictionary<string, Sprite>();
            _factionSymbolList.Add("Crown", CrownSymbol);
            _factionSymbolList.Add("Church", ChurchSymbol);
            _factionSymbolList.Add("Military", MilitarySymbol);
            _factionSymbolList.Add("Bourgeoisie", BourgeoisieSymbol);
            _factionSymbolList.Add("Revolution", RevolutionSymbol);
        }
    }
}

