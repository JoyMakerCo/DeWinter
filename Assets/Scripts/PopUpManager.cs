using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DeWinter;

public class PopUpManager : MonoBehaviour
{
// TODO: Refactor to work with Dialog Manager
    public GameObject screenFader;
    public LevelManager levelManager;

	public GameObject newGameModal;
    public GameObject quitGameModal;
    public GameObject messageModal;
    public GameObject workTheRoomTutorialModal;
    public GameObject twoPartyChoiceModal;
    public GameObject twoPartyRSVPdModal;
    public GameObject rSVPModal;
    public GameObject pierreQuestModal;
    public GameObject gossipSaleModal;
    public GameObject cancellationModal;
    public GameObject buyOrSellModal;
    public GameObject hireAndFireServantModal;
    public GameObject cantAffordModal;
    public GameObject confidenceTallyModal;
    public GameObject roomChoiceModal;
    public GameObject workTheRoomModal;
    public GameObject workTheHostModal;
    public GameObject hostRemarkModal;
    public GameObject ambushModal;
    public GameObject noOutfitModal;
    public GameObject alterOutfitModal;
    public GameObject sewNewOutfitModal;

    public GameObject hostRemarkSlotPrefab;

    //This is used at the very beginning when the Player is starting a new Game
    public void CreateNewGamePopUp()
    {
        //Make the Pop Up
        GameObject popUp = Instantiate(newGameModal) as GameObject;
        popUp.transform.SetParent(gameObject.transform, false);
    }

    //This is how Players Quit the Game
    void CreateQuitGamePopUp()
    {
        //Make the Pop Up
        GameObject popUp = Instantiate(quitGameModal) as GameObject;
        popUp.transform.SetParent(gameObject.transform, false);
        //Modal Background Shift
    }

    //This is used at the beginning of the Tutorial Party to teach you how Parties work and navigating them
    void CreatePartyTutorialPopUp()
    {
        //Make the Pop Up
        GameObject popUp = Instantiate(messageModal) as GameObject;
        popUp.transform.SetParent(gameObject.transform, false);
        popUp.transform.SetAsFirstSibling();
        //Title Text
        Text titleText = popUp.transform.Find("TitleText").GetComponent<Text>();
        titleText.text = "Welcome!";
        //Body Text
        Text bodyText = popUp.transform.Find("BodyText").GetComponent<Text>();
        bodyText.text = "Bonjour Madamme and Welcome to the Orphan's Feast!"
            + "\nYou're alone? Don't worry! You'll find some of our other patrons in the next room." 
            + "\n\nIn fact, we even have a real big shot here tonight. He's getting drinks in the back. You should talk to him!"
            + "\n\n<You're in the Vestibule, click on a Room next to you to go to that Room>";
    }

    //This is used at the beginning of the first Tutorial Conversation to teach you how Work the Room... works
    void CreateWorkTheRoomTutorialPopUp(WorkTheRoomManager wRM)
    {
        //Make the Pop Up
        GameObject popUp = Instantiate(workTheRoomTutorialModal) as GameObject;
        popUp.transform.SetParent(gameObject.transform, false);
        //Set the Pop Up's Variable
        WorkTheRoomPopUpModalController popUpController = popUp.transform.GetComponent<WorkTheRoomPopUpModalController>();
        popUpController.workTheRoomManager = wRM;
        //Title Text
        Text titleText = popUp.transform.Find("TitleText").GetComponent<Text>();
        titleText.text = "Charming the Room";
        //Body Text
        Text bodyText = popUp.transform.Find("BodyText").GetComponent<Text>();
        bodyText.text = "There are some Guests in front of you. Say something to them!"
            + "\nHow? Click on your Remarks at the bottom of the screen, then click on the Guests to use the Remarks."
            + "\nTry to match the color of your Remarks to the color of the Guests. If they like what you're about to say they'll turn green. If they don't like it they'll turn red."
            + "\nOnce their Opinion Bar is all the way to the right then they'll be Charmed. Charm them both to finish the Conversation. Just don't make them too angry or you'll Put them Off and maybe run out of Confidence.";
    }

    //This is how the Players know that Styles have gone in and out of fashion
    void CreateFashionPopUp(object[] stringStorage)
    { 
        string oldStyle = stringStorage[0] as string;
        string newStyle = stringStorage[1] as string;
        //Make the Pop Up
        GameObject popUp = Instantiate(messageModal) as GameObject;
        popUp.transform.SetParent(gameObject.transform, false);
        popUp.transform.SetAsFirstSibling();
        //Title Text
        Text titleText = popUp.transform.Find("TitleText").GetComponent<Text>();
        titleText.text = "Style Change!";
        //Body Text
        Text bodyText = popUp.transform.Find("BodyText").GetComponent<Text>();
        bodyText.text = "The " + oldStyle + " Style is now out of fashion and " +
            "\nthe " + newStyle + " Style is now in fashion!" +
            "\nAdjust your wardrobe accordingly!";
    }

    //This tells the Players that it's Pay Day for all their Servants
    void CreatePayDayPopUp()
    {
        //Set Up the Information
        int totalWages = 0;
        string servantsHired = "";
        ServantModel model = DeWinterApp.GetModel<ServantModel>();
		GameModel gameModel = DeWinterApp.GetModel<GameModel>();
		ServantVO servant;
        foreach (string slot in model.Servants.Keys)
        {
			if (model.Hired.TryGetValue(slot, out servant))
        	{
	            totalWages += servant.Wage;
				servantsHired += servant.NameAndTitle;
	        }
        }
        //Make the Pop up
        GameObject popUp = Instantiate(messageModal) as GameObject;
        popUp.transform.SetParent(gameObject.transform, false);
        popUp.transform.SetAsFirstSibling();
        //Title Text
        Text titleText = popUp.transform.Find("TitleText").GetComponent<Text>();
        titleText.text = "It's Payday";
        //Body Text (Update with more Servants)
        Text bodyText = popUp.transform.Find("BodyText").GetComponent<Text>();
        bodyText.text = "It's time to pay the help. You're currently employing " + servantsHired + "." +
            "\nTheir cost of employment this week is " + totalWages + "." +
			"\nThis leaves you with £" + gameModel.Livre.ToString("#,##0");
    }

