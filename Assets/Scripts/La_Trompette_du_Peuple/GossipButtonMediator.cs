using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ambition;

public class GossipButtonMediator: MonoBehaviour {
    public Image ChoicePip; // This is the icon that appears next the item to make them all easier to read. Necessary for the event triggers
    public Sprite ChoicePipUp;
    public Sprite ChoicePipHover;
    public Sprite ChoicePipSelected;

    public Text GossipNameText;
    public Text GossipRelevanceText;
    public Image GossipIcon;

    public Sprite CrownSymbol;
    public Sprite ChurchSymbol;
    public Sprite MilitarySymbol;
    public Sprite BourgeoisieSymbol;
    public Sprite RevolutionSymbol;

    private Dictionary<string, Sprite> _factionSymbolList;

    private ItemVO _gossip;

    void Awake()
    {
        MouseLeaves();
        SetUpDictionary();
    }

    public void MouseOver()
    {
        ChoicePip.sprite = ChoicePipHover;
    }

    public void MouseLeaves()
    {
        ChoicePip.sprite = ChoicePipUp;
    }

    public void MouseClick()
    {
        ChoicePip.sprite = ChoicePipSelected;
        AmbitionApp.SendMessage(InventoryMessages.DISPLAY_GOSSIP, _gossip);
    }

    public void SetItem(ItemVO gossip)
    {
        _gossip = gossip;
        System.DateTime today = AmbitionApp.GetModel<CalendarModel>().Today;
        GossipNameText.text = gossip.Name;

        var relevance = GossipWrapperVO.GetRelevance(gossip);
        GossipRelevanceText.text = "- " + AmbitionApp.Localize("gossip_relevance_" + relevance); // + " days left");       
        Sprite factionSprite;
        _factionSymbolList.TryGetValue(gossip.ID, out factionSprite);
        GossipIcon.sprite = factionSprite;
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
