using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextPartyCounter : MonoBehaviour {

    GameObject PartyCounter;

    public Image FactionSymbol;
    public Sprite CrownSymbol;
    public Sprite ChurchSymbol;
    public Sprite MilitarySymbol;
    public Sprite BourgeoisieSymbol;
    public Sprite RevolutionSymbol;

    public GameObject Tooltip;
    //public Text TooltipText;
    public LocalizedText TooltipText;

    public Dictionary<string, Sprite> FactionSymbolList;

    private string _partyFaction;

    // Use this for initialization
    private void Awake()
    {
        SetUpDictionary();
        HideTooltip();
        _partyFaction = _checkFuturePartyFaction();
        if (_partyFaction != "")
        {
            DisplayPartyFaction(_partyFaction);
        } else
        {
            PartyCounter.SetActive(false);
        }
    }

    private void SetUpDictionary()
    {
        FactionSymbolList = new Dictionary<string, Sprite>();
        FactionSymbolList.Add("Crown", CrownSymbol);
        FactionSymbolList.Add("Church", ChurchSymbol);
        FactionSymbolList.Add("Military", MilitarySymbol);
        FactionSymbolList.Add("Bourgeoisie", BourgeoisieSymbol);
        FactionSymbolList.Add("Revolution", RevolutionSymbol);
    }

    //To Do: Make this use the stuff in CheckInvitationState.cs, whenever it is available. Does it actually exist?
    private string _checkFuturePartyFaction()
    {
        return "Church"; 
    }

    private void DisplayPartyFaction(string faction)
    {
        Sprite symbolsprite;
        FactionSymbolList.TryGetValue(faction, out symbolsprite);
        FactionSymbol.sprite = symbolsprite;
    }

    public void DisplayTooltip()
    {
        Tooltip.SetActive(true);
        TooltipText.Phrase = "party_" + _partyFaction + "_likes_and_dislikes"; //This is to reference a string in Default.json
    }

    public void HideTooltip()
    {
        Tooltip.SetActive(false);
    }
}