    //This confirmation modal is used for Hiring and Firing Servants
    void CreateHireAndFireModal(ServantVO s)
    {
        //Make the Pop up
        GameObject popUp = Instantiate(buyOrSellModal) as GameObject;
        popUp.transform.SetParent(gameObject.transform, false);
        Text bodyText = popUp.transform.Find("BodyText").GetComponent<Text>();
        Text titleText = popUp.transform.Find("TitleText").GetComponent<Text>();
        Text hireFireButtonText = popUp.transform.Find("HireFireButton").Find("Text").GetComponent<Text>();
        Text dontHireFireButtonText = popUp.transform.Find("DontHireFireButton").Find("Text").GetComponent<Text>();

        //Set the Pop Up Values
        //Fill in the Text
        if (s.Hired)
        {
            titleText.text = "Fire + " + s.Name + "?";
            bodyText.text = "Are you sure you want fire " + s.NameAndTitle + "?";
            hireFireButtonText.text = "Fire";
            dontHireFireButtonText.text = "Don't Fire";
        }
        else
        {
			titleText.text = "Hire + " + s.Name + "?";
            bodyText.text = "Are you sure you want to hire " + s.NameAndTitle + " for £" + s.Wage.ToString() + "?";
            hireFireButtonText.text = "Hire";
            dontHireFireButtonText.text = "Don't Hire";
        }
    }

    //This is a modal used to explain why the Player can't actually Fire Camille, the Handmaiden
    void CreateCamilleFiringModal()
    {
        //Make the Pop up
        GameObject popUp = Instantiate(messageModal) as GameObject;
        popUp.transform.SetParent(gameObject.transform, false);
        popUp.transform.SetAsFirstSibling();
        //Title Text
        Text titleText = popUp.transform.Find("TitleText").GetComponent<Text>();
        titleText.text = "Wait a minute...";
        //Body Text (Update with more Servants)
        Text bodyText = popUp.transform.Find("BodyText").GetComponent<Text>();
        bodyText.text = "“Camille, what exactly do I pay you for?”" +
                        "\n“Oh Madamme, why do you ask?”" +
                        "\n“No reason, just thinking about... budget stuff.”" +
                        "\n“Well Madamme, I clean your house, do your laundry, cook your meals, sweep your chimney, manage the household, mend your clothes -”" +
                        "\n“Fine! Fine! Forget I asked!”";
    }

    void CreatePartyInvitationPopUp(Party party)
    {
            //Make the Pop up
            GameObject popUp = Instantiate(messageModal) as GameObject;
            popUp.transform.SetParent(gameObject.transform, false);
            popUp.transform.SetAsFirstSibling();
            //Title Text
            Text titleText = popUp.transform.Find("TitleText").GetComponent<Text>();
            titleText.text = "An Invitation Has Arrived!";
            //Body Text
            Text bodyText = popUp.transform.Find("BodyText").GetComponent<Text>();
            bodyText.text = "Madamme, an invitation from " + party.host.Name + " of the " + party.faction + " has arrived!" +
                "\n\nIt says you've been invited to attend their " + party.SizeString() + " Party." +
                "\n\nI've added the event to your Calendar.";
    }

    //This is how Players RSVP to parties
    void CreateRSVPPopUp(Party affectedParty)
    {
        //Make the Pop up
        GameObject popUp = Instantiate(rSVPModal) as GameObject;
        popUp.transform.SetParent(gameObject.transform, false);
        RSVPPopUpController popUpController = popUp.GetComponent<RSVPPopUpController>();
        ServantModel model = DeWinterApp.GetModel<ServantModel>();
        popUpController.party = affectedParty;
        //Title Text
        Text titleText = popUp.transform.Find("TitleText").GetComponent<Text>();
        titleText.text = "RSVP";
        //Body Text (Update with Spymaster)
        Text bodyText = popUp.transform.Find("BodyText").GetComponent<Text>();
        bodyText.text = "You've been invited to a " + affectedParty.SizeString() + " Party being held by " + affectedParty.host.Name + " of the " + affectedParty.faction + ".";
        if (model.Servants.ContainsKey("Spymaster")) {
            if(affectedParty.enemyList.Count > 0)
            {
                bodyText.text += "\nIt appears that some of your enemies will be in attendance:";
                foreach (Enemy e in affectedParty.enemyList){
                    bodyText.text += "\n-" + e.Name;
                }
                bodyText.text += "\nWould you still like to attend?";
            } else
            {
                bodyText.text += "\nIt appears that none of your Enemies will be in attendance. Magnifique!" +
                                 "\nWould you like to attend?";
            }

        } else {
            bodyText.text += "\nWould you like to attend?";
        }
    }

    void CreateTwoPartyChoicePopUp(Party party1, Party party2, bool isToday)
    {
        //Make the Pop up
        GameObject popUp = Instantiate(twoPartyChoiceModal) as GameObject;
        popUp.transform.SetParent(gameObject.transform, false);
        //Title Text
        Text titleText = popUp.transform.Find("TitleText").GetComponent<Text>();
        titleText.text = "Which Party?";
        //Body Text
        Text bodyText = popUp.transform.Find("BodyText").GetComponent<Text>();
        bodyText.text = "There are two parties being held on this day." + 
                "\nWhich party are you interested in?";
        //Party Assignment
        TwoPartyChoicePopUpController popUpController = popUp.GetComponent<TwoPartyChoicePopUpController>();
		popUpController.isToday = isToday;
        popUpController.screenFader = screenFader;
    }

