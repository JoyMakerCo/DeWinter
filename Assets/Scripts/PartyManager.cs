using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

// TODO: Make this part of PartyModel
namespace DeWinter
{
	public class PartyManager : MonoBehaviour
	{
	    public GameObject screenFader; // It's for the Confidence Tally pop-up
	    public RoomManager roomManager;

		private FactionVO _faction;

	    void Start()
		{
			_faction = GameData.factionList[GameData.tonightsParty.faction];
	        FashionChangeCheck(); //If the Player is of sufficiently high General Reputation, they may change the Style of Fashion just by showing up
	        ConfidenceTally(); //Tally up the Player's Total Confidence

	        //Damage the Outfit's Novelty, how that the Confidence has already been Tallied
			DeWinterApp.SendMessage<Outfit>(InventoryConsts.DEGRADE_OUTFIT, OutfitInventory.PartyOutfit);

			ItemVO accessory;
			InventoryModel imod = DeWinterApp.GetModel<InventoryModel>();
			if (imod.Equipped.TryGetValue(ItemConsts.ACCESSORY, out accessory) && accessory.Name == "Garter Flask")
			{
				GameData.tonightsParty.maxPlayerDrinkAmount++;
			}

	        //Extra Turns because of Faction Reputation Level?
			switch (GameData.tonightsParty.partySize)
			{
				case 1:
					if (_faction.ReputationLevel >= 4)
						GameData.tonightsParty.turnsLeft = GameData.tonightsParty.turns += 2;
					break;
				case 2:
					if (_faction.ReputationLevel >= 7)
						GameData.tonightsParty.turnsLeft = GameData.tonightsParty.turns += 3;
					break;
				case 3:
					if (_faction.ReputationLevel >= 9)
						GameData.tonightsParty.turnsLeft = GameData.tonightsParty.turns += 4;
					break;
			}
	        TutorialCheck();
	        FreeWineCheck();
	    }

	    void ConfidenceTally()
	    {
	        GameData.tonightsParty.maxPlayerConfidence = 0;
	        GameData.tonightsParty.currentPlayerConfidence = 0;
	        InventoryModel imod = DeWinterApp.GetModel<InventoryModel>();
	        //Calculate Confidence Values Here------------
	        //Faction Outfit Likes (Military doesn't know anything, so they use the Average Value)
	        float outfitReaction;
	        if (GameData.tonightsParty.faction != "Military")
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
	            GameData.tonightsParty.maxPlayerConfidence = (int)outfitReaction;
	        } else {
	            outfitReaction = 100;
	            GameData.tonightsParty.maxPlayerConfidence = (int)outfitReaction;
	        }
	        //Is it in Style?
	        int outfitStyleReaction;
	        if (imod.CurrentStyle == OutfitInventory.PartyOutfit.style)
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
				if (imod.CurrentStyle == GameData.partyAccessory.States["Style"] as string)
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
	        int factionReaction = _faction.ConfidenceBonus;
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

	    //If this Party is the Tutorial Party then it needs to give an explanatory pop-up at the Party's start
	    void TutorialCheck()
	    {
	        if (GameData.tonightsParty.tutorial)
	        {
	        	DeWinterApp.OpenMessageDialog(DialogConsts.PARTY_TUTORIAL_DIALOG);
	        }
	    }

	    void FreeWineCheck()
	    {
	        if(_faction.ReputationLevel >= 2)
	        {
	            //Fill Up their Glass
	            GameData.tonightsParty.currentPlayerDrinkAmount = GameData.tonightsParty.maxPlayerDrinkAmount;
	            //Explanatory Pop Up
	            Dictionary<string, string> substitutions = new Dictionary<string, string>()
					{{"$HOSTNAME",GameData.tonightsParty.host.Name}};
	            DeWinterApp.OpenMessageDialog(DialogConsts.REPUTATION_WINE_DIALOG, substitutions);
	        }
	    }

	    //This is currently called by the 'Leave the Party' Button in the Party Scene. May need to automate this in some fashion?
	    public void FinishTheParty()
	    {
	        if (!GameData.tonightsParty.tutorial || (GameData.tonightsParty.tutorial && GameData.tonightsParty.host.notableLockedInState != LockedInState.Interested))
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
				DeWinterApp.SendMessage<string>(GameMessages.LOAD_SCENE, "Game_AfterPartyReport");
	        } else
	        {
	            Debug.Log("Player can't leave the Tutorial Party until they've met the Host");
	        }
	    }

	    //If the Player is of sufficiently high General Reputation, they may change the Style of Fashion just by showing up with matching Outfit and Accessories
	    void FashionChangeCheck()
	    {
	    	GameModel gm = DeWinterApp.GetModel<GameModel>();
			InventoryModel imod = DeWinterApp.GetModel<InventoryModel>();
	        //Is the Player in the wrong Style (but matching) and are they Level 8 or higher?
	        if (OutfitInventory.PartyOutfit.style != imod.CurrentStyle
	        	&& gm.ReputationLevel >= 8
	        	&& OutfitInventory.PartyOutfit.style == GameData.partyAccessory.States[ItemConsts.STYLE] as string)
	        {
	            //25% Chance
	            if(Random.Range(0,4) == 0)
	            {
	                //Send Out a Relevant Pop-Up
	                Dictionary<string, string> substitutions = new Dictionary<string, string>(){
						{"$OLDSTYLE",imod.CurrentStyle},
						{"$NEWSTYLE",OutfitInventory.PartyOutfit.style}};
					DeWinterApp.OpenMessageDialog(DialogConsts.SET_TREND_DIALOG, substitutions);
	            }
	        }
	    }
	}
}