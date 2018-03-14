using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Ambition
{
	public class GossipTrading : MonoBehaviour
	{
	    public GossipInventoryList gossipInventoryList;
	    public PierreQuestInventoryList questList;
	    public string tradingFor;
	    public SceneFadeInOut screenFader;
	    private Text myDescriptionText;
	    private Image myBackgroundImage;

	    void Start()
	    {
	        myDescriptionText = this.transform.Find("Text").GetComponent<Text>();
	        myBackgroundImage = this.transform.GetComponent<Image>();
	    }

	    void Update()
	    {
	        if (gossipInventoryList.selectedGossipItem != -1)
	        {
	            if (tradingFor == "Livres")
	            {
	                myDescriptionText.text = "Sell Gossip for: " + GameData.gossipInventory[gossipInventoryList.selectedGossipItem].LivreValue().ToString("£" + "#,##0") + " Livres";
	                myDescriptionText.color = Color.white;
	                myBackgroundImage.color = Color.white;
	            }
	            else if (tradingFor == "Allegiance")
	            {
	                if (GameData.gossipInventory[gossipInventoryList.selectedGossipItem].AllegianceShiftValue() > 0)
	                {
	                    myDescriptionText.text = "Push the " + GameData.gossipInventory[gossipInventoryList.selectedGossipItem].Faction + " to the Crown by " + GameData.gossipInventory[gossipInventoryList.selectedGossipItem].AllegianceShiftValue();
	                    myDescriptionText.color = Color.white;
	                    myBackgroundImage.color = Color.white;
	                }
	                else if (GameData.gossipInventory[gossipInventoryList.selectedGossipItem].AllegianceShiftValue() < 0)
	                {
	                    myDescriptionText.text = "Push the " + GameData.gossipInventory[gossipInventoryList.selectedGossipItem].Faction + " to the Third Estate by " + GameData.gossipInventory[gossipInventoryList.selectedGossipItem].AllegianceShiftValue();
	                    myDescriptionText.color = Color.white;
	                    myBackgroundImage.color = Color.white;
	                } else
	                {
	                    myDescriptionText.text = "";
	                    myDescriptionText.color = Color.clear;
	                    myBackgroundImage.color = Color.clear;
	                }
	            }
	            else if (tradingFor == "Power")
	            {
	                if (GameData.gossipInventory[gossipInventoryList.selectedGossipItem].PowerShiftValue() > 0)
	                {
	                    myDescriptionText.text = "Boost " + GameData.gossipInventory[gossipInventoryList.selectedGossipItem].Faction + " Power by " + GameData.gossipInventory[gossipInventoryList.selectedGossipItem].PowerShiftValue();
	                    myDescriptionText.color = Color.white;
	                    myBackgroundImage.color = Color.white;
	                } else
	                {
	                    myDescriptionText.text = "Undermine " + GameData.gossipInventory[gossipInventoryList.selectedGossipItem].Faction + " Power by " + GameData.gossipInventory[gossipInventoryList.selectedGossipItem].PowerShiftValue();
	                    myDescriptionText.color = Color.white;
	                    myBackgroundImage.color = Color.white;
	                }             
	            }
	        } else
	        {
	            if (tradingFor == "Livres")
	            {
	                myDescriptionText.text = "Sell Gossip for Livres";
	                myDescriptionText.color = Color.white;
	                myBackgroundImage.color = Color.white;
	            }
	            else if (tradingFor == "Allegiance")
	            {
	                myDescriptionText.text = "Leak Gossip for Faction Allegiance";
	                myDescriptionText.color = Color.white;
	                myBackgroundImage.color = Color.white;
	            }
	            else if (tradingFor == "Power")
	            {
	                myDescriptionText.text = "Propoganda for Faction Power";
	                myDescriptionText.color = Color.white;
	                myBackgroundImage.color = Color.white;
	            }
	        }    
	    }

	    public void TradeGossipPopUp()
	    {
	        object[] objectStorage = new object[6];
	        objectStorage[0] = this;
	        objectStorage[1] = GameData.gossipInventory[gossipInventoryList.selectedGossipItem];
	        objectStorage[2] = tradingFor;
	        switch (tradingFor)
	        {
	            case "Livres":
	                objectStorage[3] = GameData.gossipInventory[gossipInventoryList.selectedGossipItem].LivreValue();
	                break;
	            case "Allegiance":
	                objectStorage[3] = GameData.gossipInventory[gossipInventoryList.selectedGossipItem].AllegianceShiftValue();
	                break;
	            case "Power":
	                objectStorage[3] = GameData.gossipInventory[gossipInventoryList.selectedGossipItem].PowerShiftValue();
	                break;
	        }        
	        objectStorage[4] = GameData.gossipInventory[gossipInventoryList.selectedGossipItem].Faction;
	        objectStorage[5] = CaughtChance();
	        screenFader.gameObject.SendMessage("CreateSellGossipModal", objectStorage);
	    }

		// TODO: Make this a command
		public void SellForLivres()
	    {
	        GameData.moneyCount += GameData.gossipInventory[gossipInventoryList.selectedGossipItem].LivreValue(); //Already Includes the Freshness Formula
	        PierreQuestCheck(GameData.gossipInventory[gossipInventoryList.selectedGossipItem]);
	        GetCaughtCheck();
	        RemoveGossipItem(gossipInventoryList.selectedGossipItem);
	    }

	    // TODO: Make this a command
	    public void LeakForAllegiance()
	    {
			Gossip gossip = GameData.gossipInventory[gossipInventoryList.selectedGossipItem];
	        if (gossip.AllegianceShiftValue() != 0)
	        {
				AdjustFactionVO vo = new AdjustFactionVO(gossip.Faction, 0, 0, gossip.AllegianceShiftValue());
				AmbitionApp.SendMessage<AdjustFactionVO>(vo);
	            PierreQuestCheck(gossip);
	            GetCaughtCheck();
	            RemoveGossipItem(gossipInventoryList.selectedGossipItem);
	        }
	    }

		// TODO: Make this a command
	    public void PropagandaForPower()
	    {
			Gossip gossip = GameData.gossipInventory[gossipInventoryList.selectedGossipItem];
			AdjustFactionVO vo = new AdjustFactionVO(gossip.Faction, 0, gossip.PowerShiftValue());
			AmbitionApp.SendMessage<AdjustFactionVO>(vo);
	        PierreQuestCheck(gossip);
	        GetCaughtCheck();
	        RemoveGossipItem(gossipInventoryList.selectedGossipItem);
	    }

	    void RemoveGossipItem(int gossipNumber)
	    {
	        GameData.gossipInventory.RemoveAt(gossipInventoryList.selectedGossipItem);
	        gossipInventoryList.selectedGossipItem = -1;
	        gossipInventoryList.ClearInventoryButtons();
	        gossipInventoryList.GenerateInventoryButtons();
	    }

	    void PierreQuestCheck(Gossip g)
	    {
			List<PierreQuest> completeList = GameData.pierreQuestInventory.FindAll(p => p.GossipMatch(g));
	        foreach (PierreQuest q in completeList)
	        {
	        	AmbitionApp.SendMessage<CommodityVO>(q.reward);
				Dictionary<string,string> subs = new Dictionary<string, string>(){{"$REWARD",q.reward.ID}};
		    	AmbitionApp.OpenMessageDialog(DialogConsts.REDEEM_QUEST_DIALOG, subs);
				GameData.pierreQuestInventory.Remove(q);
	        }
	        if(completeList.Count > 0)
	        {
	            questList.selectedQuest = -1;
	        }
	        questList.ClearInventoryButtons();
	        questList.GenerateInventoryButtons();
	    }
	    
	    int CaughtChance()
	    {
	        switch (gossipInventoryList.gossipItemsSoldToday)
	        {
	            case 0:
	                return 0;
	            case 1:
	                return 7;
	            case 2:
	                return 15;
	            case 3:
	                return 25;
	            case 4:
	                return 35;
	            case 5:
	                return 45;
	            case 6:
	                return 55;
	            case 7:
	                return 65;
	            case 8:
	                return 75;
	            case 9:
	                return 85;
	            default: //Anything above 9
	                return 95;
	        }
	    }

	    void GetCaughtCheck()
	    {
	        if (Util.RNG.Generate(1,101) <= CaughtChance())
	        {
				FactionModel model = AmbitionApp.GetModel<FactionModel>();
	            //Player Rep Loss
				if (model["Third Estate"].Level >= 3)
	            {
	                GameData.reputationCount -= 15;
					model[GameData.gossipInventory[gossipInventoryList.selectedGossipItem].Faction].Reputation -= 15;
	            }
	            else
	            {
	                GameData.reputationCount -= 25;
					model[GameData.gossipInventory[gossipInventoryList.selectedGossipItem].Faction].Reputation -= 25;
	            }
	            //Angry Pop-Up About It
	            screenFader.gameObject.SendMessage("CreateCaughtTradingGossipModal", GameData.gossipInventory[gossipInventoryList.selectedGossipItem].Faction);
	        }
	        gossipInventoryList.gossipItemsSoldToday++;
	    }
	}
}