    //This is how Players cancel their RSVP to Parties
    void CreateCancellationPopUp(object[] objectStorage)
    {
        Party affectedParty = objectStorage[0] as Party;
        bool today = (bool)(objectStorage[1]);
        //Make the Pop up
        GameObject popUp = Instantiate(cancellationModal) as GameObject;
        popUp.transform.SetParent(gameObject.transform, false);
        RSVPPopUpController popUpController = popUp.GetComponent<RSVPPopUpController>();
        popUpController.party = affectedParty;
        popUpController.today = today;
        //Title Text
        Text titleText = popUp.transform.Find("TitleText").GetComponent<Text>();
        titleText.text = "Cancel Your RSVP?";
        //Body Text
        if (today)
        {
            Text bodyText = popUp.transform.Find("BodyText").GetComponent<Text>();
            bodyText.text = "The " + affectedParty.SizeString() + " Party being held by " + affectedParty.host.Name + " of the " + affectedParty.faction + " is tonight!" +
                "\nCancelling the day of will seriously harm your reputation." +
                "\nAre you sure you want to cancel last minute like this? ";
        } else
        {
            Text bodyText = popUp.transform.Find("BodyText").GetComponent<Text>();
            bodyText.text = "You've already agreed to attend the " + affectedParty.SizeString() + " Party being held by " + affectedParty.host.Name + " of the " + affectedParty.faction + "." +
                "\nCancelling will harm your reputation." +
                "\nWould you like cancel?";
        }
    }

    //This tells Players they can't attend a Party if they don't have any Outfits
    void CreateNoOutfitModal()
    {
        //Make the Pop Up
        GameObject popUp = Instantiate(noOutfitModal) as GameObject;
        popUp.transform.SetParent(gameObject.transform, false);
        //Title Text
        Text titleText = popUp.transform.Find("TitleText").GetComponent<Text>();
        titleText.text = "You Have Nothing to Wear!";
        //Body Text
        Text bodyText = popUp.transform.Find("BodyText").GetComponent<Text>();
        bodyText.text = "Madamme, it's not a figure of speech." +
            "\nYou literally have nothing to wear to this party. If we check in with the Merchant she might have something, but otherwise you may have to cancel.";
    }

    //This is used in the Wardrobe Screen to confirm buying or selling Items
    void CreateBuyOrSellModal(object[] objectStorage)
    {
        //Info Is Parsed Out Here
        string inventoryType = objectStorage[0] as string;
        string itemType = objectStorage[1] as string;
        int itemPrice;

        //Make the Pop up
        GameObject popUp = Instantiate(buyOrSellModal) as GameObject;
        popUp.transform.SetParent(gameObject.transform, false);
        Text bodyText = popUp.transform.Find("BodyText").GetComponent<Text>();
        Text titleText = popUp.transform.Find("TitleText").GetComponent<Text>();

        //Set the Pop Up Values
        BuyAndSellPopUpController controller = popUp.GetComponent<BuyAndSellPopUpController>();
        controller.inventoryType = inventoryType;
        controller.itemType = itemType;
		controller.outfit = objectStorage[2] as Outfit;
		controller.accessory = objectStorage[2] as ItemVO;

        //Fill in the Text
		if (controller.outfit != null)
        {
            if (inventoryType == "personal")
            {
                titleText.text = "Sell This?";
				itemPrice = controller.outfit.OutfitPrice(inventoryType); //Items are at Half Price from the Player Inventory to the Merchant
				bodyText.text = "Are you sure you want to sell this " + controller.outfit.Name();
			}
            else
            {
                titleText.text = "Buy This?";
				itemPrice = controller.outfit.OutfitPrice(inventoryType);
				bodyText.text = "Are you sure you want to buy this " + controller.outfit.Name() + " for " + itemPrice.ToString("£" + "#,##0") + "?";
            }
        }
		else if (controller.accessory != null)
        {
            if (inventoryType == "personal")
            {
                titleText.text = "Sell This?";
				itemPrice = controller.accessory.SellPrice; //Items are at Half Price from the Player Inventory to the Merchant
				bodyText.text = "Are you sure you want to sell this " + controller.accessory.Name + " for " + itemPrice.ToString("£" + "#,##0") + "?";
            }
            else
            {
                titleText.text = "Buy This?";
				itemPrice = controller.accessory.Price;
				bodyText.text = "Are you sure you want to buy this " + controller.accessory.Name + " for " + itemPrice.ToString("£" + "#,##0") + "?";
            }
        }
    }

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

    void CreateOutOfMoneyModal()
    {
        //Make the Pop Up
        GameObject popUp = Instantiate(cantAffordModal) as GameObject;
        popUp.transform.SetParent(gameObject.transform, false);
        //Title Text
        Text titleText = popUp.transform.Find("TitleText").GetComponent<Text>();
        titleText.text = "Oh No!";
        //Body Text
        Text bodyText = popUp.transform.Find("BodyText").GetComponent<Text>();
        if(GameData.reputationCount > 20)
        {
            bodyText.text = "I'm sorry Madamme, you appear to be completely out of money." +
                "\n\nIt looks like you'll have to borrow money from your various friends just to stay solvent." +
                "\n\nThis will cost you 20 Reputation but it will also get get you 200 Livres.";
        } else
        {
            bodyText.text = "I'm sorry Madamme, you appear to be completely out of money." +
                "\n\nUnfortunatley, it appears that you don't have enough Reputation to actually get the loans you'd need to continue." +
                "\n\nI'm sorry Madamme, but I think this might be the end.";
            Text buttonText = popUp.transform.Find("BorrowMoneyButton").Find("Text").GetComponent<Text>();
            buttonText.text = "Well... Damn";
        }

        //Make one of the buttons disappear
        Button dismissButtonButton = popUp.transform.Find("DismissButton").GetComponent<Button>();
        Image dismissButtonImage = popUp.transform.Find("DismissButton").GetComponent<Image>();
        Text dismissButtonText = popUp.transform.Find("DismissButton").Find("Text").GetComponent<Text>();
        dismissButtonButton.interactable = false;
        dismissButtonImage.color = Color.clear;
        dismissButtonText.color = Color.clear;
    }

