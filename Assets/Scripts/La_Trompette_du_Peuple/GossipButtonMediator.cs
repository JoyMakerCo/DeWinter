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

    private Gossip _gossip;

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

    public void SetItem(Gossip gossip)
    {
        _gossip = gossip;
        GossipNameText.text = gossip.Name();
        GossipRelevanceText.text = "- " + gossip.FreshnessString() + " (" + gossip.Freshness + " days left)";       
        Sprite factionSprite;
        _factionSymbolList.TryGetValue(gossip.Faction, out factionSprite);
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
