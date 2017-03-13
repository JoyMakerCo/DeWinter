using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DeWinter;

// TODO: Make this part of PartyModel
public class PartyManager : MonoBehaviour
{
    public GameObject screenFader; // It's for the Confidence Tally pop-up
    public RoomManager roomManager;
    public LevelManager levelManager;

    private PartyModel _model;
    private MapModel _mapModel;
    private Party _party;
    private FactionVO _faction;

    void Start()
	{
		_model = DeWinterApp.GetModel<PartyModel>();
		_mapModel = DeWinterApp.GetModel<MapModel>();
		_party = GameData.tonightsParty;
		_faction = GameData.factionList[_party.faction];
        ConfidenceTally();
        FashionChangeCheck(); //If the Player is of sufficiently high General Reputation, they may change the Style of Fashion just by showing up
        ConfidenceTally(); //Tally up the Player's Total Confidence
		DeWinterApp.SendMessage<Party>(MapMessage.GENERATE_MAP, _party);

        //Damage the Outfit's Novelty, how that the Confidence has already been Tallied
        _party.playerOutfit = OutfitInventory.PartyOutfit;
		DeWinterApp.SendMessage<Outfit>(InventoryConsts.DEGRADE_OUTFIT, _party.playerOutfit);

		ItemVO accessory;
		if (DeWinterApp.GetModel<InventoryModel>().Equipped.TryGetValue(ItemConsts.ACCESSORY, out accessory) && accessory.Name == "Garter Flask")
		{
			_party.maxPlayerDrinkAmount++;
		}

        //Extra Turns because of Faction Reputation Level?
		switch (_party.partySize)
		{
			case 1:
				if (_faction.ReputationLevel >= 4)
					_party.turnsLeft = _party.turns += 2;
				break;
			case 2:
				if (_faction.ReputationLevel >= 7)
					_party.turnsLeft = _party.turns += 3;
				break;
			case 3:
				if (_faction.ReputationLevel >= 9)
					_party.turnsLeft = _party.turns += 4;
				break;
		}
        TutorialCheck();
        FreeWineCheck();
    }

    void ConfidenceTally()
    {
        _party.maxPlayerConfidence = 0;
        _party.currentPlayerConfidence = 0;
        //Calculate Confidence Values Here------------
        //Faction Outfit Likes (Military doesn't know anything, so they use the Average Value)
        float outfitReaction;
        if (_party.faction != "Military")
        {
            float modestyLike = _faction.Modesty;
            float luxuryLike = _faction.Luxury;
            float outfitModesty = OutfitInventory.PartyOutfit.modesty;
            float outfitLuxury = OutfitInventory.PartyOutfit.luxury;
            float outfitNovelty = (float)OutfitInventory.PartyOutfit.novelty * 0.01f;

            Debug.Log("Outfit Novelty: " + outfitNovelty);
            //Fix this formula
            outfitReaction = Mathf.Round((((400 - (Mathf.Abs(modestyLike - outfitModesty) + Mathf.Abs(luxuryLike - outfitLuxury))))/2)* outfitNovelty);
            Debug.Log("Rounded Outfit Reaction: " + outfitReaction);
            _party.maxPlayerConfidence = (int)outfitReaction;
        } else {
            outfitReaction = 100;
            _party.maxPlayerConfidence = (int)outfitReaction;
        }
        //Is it in Style?
        int outfitStyleReaction;
        if (GameData.currentStyle == OutfitInventory.PartyOutfit.style)
        {
            outfitStyleReaction = 50;
            outfitStyleReaction = 30;
        } else
        {
            outfitStyleReaction = 0;
        }
        _party.maxPlayerConfidence += outfitStyleReaction;
        //Is the Accessory in Style and is there a Match?
        int accessoryStyleReaction = 0;
        int outfitAccessoryStyleMatch = 0;
        if (GameData.partyAccessory != null) //Has an Accessory been worn at all?
        {
			if (GameData.currentStyle == GameData.partyAccessory.States["Style"] as string)
            {
                accessoryStyleReaction = 30;
            }
            else
            {
                accessoryStyleReaction = 0;
            }
            _party.maxPlayerConfidence += accessoryStyleReaction;
            if (OutfitInventory.PartyOutfit.style == GameData.partyAccessory.States["Style"] as string)
            {
                outfitAccessoryStyleMatch = 30;
            }
            else
            {
                outfitAccessoryStyleMatch = 0;
            }
            _party.maxPlayerConfidence += outfitAccessoryStyleMatch;
        }
        //Faction Rep
        int factionReaction = _faction.ConfidenceBonus;
        GameModel gameModel = DeWinterApp.GetModel<GameModel>();
        _party.maxPlayerConfidence += factionReaction;
        //General Rep Reaction
		int generalRepReaction = gameModel.ConfidenceBonus;
        _party.maxPlayerConfidence += generalRepReaction;
        //Set Starting Confidence (Needs penalties for multiple Parties in a Row)
        _party.currentPlayerConfidence = _party.maxPlayerConfidence;
        _party.startingPlayerConfidence = _party.currentPlayerConfidence; 
        //Put Results in the Pop-Up Here
        object[] objectStorage = new object[11];
        objectStorage[0] = OutfitInventory.PartyOutfit;
        objectStorage[1] = GameData.partyAccessory;
        objectStorage[2] = _party.faction;
        objectStorage[3] = (int)outfitReaction;
        objectStorage[4] = outfitStyleReaction;
        objectStorage[5] = accessoryStyleReaction;
        objectStorage[6] = outfitAccessoryStyleMatch;
        objectStorage[7] = factionReaction;
        objectStorage[8] = generalRepReaction;
        objectStorage[9] = _party.maxPlayerConfidence;
        objectStorage[10] = _party.currentPlayerConfidence;
        screenFader.gameObject.SendMessage("CreateConfidenceTallyModal", objectStorage);
    }