    //This is used in the Wardrobe Screen to tell Players they don't have enough spare room in their Wardrobe for more Outfits purchased from the Merchant
    void CreateCantFitModal(object[] objectStorage)
    {
        int merchantInventoryNumber = (int)objectStorage[0];

        //Make the Pop Up
        GameObject popUp = Instantiate(messageModal) as GameObject;
        popUp.transform.SetParent(gameObject.transform, false);
        //Title Text
        Text titleText = popUp.transform.Find("TitleText").GetComponent<Text>();
        titleText.text = "Oh No!";
        //Body Text
        Text bodyText = popUp.transform.Find("BodyText").GetComponent<Text>();
        bodyText.text = "I'm sorry Madamme, but you do not have enough space in your wardobe to fit this " + OutfitInventory.outfitInventories["merchant"][merchantInventoryNumber].Name() + ". " +
            "\nAt this time your Wardrobe can only hold " + OutfitInventory.personalInventoryMaxSize + " Outfits. Perhaps there is some way to expand your closet space?";
    }

    //This is used in the Wardrobe Screen to tell Players they don't have enough spare room in their Wardrobe for more Outfits created by the Seamstress
    void CreateCantFitNewOutfitModal()
    {
        //Make the Pop Up
        GameObject popUp = Instantiate(messageModal) as GameObject;
        popUp.transform.SetParent(gameObject.transform, false);
        //Title Text
        Text titleText = popUp.transform.Find("TitleText").GetComponent<Text>();
        titleText.text = "Oh No!";
        //Body Text
        Text bodyText = popUp.transform.Find("BodyText").GetComponent<Text>();
        bodyText.text = "I'm sorry Madamme, but you do not have enough space in your wardobe to fit a new Outfit. I can't create anything when you have no room to store it." +
            "\nAt this time your Wardrobe can only hold " + OutfitInventory.personalInventoryMaxSize + " Outfits. Perhaps there is some way to expand your closet space?";
    }

    //This is used in the Wardrobe Screen so Players can use the 'Alteration' function of the Tailor Servant
    void CreateAlterOutfitModal(object[] objectStorage)
    {
        int inventoryNumber = (int)objectStorage[0];
        Outfit outfit = OutfitInventory.outfitInventories["personal"][inventoryNumber];

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
        bodyText.text = "How would you like me to alter the " + outfit.Name() + "? This will cost 20 Livres."
            + "\nSelect One:";
        Slider modestyBar = popUp.transform.Find("ModestyText").Find("Slider").GetComponent<Slider>();
        modestyBar.value = outfit.modesty;
        Slider luxuryBar = popUp.transform.Find("LuxuryText").Find("Slider").GetComponent<Slider>();
        luxuryBar.value = outfit.luxury;
    }

    //This is used in the Wardrobe Screen so Players can use the 'Sew New Outfit' function of the Seamstress Servant
    void CreateSewNewOutfitModal(object[] objectStorage)
    {
        OutfitInventoryList personalOutfitInventoryList = objectStorage[0] as OutfitInventoryList;
        //Make the Pop Up
        GameObject popUp = Instantiate(sewNewOutfitModal) as GameObject;
        popUp.transform.SetParent(gameObject.transform, false);
        SewNewOutfitPopUpController popUpController = popUp.GetComponent<SewNewOutfitPopUpController>();
        popUpController.personalInventoryList = personalOutfitInventoryList;
        //Title Text
        Text titleText = popUp.transform.Find("TitleText").GetComponent<Text>();
        titleText.text = "New Outfit";
        //Body Text
        Text bodyText = popUp.transform.Find("BodyText").GetComponent<Text>();
        bodyText.text = "What would you like me to create?";
    }

    //This is used at the beginning of a Party, if the Player is of such a high General Reputation that they change the Style of Fashion just by showing up with matching Outfit and Accessories
    void CreateEntranceFashionChangeModal(string[] stringStorage)
    {
        string oldStyle = stringStorage[0] as string;
        string newStyle = stringStorage[1] as string;
        //Make the Pop Up
        GameObject popUp = Instantiate(messageModal) as GameObject;
        popUp.transform.SetParent(gameObject.transform, false);
        popUp.transform.SetAsFirstSibling();
        //Title Text
        Text titleText = popUp.transform.Find("TitleText").GetComponent<Text>();
        titleText.text = "Style Change!";
        //Body Text
        Text bodyText = popUp.transform.Find("BodyText").GetComponent<Text>();
        bodyText.text = "You hear audible gasps as you exit your carriage. Your expertly coordinated Outfit, combined with your significant social stature, seems to have taken everyone aback. You can see the wheels turning in their minds as they rethink their stylings and choice of attire." +
            "\nYou have started a new fashion trend. The " + oldStyle + " Style is now out of fashion and " +
            "\nthe " + newStyle + " Style is now in fashion!";
    }

