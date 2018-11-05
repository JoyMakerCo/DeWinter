using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Ambition;

public class PopUpManager : MonoBehaviour
{
// TODO: Refactor to work with Dialog Manager
    public GameObject screenFader;

    public GameObject twoPartyRSVPdModal;
	public GameObject cantAffordModal;
    public GameObject pierreQuestModal;
    public GameObject gossipSaleModal;
    public GameObject hireAndFireServantModal;
    public GameObject alterOutfitModal;
    public GameObject sewNewOutfitModal;

    public GameObject hostRemarkSlotPrefab;

    //This is used in the Wardrobe Screen and Servant Hiring Screen to tell Players they don't have enough money to afford something
    void CreateCantAffordModal(object[] objectStorage)
    {
        string objectString = (string)objectStorage[0];

        //Make the Pop Up
        GameObject popUp = Instantiate(cantAffordModal) as GameObject;
        popUp.transform.SetParent(gameObject.transform, false);
        //Title Text
        Text titleText = popUp.transform.Find("TitleText").GetComponent<Text>();
        titleText.text = "Oh No!";
        //Body Text
        Text bodyText = popUp.transform.Find("BodyText").GetComponent<Text>();
        bodyText.text = "I'm sorry Madamme, but you do not have enough Livres to afford the " + objectString + "." +
                        "\n\nYou could either sell some of your existing wardrobe, or you could borrow money from your various friends." +
                        "\n\nThis will cost you 20 Reputation but it would get you 200 Livres.";
    }

    //This is used in the Wardrobe Screen so Players can use the 'Alteration' function of the Tailor Servant
    void CreateAlterOutfitModal(object[] objectStorage)
    {
        int inventoryNumber = (int)objectStorage[0];
        OutfitVO outfit = AmbitionApp.GetModel<InventoryModel>().Inventory.FindAll(i=>i.Type == ItemConsts.OUTFIT)[inventoryNumber] as OutfitVO;

        //Make the Pop Up
        GameObject popUp = Instantiate(alterOutfitModal) as GameObject;
        popUp.transform.SetParent(gameObject.transform, false);
        AlterOufitPopUpController popUpController = popUp.GetComponent<AlterOufitPopUpController>();
        popUpController.outfit = outfit;
        //Title Text
        Text titleText = popUp.transform.Find("TitleText").GetComponent<Text>();
        titleText.text = "Alter Outfit";
        //Body Text
        Text bodyText = popUp.transform.Find("BodyText").GetComponent<Text>();
        bodyText.text = "How would you like me to alter the " + outfit.Name + "? This will cost 20 Livres."
            + "\nSelect One:";
        Slider modestyBar = popUp.transform.Find("ModestyText").Find("Slider").GetComponent<Slider>();
        modestyBar.value = outfit.Modesty;
        Slider luxuryBar = popUp.transform.Find("LuxuryText").Find("Slider").GetComponent<Slider>();
        luxuryBar.value = outfit.Luxury;
    }

    //This is used in the Estate Tab to tell Players that they were caught trading in Gossip Items
    void CreateCaughtTradingGossipModal(string faction)
    {
    	Dictionary<string,string> subs = new Dictionary<string, string>(){{"$FACTION",faction}};
    	if (AmbitionApp.GetModel<FactionModel>()["Third Estate"].Level >= 2)
    	{
    		AmbitionApp.OpenMessageDialog(DialogConsts.CAUGHT_GOSSIPING_THIRD_ESTATE_DIALOG, subs);
    	}
    	else
    	{
			AmbitionApp.OpenMessageDialog(DialogConsts.CAUGHT_GOSSIPING_DIALOG, subs);
    	}
    }

    //This is used in the Estate Tab to tell Players that Pierre has assigned a new Quest
    void CreateNewPierreQuestModal(object[] objectStorage)
    {
        PierreQuest quest = objectStorage[0] as PierreQuest;
        PierreQuestInventoryList questList = objectStorage[1] as PierreQuestInventoryList;    
        //Make the Pop Up
        GameObject popUp = Instantiate(pierreQuestModal) as GameObject;
        popUp.transform.SetParent(gameObject.transform, false);
        popUp.transform.SetAsFirstSibling();
        //Set the Quest and the QuestList
        PierreQuestModal questModal = popUp.GetComponent<PierreQuestModal>();
        questModal.quest = quest;
        questModal.questList = questList;
        //Title Text
        Text titleText = popUp.transform.Find("TitleText").GetComponent<Text>();
        titleText.text = "A Call for Gossip!";
        //Body Text
        Text bodyText = popUp.transform.Find("BodyText").GetComponent<Text>();
        bodyText.text = "Madamme, it's urgent! My finely honed journalistic senses are telling me that the public is currently crying out for Gossip concerning the " + quest.Faction + "." +
                "\n\nIf you can get that to me in " + quest.daysTimeLimit + " Days then I'll be able to get you a reward of " + quest.reward.ID + ". \n\nHow does that sound?";
    }
}