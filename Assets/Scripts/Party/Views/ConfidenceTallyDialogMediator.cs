using System;
using System.Collections.Generic;
using Dialog;
using UnityEngine.UI;

namespace Ambition
{
	public class ConfidenceTallyDialogMediator : DialogView, IDialog<Dictionary<string, int>>
	{
		public Text TitleText;
		public Text BodyText;

		//This is used in the beginning of the Party Screen to tally up a Player's Confidence stat
		//TODO: Implementation of this dialog is a bit of a disaster.
	    public void OnOpen(Dictionary<string, int> parameters)
	    {
	    	PartyModel pmod = AmbitionApp.GetModel<PartyModel>();
			InventoryModel imod = AmbitionApp.GetModel<InventoryModel>();
			ItemVO accessory;
			Outfit outfit = AmbitionApp.GetModel<OutfitInventoryModel>().PartyOutfit;
			string faction = pmod.Party.faction;

			imod.Equipped.TryGetValue(ItemConsts.ACCESSORY, out accessory);
//
//		        objectStorage[3] = (int)parameters["outfit"];
//		        objectStorage[4] = parameters["outfitStyle"];
//		        objectStorage[5] = parameters["accessory"];
//		        objectStorage[6] = parameters["styleMatch"];
//		        objectStorage[7] = faction.ConfidenceBonus;
//		        objectStorage[8] = generalRepReaction;
//		        objectStorage[9] = model.MaxConfidence;
//				objectStorage[10] = model.Confidence;
//			}};


	        // TODO: So much localization....
			TitleText.text = "Welcome to The Party!";
	        string line1;
	        string line2;
	        string line3;
	        string line4;
	        string line5;
	        string line6;

	        //--- Line 1 ---
	        if (accessory != null)
	        {
	            line1 = "You wore your " + outfit.Name + " and " + accessory.Name + " to the Party, hosted by the " + faction + ".";
	        } else
	        {
	            line1 = "You wore your " + outfit.Name + " to the Party, hosted by the " + faction + ".";
	        }

	        //--- Line 2 ---
	        if (faction != "Military")
	        {
				if (parameters["outfit"] < 50) //It's a very bad reaction
	            {
	                line2 = "\n\nThe moment you step out of your carriage you can hear snickers and angry whispers from the crowd. Oh no. This is not what the hosts wanted at all. (+" + parameters["outfit"] + " Max Confidence)";

	            } else if (parameters["outfit"] >= 75 && parameters["outfit"] < 100) //If it's a not great reaction
	            {
	                line2 = "\n\nFrom the way a few of the more boorish guests are glaring, you can tell that may have picked the wrong Outfit for this occassion. (+" + parameters["outfit"] + " Max Confidence)";
	            }
	            else if (parameters["outfit"] >= 85 && parameters["outfit"] < 115) //If it's a middle of the road reaction
	            {
	                line2 = "\n\nNobody seems to notice your Outfit. It hasn't really made an impression. You can live with that. (+" + parameters["outfit"] + " Max Confidence)";
	            } else if (parameters["outfit"] >= 115 && parameters["outfit"] < 150) //It it's a somewhat positive reaction
	            {
	                line2 = "\n\nAs you enter the party venue you notice some quiet nods of approval. You have done well in preparing for this party.(+" + parameters["outfit"] + " Max Confidence)";
	            } else //If it's a very positive reaction
	            {
	                line2 = "\n\nThe second partygoers spot your outfit they gasp. Their tiny exclamations of envy are like delicate music to your ears. (+" + parameters["outfit"] + " Max Confidence)";
	            }
	        } else
	        {
	            line2 = "\n\nHowever, the Military doesn't know fashion so they give you a pass. (+" + parameters["outfit"] + " Max Confidence)";
	        }
	        //--- Line 3 ---
	        //Without Accessory
	        if(accessory == null)
	        {
	            //In Style
				if (parameters["accessory"] > 0)
	            {
					line3 = "\n\nYour Outfit's in style with the latest in " + imod.CurrentStyle + " fashion! (+" + parameters["accessory"].ToString() + " Max Confidence)";
	            }
	            //Out of Style
	            else
	            {
					line3 = "\n\nOh no! Your Outfit is in the " + outfit.style + " style and it appears that " + imod.CurrentStyle + " is in vogue at the moment. (+" + parameters["accessory"].ToString() + " Max Confidence)";
	            }
	        }
	        //With Accessory
	        else
	        {
	            //Outfit is in Style, so is the Accessory
	            if (parameters["outfitStyle"] > 0 && parameters["accessory"] > 0)
	            {
	                line3 = "\n\nWhat's this? Your Outfit doesn't just match your Accessories, it's also in style with the latest in " + imod.CurrentStyle + " fashion. Incredible! (+" + (parameters["outfitStyle"] + parameters["accessory"] + parameters["styleMatch"]) + " Max Confidence)";
	            }
	            //Outfit is in Style, but the Accessory is not
	            else if (parameters["outfitStyle"] > 0 && parameters["accessory"] == 0)
	            {
	                line3 = "\n\nAh! Your Outfit is in the " + outfit.style + " style, which is in fashion. However, is appears that your Accessory is not. (+" + (parameters["outfitStyle"] + parameters["accessory"] + parameters["styleMatch"]) + " Max Confidence)";
	            } 
	            //Outfit is not in Style, but the Accessory is
	            else if (parameters["outfitStyle"] == 0 && parameters["accessory"] > 0)
	            {
	                line3 = "\n\nAh! Your Outfit is in the " + outfit.style + " style, while the " + imod.CurrentStyle + " is what's in fashion. However, your Accessory is in fashionis. Which is good, at least. (+" + (parameters["outfitStyle"] + parameters["accessory"] + parameters["styleMatch"]) + " Max Confidence)";
	            }
	            //Neither are in Style, but they Match
	            else if (parameters["outfitStyle"] == 0 && parameters["accessory"] == 0 && parameters["styleMatch"] > 0)
	            {
	                line3 = "\n\nHmm... Your Outfit and Accessory match, but they're in the " + outfit.style + " style and it appears that " + imod.CurrentStyle + " is in vogue at the moment. At least you're well coordinated. (+" + (parameters["outfitStyle"] + parameters["accessory"] + parameters["styleMatch"]) + " Max Confidence)";
	            }
	            //Neither are in Style and they don't even fucking Match, what a fucking mess
	            else
	            {
	                line3 = "\n\nMon dieu! Your Outfit is in the " + outfit.style + " style, your Accessory is in the " + (string)(accessory.States[ItemConsts.STYLE]) + " and the " + imod.CurrentStyle + " is what's in Fashion! How did this happen? (+" + (parameters["outfitStyle"] + parameters["accessory"] + parameters["styleMatch"]) + " Max Confidence)";
	            } 
	        }
	        
	        //--- Line 4 ---
	        line4 = "\n\nThe " + faction + ", of course have their opinion on you... (+" + parameters["faction"] + " Max Confidence)";

	        //--- Line 5 ---
			line5 = "\n\nSociety as a whole also has their opinions. (+" + parameters["general"] + " Max Confidence)";

	        //--- Line 6 ---

			line6 = "\n\nOverall your Maximum Confidence is at " + parameters["total"].ToString() + " and your Current Confidence is " + parameters["total"];

	        BodyText.text = line1 + line2 + line3 + line4 + line5 + line6;
	    }
	}
}