    //This is used in the beginning of the Party Screen to tally up a Player's Confidence stat
    void CreateConfidenceTallyModal(object[] objectStorage)
    {
        Outfit outfit = objectStorage[0] as Outfit;
        ItemVO accessory = objectStorage[1] as ItemVO;
        string partyFaction = objectStorage[2].ToString();
        int outfitReaction = (int)objectStorage[3];
        int outfitStyleReaction = (int)objectStorage[4];
        int accessoryStyleReaction = (int)objectStorage[5];
        int outfitAccessoryStyleMatch = (int)objectStorage[6];
        int factionReaction = (int)objectStorage[7];
        int generalRepReaction = (int)objectStorage[8];
        int maxConfidence = (int)objectStorage[9];
        int currentConfidence = (int)objectStorage[10];

        //Make the Pop Up
        GameObject popUp = Instantiate(confidenceTallyModal) as GameObject;
        popUp.transform.SetParent(gameObject.transform, false);
        //Title Text
        Text titleText = popUp.transform.Find("TitleText").GetComponent<Text>();
        titleText.text = "Welcome to The Party!";
        //Body Text
        Text bodyText = popUp.transform.Find("BodyText").GetComponent<Text>();
        string line1;
        string line2;
        string line3;
        string line4;
        string line5;
        string line6;
        //--- Line 1 ---
        if (accessory != null)
        {
            line1 = "You wore your " + outfit.Name() + " and " + accessory.Name + " to the Party, hosted by the " + partyFaction + ".";
        } else
        {
            line1 = "You wore your " + outfit.Name() + " to the Party, hosted by the " + partyFaction + ".";
        }
        //--- Line 2 ---
        if (partyFaction != "Military")
        {
            if (outfitReaction < 50) //It's a very bad reaction
            {
                line2 = "\n\nThe moment you step out of your carriage you can hear snickers and angry whispers from the crowd. Oh no. This is not what the hosts wanted at all. (+" + outfitReaction + " Max Confidence)";

            } else if (outfitReaction >= 75 && outfitReaction < 100) //If it's a not great reaction
            {
                line2 = "\n\nFrom the way a few of the more boorish guests are glaring, you can tell that may have picked the wrong Outfit for this occassion. (+" + outfitReaction + " Max Confidence)";
            }
            else if (outfitReaction >= 85 && outfitReaction < 115) //If it's a middle of the road reaction
            {
                line2 = "\n\nNobody seems to notice your Outfit. It hasn't really made an impression. You can live with that. (+" + outfitReaction + " Max Confidence)";
            } else if (outfitReaction >= 115 && outfitReaction < 150) //It it's a somewhat positive reaction
            {
                line2 = "\n\nAs you enter the party venue you notice some quiet nods of approval. You have done well in preparing for this party.(+" + outfitReaction + " Max Confidence)";
            } else //If it's a very positive reaction
            {
                line2 = "\n\nThe second partygoers spot your outfit they gasp. Their tiny exclamations of envy are like delicate music to your ears. (+" + outfitReaction + " Max Confidence)";
            }
        } else
        {
            line2 = "\n\nHowever, the Military doesn't know fashion so they give you a pass. (+" + outfitReaction + " Max Confidence)";
        }
        //--- Line 3 ---
        //Without Accessory
        if(accessory == null)
        {
            //In Style
            if (outfitStyleReaction > 0)
            {
                line3 = "\n\nYour Outfit's in style with the latest in " + GameData.currentStyle + " fashion! (+" + outfitStyleReaction + " Max Confidence)";
            }
            //Out of Style
            else
            {
                line3 = "\n\nOh no! Your Outfit is in the " + outfit.style + " style and it appears that " + GameData.currentStyle + " is in vogue at the moment. (+" + outfitStyleReaction + " Max Confidence)";
            }
        }
        //With Accessory
        else
        {
            //Outfit is in Style, so is the Accessory
            if (outfitStyleReaction > 0 && accessoryStyleReaction > 0)
            {
                line3 = "\n\nWhat's this? Your Outfit doesn't just match your Accessories, it's also in style with the latest in " + GameData.currentStyle + " fashion. Incredible! (+" + (outfitStyleReaction + accessoryStyleReaction + outfitAccessoryStyleMatch) + " Max Confidence)";
            }
            //Outfit is in Style, but the Accessory is not
            else if (outfitStyleReaction > 0 && accessoryStyleReaction == 0)
            {
                line3 = "\n\nAh! Your Outfit is in the " + outfit.style + " style, which is in fashion. However, is appears that your Accessory is not. (+" + (outfitStyleReaction + accessoryStyleReaction + outfitAccessoryStyleMatch) + " Max Confidence)";
            } 
            //Outfit is not in Style, but the Accessory is
            else if (outfitStyleReaction == 0 && accessoryStyleReaction > 0)
            {
                line3 = "\n\nAh! Your Outfit is in the " + outfit.style + " style, while the " + GameData.currentStyle + " is what's in fashion. However, your Accessory is in fashionis. Which is good, at least. (+" + (outfitStyleReaction + accessoryStyleReaction + outfitAccessoryStyleMatch) + " Max Confidence)";
            }
            //Neither are in Style, but they Match
            else if (outfitStyleReaction == 0 && accessoryStyleReaction == 0 && outfitAccessoryStyleMatch > 0)
            {
                line3 = "\n\nHmm... Your Outfit and Accessory match, but they're in the " + outfit.style + " style and it appears that " + GameData.currentStyle + " is in vogue at the moment. At least you're well coordinated. (+" + (outfitStyleReaction + accessoryStyleReaction + outfitAccessoryStyleMatch) + " Max Confidence)";
            }
            //Neither are in Style and they don't even fucking Match, what a fucking mess
            else
            {
                line3 = "\n\nMon dieu! Your Outfit is in the " + outfit.style + " style, your Accessory is in the " + (string)(accessory.States[ItemConsts.STYLE]) + " and the " + GameData.currentStyle + " is what's in Fashion! How did this happen? (+" + (outfitStyleReaction + accessoryStyleReaction + outfitAccessoryStyleMatch) + " Max Confidence)";
            } 
        }
        
        //--- Line 4 ---
        line4 = "\n\nThe " + partyFaction + ", of course have their opinion on you... (+" + factionReaction + " Max Confidence)";
        //--- Line 5 ---
        line5 = "\n\nSociety as a whole also has their opinions. (+" + generalRepReaction + " Max Confidence)";
        //--- Line 6 ---
        line6 = "\n\nOverall your Maximum Confidence is at " + maxConfidence + " and your Current Confidence is " + currentConfidence;
        bodyText.text = line1 + line2 + line3 + line4 + line5 + line6;
    }

