using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DeWinter;

public class TonightsPartyInfo : MonoBehaviour {

    Text tonightsPartyText;
    Text hostedByText;
    Text factionLikesText;
    Text factionDislikesText;
    Text partyDescriptionText;
    Text objective1Text;
    Text objective2Text;
    Text objective3Text;
    Text guest1Text;
    Text guest2Text;
    Text guest3Text;

    // Use this for initialization
    void Start () {
	    tonightsPartyText = this.transform.Find("Tonight'sPartyText").GetComponent<Text>();
        hostedByText = this.transform.Find("HostedByTitle").Find("HostedByText").GetComponent<Text>();
        factionLikesText = this.transform.Find("FactionLikesTitle").Find("FactionLikesText").GetComponent<Text>();
        factionDislikesText = this.transform.Find("FactionDislikesTitle").Find("FactionDislikesText").GetComponent<Text>();
        partyDescriptionText = this.transform.Find("PartyDescriptionText").GetComponent<Text>();
        objective1Text = this.transform.Find("ObjectivesTitle").Find("Objective1Text").GetComponent<Text>();
        objective2Text = this.transform.Find("ObjectivesTitle").Find("Objective2Text").GetComponent<Text>();
        objective3Text = this.transform.Find("ObjectivesTitle").Find("Objective3Text").GetComponent<Text>();
        guest1Text = this.transform.Find("NotableGuestsTitle").Find("Guest1Text").GetComponent<Text>();
        guest2Text = this.transform.Find("NotableGuestsTitle").Find("Guest2Text").GetComponent<Text>();
        guest3Text = this.transform.Find("NotableGuestsTitle").Find("Guest3Text").GetComponent<Text>();
    }
	
	// Update is called once per frame
	// TODO: Respond to a setter
	void Update () {
		if (GameData.tonightsParty != null)
		{
	        tonightsPartyText.text = GameData.tonightsParty.Name();
	        hostedByText.text = GameData.tonightsParty.faction;
			factionLikesText.text = GetLikes(GameData.factionList[GameData.tonightsParty.faction]);
			factionDislikesText.text = GetDislikes(GameData.factionList[GameData.tonightsParty.faction]);
	        partyDescriptionText.text = GameData.tonightsParty.Description();
	        objective1Text.text = GameData.tonightsParty.Objective1();
	        objective2Text.text = GameData.tonightsParty.Objective2();
	        objective3Text.text = GameData.tonightsParty.Objective3();
	        guest1Text.text = GameData.tonightsParty.Guest1();
	        guest2Text.text = GameData.tonightsParty.Guest2();
	        guest3Text.text = GameData.tonightsParty.Guest3();
	     }
    }

    private string GetLikes(FactionVO faction)
    {
		if (faction.Modesty == 0 && faction.Luxury == 0)
			return "They don't care about your clothes.";

		FactionModel fmod = DeWinterApp.GetModel<FactionModel>();
		int index = (faction.Luxury < 0 ? 0 : faction.Luxury == 0 ? 1 : 2);
		string str = (index != 1) ? fmod.Preference["Luxury"][index] : "";

		index = (faction.Modesty < 0 ? 0 : faction.Modesty == 0 ? 1 : 2);
		str += (index != 1) ? (", " + fmod.Preference["Modesty"][index]) : "";

		return str + " outfits.";
    }

	private string GetDislikes(FactionVO faction)
    {
		if (faction.Modesty == 0 && faction.Luxury == 0)
			return "They don't care about your clothes.";

		FactionModel fmod = DeWinterApp.GetModel<FactionModel>();
		int index = (faction.Luxury < 0 ? 0 : faction.Luxury == 0 ? 2 : 1);
		string str = (index != 1) ? fmod.Preference["Luxury"][index] : "";

		index = (faction.Modesty < 0 ? 0 : faction.Modesty == 0 ? 2 : 1);
		str += (index != 1) ? (", " + fmod.Preference["Modesty"][index]) : "";

		return str + " outfits.";
    }
}