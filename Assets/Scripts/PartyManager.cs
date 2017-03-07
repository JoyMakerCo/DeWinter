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

    private PartyModel _model;
    private MapModel _mapModel;

    void Start()
	{
		_model = DeWinterApp.GetModel<PartyModel>();
		_mapModel = DeWinterApp.GetModel<MapModel>();
        ConfidenceTally();
        EngageParty();
        FashionChangeCheck(); //If the Player is of sufficiently high General Reputation, they may change the Style of Fashion just by showing up
        ConfidenceTally(); //Tally up the Player's Total Confidence
		DeWinterApp.SendMessage<Party>(PartyConstants.GENERATE_MAP, GameData.tonightsParty);

		EnemyCheck(); //Place all the Enemies in the Party
        EngageParty(); //Start the Party
    }

    //Checks all the relevant Enemies and sees if they're going to attend the Party
    void EnemyCheck()
    {
		_model.Party.enemyList = new List<Enemy>();
		foreach (RoomVO room in _mapModel.Map.Rooms)
		{
// TODO: Enemies populated to Party FIRST, then added to map upon generation
			_model.Party.enemyList.AddRange(room.Enemies);
		}
    }

    void EngageParty()
    {
        //Instantiate a the room Holder Parent Object
		DeWinterApp.SendMessage<Party>(PartyConstants.GENERATE_MAP, GameData.tonightsParty);

        //Damage the Outfit's Novelty, how that the Confidence has already been Tallied
        GameData.tonightsParty.playerOutfit = OutfitInventory.PartyOutfit;
		DeWinterApp.SendMessage<Outfit>(InventoryConsts.DEGRADE_OUTFIT, GameData.tonightsParty.playerOutfit);

		ItemVO accessory;
		if (DeWinterApp.GetModel<InventoryModel>().Equipped.TryGetValue(ItemConsts.ACCESSORY, out accessory) && accessory.Name == "Garter Flask")
		{
			GameData.tonightsParty.maxPlayerDrinkAmount++;
		}

        //Extra Turns because of Faction Reputation Level?
        if (GameData.tonightsParty.partySize == 1 && GameData.factionList[GameData.tonightsParty.faction].ReputationLevel >= 4)
        {
            GameData.tonightsParty.turns += 2;
            GameData.tonightsParty.turnsLeft = GameData.tonightsParty.turns;
        }
        else if (GameData.tonightsParty.partySize == 2 && GameData.factionList[GameData.tonightsParty.faction].ReputationLevel >= 7)
        {
            GameData.tonightsParty.turns += 3;
            GameData.tonightsParty.turnsLeft = GameData.tonightsParty.turns;
        }
        else if (GameData.tonightsParty.partySize == 3 && GameData.factionList[GameData.tonightsParty.faction].ReputationLevel >= 9)
        {
            GameData.tonightsParty.turns += 4;
            GameData.tonightsParty.turnsLeft = GameData.tonightsParty.turns;
        }
        FreeWineCheck();
    }

    void ConfidenceTally()
    {
        GameData.tonightsParty.maxPlayerConfidence = 0;
        GameData.tonightsParty.currentPlayerConfidence = 0;
        //Calculate Confidence Values Here------------
        //Faction Outfit Likes (Military doesn't know anything, so they use the Average Value)
        float outfitReaction;
        if (GameData.tonightsParty.faction != "Military")
        {
            float modestyLike = GameData.factionList[GameData.tonightsParty.faction].Modesty;
            float luxuryLike = GameData.factionList[GameData.tonightsParty.faction].Luxury;
            float outfitModesty = OutfitInventory.PartyOutfit.modesty;
            float outfitLuxury = OutfitInventory.PartyOutfit.luxury;
            float outfitNovelty = (float)OutfitInventory.PartyOutfit.novelty * 0.01f;
            Debug.Log("Outfit Novelty: " + outfitNovelty);
            //Fix this formula
            outfitReaction = Mathf.Round((((400 - (Mathf.Abs(modestyLike - outfitModesty) + Mathf.Abs(luxuryLike - outfitLuxury))))/2)* outfitNovelty);
            Debug.Log("Rounded Outfit Reaction: " + outfitReaction);
            GameData.tonightsParty.maxPlayerConfidence = (int)outfitReaction;
        } else {
            outfitReaction = 100;
            GameData.tonightsParty.maxPlayerConfidence = (int)outfitReaction;
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
        GameData.tonightsParty.maxPlayerConfidence += outfitStyleReaction;
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
            GameData.tonightsParty.maxPlayerConfidence += accessoryStyleReaction;
            if (OutfitInventory.PartyOutfit.style == GameData.partyAccessory.States["Style"] as string)
            {
                outfitAccessoryStyleMatch = 30;
            }
            else
            {
                outfitAccessoryStyleMatch = 0;
            }
            GameData.tonightsParty.maxPlayerConfidence += outfitAccessoryStyleMatch;
        }
        //Faction Rep
        int factionReaction = GameData.factionList[GameData.tonightsParty.faction].ConfidenceBonus;
        GameModel gameModel = DeWinterApp.GetModel<GameModel>();
        GameData.tonightsParty.maxPlayerConfidence += factionReaction;
        //General Rep Reaction
		int generalRepReaction = gameModel.ConfidenceBonus;
        GameData.tonightsParty.maxPlayerConfidence += generalRepReaction;
        //Set Starting Confidence (Needs penalties for multiple Parties in a Row)
        GameData.tonightsParty.currentPlayerConfidence = GameData.tonightsParty.maxPlayerConfidence;
        GameData.tonightsParty.startingPlayerConfidence = GameData.tonightsParty.currentPlayerConfidence; 
        //Put Results in the Pop-Up Here
        object[] objectStorage = new object[11];
        objectStorage[0] = OutfitInventory.PartyOutfit;
        objectStorage[1] = GameData.partyAccessory;
        objectStorage[2] = GameData.tonightsParty.faction;
        objectStorage[3] = (int)outfitReaction;
        objectStorage[4] = outfitStyleReaction;
        objectStorage[5] = accessoryStyleReaction;
        objectStorage[6] = outfitAccessoryStyleMatch;
        objectStorage[7] = factionReaction;
        objectStorage[8] = generalRepReaction;
        objectStorage[9] = GameData.tonightsParty.maxPlayerConfidence;
        objectStorage[10] = GameData.tonightsParty.currentPlayerConfidence;
        screenFader.gameObject.SendMessage("CreateConfidenceTallyModal", objectStorage);
    }

    void FreeWineCheck()
    {
        if(GameData.factionList[GameData.tonightsParty.faction].ReputationLevel >= 2)
        {
            //Fill Up their Glass
            GameData.tonightsParty.currentPlayerDrinkAmount = GameData.tonightsParty.maxPlayerDrinkAmount;
            //Explanatory Pop Up
            screenFader.gameObject.SendMessage("CreateEntranceWineModal", GameData.tonightsParty);
        }
    }

    //This is currently called by the 'Leave the Party' Button in the Party Scene. May need to automate this in some fashion?
    public void FinishTheParty()
    {
        GameData.tonightsParty.turnsLeft = 0; // This makes this easier for the After Party Report to Advance Time properly
        GameData.tonightsParty.CompileRewardsAndGossip();
        //Distribute the Rewards into the Player's 'Accounts' in Game Data and the appropriate Inventories
        foreach (Reward t in GameData.tonightsParty.wonRewardsList)
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
                    if(t.amount > 0)
                    {
						DeWinterApp.SendMessage<string>(ServantConsts.INTRODUCE_SERVANT, t.SubType());
                    }
                    break;
                case "Gossip":
                    if (t.amount > 0)
                    {
                        for(int i = 0; i < t.amount; i++)
                        {
                            GameData.gossipInventory.Add(new Gossip(t.SubType()));
                        }
                    }
                    break;
                default:
                    Debug.Log("Something went wrong with issuing this Reward of " + t.amount + " " + t.Name());
                    break;
            }
        }
        OutfitInventory.PartyOutfit = null;
        GameData.partyAccessory = null;
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