    //This appears at the beginning of a Party when the Player has enough Reputation with the Faction that they get their wine glass instantly filled
    void CreateEntranceWineModal(Party p)
    {
        //Make the Pop Up
        GameObject popUp = Instantiate(messageModal) as GameObject;
        popUp.transform.SetParent(gameObject.transform, false);
        popUp.transform.SetAsFirstSibling();
        //Title Text
        Text titleText = popUp.transform.Find("TitleText").GetComponent<Text>();
        titleText.text = "For you, Madamme";
        //Body Text
        Text bodyText = popUp.transform.Find("BodyText").GetComponent<Text>();
        bodyText.text = "Madamme, thank you for joining us tonight. " + p.host.Name + " asked me to make sure you were taken care of when the Party began." +
                        "\n<Your Wineglass is now full>";
    }

    //This randomly appears when the Player enters an uncleared Room during a Party and they Player has enough Reputation with the Faction that they get their wine glass filled
    void CreateRandomWineModal(Party p)
    {
        //Make the Pop Up
        GameObject popUp = Instantiate(messageModal) as GameObject;
        popUp.transform.SetParent(gameObject.transform, false);
        popUp.transform.SetAsFirstSibling();
        //Title Text
        Text titleText = popUp.transform.Find("TitleText").GetComponent<Text>();
        titleText.text = "Let's Top That Up";
        //Body Text
        Text bodyText = popUp.transform.Find("BodyText").GetComponent<Text>();
        bodyText.text = "Madamme, I noticed you were running a little low on refreshment. As per " + p.host.Name + "'s request, I'll check in occassional to make sure you have enough to drink." +
                        "\n<Your Wineglass is now full>";
    }

    //This is used in the Party Scene for Players to choose whether they wish to engage in conversation (Work the Room) or try to avoid everyone (Move Through)
    void CreateRoomChoiceModal(int[] intStorage)
    {
    	MapModel model = DeWinterApp.GetModel<MapModel>();

        int xPos = intStorage[0];
        int yPos = intStorage[1];

		RoomVO room = model.Map.Rooms[xPos, yPos];

        //Make the Pop Up
        GameObject popUp = Instantiate(roomChoiceModal) as GameObject;
        popUp.transform.SetParent(gameObject.transform, false);
        //Title Text
        Text titleText = popUp.transform.Find("TitleText").GetComponent<Text>();
        titleText.text = GameData.tonightsParty.Name();
        //Body Text and Buttons
        Text bodyText = popUp.transform.Find("BodyText").GetComponent<Text>();
        
        Text moveThroughText = popUp.transform.Find("MoveThroughButton").Find("Text").GetComponent<Text>();
        Image moveThroughButtonImage = popUp.transform.Find("MoveThroughButton").GetComponent<Image>();

        if (!model.Room.HostHere) //If the Host isn't here
        {
            if(!model.Room.IsImpassible)
            {
                moveThroughButtonImage.color = Color.white;
                moveThroughText.color = Color.white;
                int moveThroughChance = room.MoveThroughChance;
                //Is the Player using the Cane Accessory? If so then increase the chance to Move Through by 10%!
                if (GameData.partyAccessory != null)
                {
					if (GameData.partyAccessory.Type == "Cane")
                    {
                        moveThroughChance += 10;
                    }
                }
                moveThroughText.text = "Move Through (" + moveThroughChance.ToString() + "%)";
                bodyText.text = "You've entered the " + room.Name +
                            "\n\nWould you like to 'Work the Room' and engage the party goers in Conversation, or would you like to 'Move Through' and hope nobody notices you?";
            } else //If the Player has entered a Room where they are not allowed to Move Through
            {
                moveThroughButtonImage.color = Color.clear;
                moveThroughText.color = Color.clear;
                bodyText.text = "You've entered the " + room.Name +
                            "\n\nClick the button below to 'Work the Room' and engage the party goers in Conversation.";
            }
        } else // If the Host Is there
        {
            moveThroughButtonImage.color = Color.clear;
            moveThroughText.color = Color.clear;
            bodyText.text = "You've entered the " + room.Name +
                       "\n\nPrepare to 'Work the Room' and engage the Host in Conversation. They may be alone but they'll be far more demanding than a regular Guest.";
        }
        Text workTheRoomText = popUp.transform.Find("WorkTheRoomButton").Find("Text").GetComponent<Text>();
        Image workTheRoomImage = popUp.transform.Find("WorkTheRoomButton").GetComponent<Image>();
        if (!room.Cleared)
        {
            workTheRoomImage.color = Color.white;
            workTheRoomText.color = Color.white;
            workTheRoomText.text = "Work the Room";
        }
        else
        {
            workTheRoomImage.color = Color.clear;
            workTheRoomText.color = Color.clear;
        }
    }

    //This is used in the Party Scene to brings up the Conversation/Work the Room Window where the Player combats Guests with their charms
    void CreateWorkTheRoomModal(object[] objectStorage)
    {
        RoomVO room = objectStorage[0] as RoomVO;
        bool isAmbush = (bool)objectStorage[1];
        RoomManager roomManager = objectStorage[2] as RoomManager;
        //Make the Pop Up
        GameObject popUp = Instantiate(workTheRoomModal) as GameObject;
        popUp.transform.SetParent(gameObject.transform, false);
        //Set the room and the Work The Room Manager should handle the rest.
        WorkTheRoomManager workManager = popUp.GetComponent<WorkTheRoomManager>();
        workManager.room = room;
        workManager.isAmbush = isAmbush;
        workManager.roomManager = roomManager;

        Debug.Log("Made Work the Room. Ambush is " + isAmbush);
    }

    //This is used in the Party Scene to brings up the Conversation/Work the Host Modal where the Player combats the Host with their charms
    void CreateWorkTheHostModal(object[] objectStorage)
    {
        RoomVO room = objectStorage[0] as RoomVO;
        RoomManager roomManager = objectStorage[1] as RoomManager; ;
        //Make the Pop Up
        GameObject popUp = Instantiate(workTheHostModal) as GameObject;
        popUp.transform.SetParent(gameObject.transform, false);
        //Set the room and the Work The Room Manager should handle the rest.
        WorkTheHostManager workManager = popUp.GetComponent<WorkTheHostManager>();
        workManager.room = room;
        workManager.roomManager = roomManager;

        Debug.Log("Made Work the Host");
    }