    //If this Party is the Tutorial Party then it needs to give an explanatory pop-up at the Party's start
    void TutorialCheck()
    {
        if (_party.tutorial)
        {
            //Explanatory Pop Up
            screenFader.gameObject.SendMessage("CreatePartyTutorialPopUp");
        }
    }

    void FreeWineCheck()
    {
        if(_faction.ReputationLevel >= 2)
        {
            //Fill Up their Glass
            _party.currentPlayerDrinkAmount = _party.maxPlayerDrinkAmount;
            //Explanatory Pop Up
            screenFader.gameObject.SendMessage("CreateEntranceWineModal", _party);
        }
    }

    //This is currently called by the 'Leave the Party' Button in the Party Scene. May need to automate this in some fashion?
    public void FinishTheParty()
    {
        if (!_party.tutorial || (_party.tutorial && _party.host.notableLockedInState != LockedInState.Interested))
        {
            _party.turnsLeft = 0; // This makes this easier for the After Party Report to Advance Time properly
            _party.CompileRewardsAndGossip();
            //Distribute the Rewards into the Player's 'Accounts' in Game Data and the appropriate Inventories
            foreach (Reward t in _party.wonRewardsList)
            {
                switch (t.Type())
                {
                    case "Reputation":
                        GameData.reputationCount += t.amount;
                        break;
                    case "Faction Rep":
                        GameData.factionList[t.SubType()].playerReputation += t.amount;
                        break;
                    case "Introduction":
                        if (t.amount > 0)
                        {
							DeWinterApp.SendMessage<string>(ServantConsts.INTRODUCE_SERVANT, t.SubType());
                        }
                        break;
                    case "Gossip":
                        for (int i = 0; i < t.amount; i++)
                        {
                            GameData.gossipInventory.Add(new Gossip(t.SubType()));
                        }
                        break;
                    default:
                        Debug.Log("Something went wrong with issuing this Reward of " + t.amount + " " + t.Name());
                        break;
                }
            }
			OutfitInventory.PartyOutfit = null;
	        GameData.partyAccessory = null;
            levelManager.LoadLevel("Game_AfterPartyReport");
        } else
        {
            Debug.Log("Player can't leave the Tutorial Party until they've met the Host");
        }
    }

    //If the Player is of sufficiently high General Reputation, they may change the Style of Fashion just by showing up with matching Outfit and Accessories
    void FashionChangeCheck()
    {
    	GameModel gm = DeWinterApp.GetModel<GameModel>();
        //Is the Player in the wrong Style (but matching) and are they Level 8 or higher?
        if (OutfitInventory.PartyOutfit.style != GameData.currentStyle
        	&& gm.ReputationLevel >= 8
        	&& OutfitInventory.PartyOutfit.style == GameData.partyAccessory.States[ItemConsts.STYLE] as string)
        {
            //25% Chance
            if(Random.Range(0,4) == 0)
            {
                //Store the Old and New Styles before the shift
                string[] stringStorage = new string[2];
                stringStorage[0] = GameData.currentStyle;
                stringStorage[1] = OutfitInventory.PartyOutfit.style;
                //Change the Style to Match the Player's Outfit (and thus their Accessory's)
                GameData.currentStyle = OutfitInventory.PartyOutfit.style;
                //Send Out a Relevant Pop-Up
                screenFader.gameObject.SendMessage("CreateEntranceFashionChangeModal", stringStorage);
            }
        }
    }
}