    void CreateHostRemarkModal(object[] objectStorage)
    {
        WorkTheHostManager workManager = objectStorage[1] as WorkTheHostManager;
        int numberOfTargetSlots = (int)objectStorage[2];
        //Make the Pop Up
        GameObject popUp = Instantiate(hostRemarkModal) as GameObject;
        popUp.transform.SetParent(workManager.gameObject.transform, false);
        workManager.hostRemarkWindow = popUp;
        //Set the Timer
        workManager.hostRemarkTimer = 6.0f;
        workManager.hostRemarkCountdownBar = popUp.transform.FindChild("CountdownBar").GetComponent<Scrollbar>();
        workManager.StockFireBackRemarkSlotList(numberOfTargetSlots);
        //Positioning (Fire Back Remark Slot Set Up) ------------------
        int buttonwidth = (int)hostRemarkSlotPrefab.GetComponent<RectTransform>().rect.width;
        int padding = 50; // Space between Target Slots  
        int offsetFromCenterX = (numberOfTargetSlots * buttonwidth) / 2;
        //Make the Fire Back Remark Slot Buttons ----------------------
        //Clear out the old list
        workManager.hostRemarkSlotButtonList.Clear();
        //Make the Buttons themselves
        for (int i = 0; i < numberOfTargetSlots; i++)
        {
            //Parenting
            GameObject remarkSlotButton = Instantiate(hostRemarkSlotPrefab, hostRemarkSlotPrefab.transform.position, hostRemarkSlotPrefab.transform.rotation) as GameObject;
            //Find Parent Object
            GameObject remarkSlotHolder = popUp.transform.FindChild("RemarkSlotHolder").gameObject;
            remarkSlotButton.transform.SetParent(remarkSlotHolder.transform, false);
            FireBackRemarkSlotButton fireBackButton = remarkSlotButton.GetComponent<FireBackRemarkSlotButton>();
            //Positioning (Actual)
            remarkSlotButton.transform.localPosition = new Vector3((i * (buttonwidth + padding) - offsetFromCenterX), 0, 0);
            //Set the Fire Back Remark that this button represents
            fireBackButton.workManager = workManager;
            fireBackButton.myHostRemarkSlot = workManager.hostRemarkSlotList[i];
            workManager.hostRemarkSlotButtonList.Add(fireBackButton);
            fireBackButton.slot = i;
        }
        workManager.hostRemarkActive = true;
        Debug.Log("Made Fire Back Remark Modal");
    }

    //This Message Modal variation is used in the Party Scene to tell the Player they succesfully Moved Through a Room in a Party
    void CreateMovedThroughModal(string[] stringStorage)
    {
        string roomName = stringStorage[0] as string;
        //Make the Pop Up
        GameObject popUp = Instantiate(messageModal) as GameObject;
        popUp.transform.SetParent(gameObject.transform, false);
        //Title Text
        Text titleText = popUp.transform.Find("TitleText").GetComponent<Text>();
        titleText.text = "Moved Through the " + roomName + "!";
        //Body Text
        Text bodyText = popUp.transform.Find("BodyText").GetComponent<Text>();
        bodyText.text = "You've succesfully moved through the " + roomName + " unnoticed." +
            "\nNow you can back to your real mission uninterrupted.";
    }

    //This Window is used to tell Players that they've failed to Move Through a Room and have been Ambushed
    void CreateAmbushedModal(string[] stringStorage)
    {
        string roomName = stringStorage[0] as string;
        //Make the Pop Up
        GameObject popUp = Instantiate(ambushModal) as GameObject;
        popUp.transform.SetParent(gameObject.transform, false);
        //Title Text
        Text titleText = popUp.transform.Find("TitleText").GetComponent<Text>();
        titleText.text = "Ambushed!";
        //Body Text
        Text bodyText = popUp.transform.Find("BodyText").GetComponent<Text>();
        bodyText.text = "You've been Ambushed in the " + roomName + " !" +
            "\nPrepare for Conversation!";
    }

    //This Window is used are Work the Room or Ambush events to tell Players what they've one in conversation combat
    void WorkTheRoomReportModal(object[] objectStorage)
    {
        int guestsCharmed = (int)objectStorage[0];
        int guestsPutOff = (int)objectStorage[1];
        bool hostHere = (bool)objectStorage[2];
        
        //Make the Pop Up
        GameObject popUp = Instantiate(messageModal) as GameObject;
        popUp.transform.SetParent(gameObject.transform, false);
        //Title Text
        Text titleText = popUp.transform.Find("TitleText").GetComponent<Text>();
        titleText.text = "Conversation Over!";
        //Body Text
        Text bodyText = popUp.transform.Find("BodyText").GetComponent<Text>();
        if (!hostHere)
        {
            bodyText.text = "The Conversation is over, you managed to Charm " + guestsCharmed + " Guests and " + guestsPutOff + " Guests felt Put Off after speaking with you.";
        } else
        {
            if(guestsCharmed == 1)
            {
                bodyText.text = "The Conversation is over, and you managed to Charm the Host!";
            } else
            {
                bodyText.text = "The Conversation is over, but the Host was unfortunately Put Off by your behavior.";
            }
            
        }
        Reward givenReward = objectStorage[3] as Reward;
        bodyText.text += "\n\nYour Reward for Clearing this Room is " + givenReward.Name();
    }

    //This Message Modal variation is used in the Party Scene to tell the Player they ran out of Confidence during the Work the Room or Host sequence and have embarassed themselves
    void CreateFailedConfidenceModal(object[] objectStorage)
    {
        Party party = objectStorage[0] as Party;
        string faction = party.faction;
        int reputationLoss = (int)objectStorage[1];
        int factionReputationLoss = (int)objectStorage[2];
        //Make the Pop Up
        GameObject popUp = Instantiate(messageModal) as GameObject;
        popUp.transform.SetParent(gameObject.transform, false);
        //Title Text
        Text titleText = popUp.transform.Find("TitleText").GetComponent<Text>();
        titleText.text = "Oh No!";
        //Body Text
        Text bodyText = popUp.transform.Find("BodyText").GetComponent<Text>();
        if (!party.tutorial) //If this is not the Tutorial Party (aka: a regular party)
        {
            bodyText.text = "It appears that you ran out of Confidence during that Conversation. You just started stammering before suddenly leaving to 'Get Some Air'." +
            "\n\nYou've spent an hour here in the Vestibule collecting your wits but your sudden disappearance was considered quite rude." +
            "\n\nYou've lost " + factionReputationLoss + " Repuation with the " + faction + " and " + reputationLoss + " Reputation with society in general.";
        } else //If this is the Tutorial Party, which is a lot more forgiving
        {
            bodyText.text = "It appears that you ran out of Confidence during that Conversation. You just started stammering before suddenly leaving to 'Get Some Air'." +
                        "\n\nYou've spent a few moments here in the Vestibule collecting your wits but your sudden disappearance was considered quite rude." +
                        "\n\nFortunately, the patrons at the Orphan's Feast are much more forgiving than the rest of the society. Give it another try!";
        }
    }

    void CreateMissedPartyRSVPModal(Party party)
    {
        //Make the Pop Up
        GameObject popUp = Instantiate(messageModal) as GameObject;
        popUp.transform.SetParent(gameObject.transform, false);
        popUp.transform.SetAsFirstSibling();
        //Title Text
        Text titleText = popUp.transform.Find("TitleText").GetComponent<Text>();
        titleText.text = "Missed RSVP!";
        //Body Text
        Text bodyText = popUp.transform.Find("BodyText").GetComponent<Text>();
        bodyText.text = "You didn't RSVP to the " + party.Name() + "." +
            "\nThe Host is rather upset with you and you've lost Reputation";
    }

    //This is used in the Estate Tab to tell Players that they were caught trading in Gossip Items
    void CreateCaughtTradingGossipModal(string faction)
    {
        //Make the Pop Up
        GameObject popUp = Instantiate(messageModal) as GameObject;
        popUp.transform.SetParent(gameObject.transform, false);
        //Title Text
        Text titleText = popUp.transform.Find("TitleText").GetComponent<Text>();
        titleText.text = "Merde!";
        //Body Text
        Text bodyText = popUp.transform.Find("BodyText").GetComponent<Text>();
        bodyText.text = "Madamme, it appears that you've been found out. While 'Le Mecure' does its best to conceal our sources, some members of the " + faction + 
                " seem to have figured out that you were out supplier. This has damaged your Reputation both with them and with society in General.";
        if (GameData.factionList["Third Estate"].ReputationLevel >= 2)
        {
            bodyText.text += "\n\nThankfully your contacts in the Third Estate have minimized the effects somewhat.";
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
                "\n\nIf you can get that to me in " + quest.daysTimeLimit + " Days then I'll be able to get you a reward of " + quest.reward.Name() + ". \n\nHow does that sound?";
    }

    //This is used in the Estate Tab to confirm selling various bits of Gossip
    void CreateSellGossipModal(object[] objectStorage)
    {
        GossipTrading gossipTrader = objectStorage[0] as GossipTrading;
        Gossip gossip = objectStorage[1] as Gossip;
        string tradeFor = objectStorage[2] as string;
        int tradeForAmount = (int)objectStorage[3];
        string faction = objectStorage[4] as string;
        int caughtChance = (int)objectStorage[5];
        //Make the Pop Up
        GameObject popUp = Instantiate(gossipSaleModal) as GameObject;
        popUp.transform.SetParent(gameObject.transform, false);
        //Set the Quest and the QuestList
        GossipTradingPopUpController popUpController = popUp.GetComponent<GossipTradingPopUpController>();
        popUpController.gossipTrading = gossipTrader;
        popUpController.tradeFor = tradeFor;
        //Title Text
        Text titleText = popUp.transform.Find("TitleText").GetComponent<Text>();
        titleText.text = "Are You Sure?";
        //Body Text
        Text bodyText = popUp.transform.Find("BodyText").GetComponent<Text>();
        bodyText.text = "Madamme,  are you sue that you'd like to sell me this " + gossip.Name();
        switch (tradeFor)
        {
            case "Livres":
                bodyText.text += " in exchange for " + tradeForAmount + " Livres?";
                break;
            case "Allegiance":
                if (tradeForAmount > 0)
                {
                    bodyText.text += " in exchange for " + tradeForAmount + " Allegiance to the Crown from the " + faction + "?";
                } else
                {
                    bodyText.text += " in exchange for " + tradeForAmount + " Allegiance to the Third Estate from the " + faction + "?";
                }
                break;
            case "Power":
                bodyText.text += " in exchange for " + tradeForAmount + " Power for the " + faction + "?";
                break;
        }
        if (caughtChance == 0)
        {
            bodyText.text += "\n\n I can easily conceal you as my source. There's no chance you'll be caught.";
        } else
        {
            bodyText.text += "\n\n The more you leak me Gossip in a single day, the harder it is for me to conceal my sources. Today, I'd guess there is currently a " + caughtChance + "% chance of you being caught. Can you acccept that risk?";
        }
    }

    //This is used in the Estate Tab to tell Players that Pierre has Redeemed their Quest
    void CreatePierreQuestReemedModal(PierreQuest quest)
    {
        //Make the Pop Up
        GameObject popUp = Instantiate(messageModal) as GameObject;
        popUp.transform.SetParent(gameObject.transform, false);
        //Title Text
        Text titleText = popUp.transform.Find("TitleText").GetComponent<Text>();
        titleText.text = "Gossip? C'est Magnifique!";
        //Body Text
        Text bodyText = popUp.transform.Find("BodyText").GetComponent<Text>();
        bodyText.text = "Thank you Madamme, this is exactly the kind of sallacious trash our esteemed readers require!" +
                "\nWe'll take care of your reward of " + quest.reward.Name() + " immediately.";
    }
}