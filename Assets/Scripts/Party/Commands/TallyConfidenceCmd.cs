﻿using System;
using System.Collections.Generic;
using Core;

namespace Ambition
{
	public class TallyConfidenceCmd : ICommand<PartyVO>
	{
		public void Execute (PartyVO party)
		{
			PartyModel model = AmbitionApp.GetModel<PartyModel>();
	        InventoryModel imod = AmbitionApp.GetModel<InventoryModel>();
			OutfitInventoryModel omod = AmbitionApp.GetModel<OutfitInventoryModel>();
	        FactionVO faction = AmbitionApp.GetModel<FactionModel>().Factions[party.faction];
	        GameModel gmod = AmbitionApp.GetModel<GameModel>();
	        int total=0;

	        //Calculate Confidence Values Here------------
	        //Faction Outfit Likes (Military doesn't know anything, so they use the Average Value)
			Dictionary<string, int> parameters = new Dictionary<string, int>();


			//TODO: We can't configure this??
	        if (party.faction != "Military")
	        {
	            int modestyLike = faction.Modesty;
				int luxuryLike = faction.Luxury;
	            int outfitModesty = omod.PartyOutfit.modesty;
	            int outfitLuxury = omod.PartyOutfit.luxury;
	            float outfitNovelty = (float)omod.PartyOutfit.novelty * 0.01f;

	            // TODO: Fix this formula
	            parameters.Add("outfit", (int)((((400 - (Math.Abs(modestyLike - outfitModesty) + Math.Abs(luxuryLike - outfitLuxury))))*0.5f)* outfitNovelty));
	        } else {
				parameters.Add("outfit", 100);
	        }

	        //Is it in Style?
	        parameters.Add("outfitStyle",  imod.CurrentStyle == omod.PartyOutfit.style ? 30 : 0);

	        //Is the Accessory in Style and is there a Match?
	        parameters.Add("accessory", 0);
			parameters.Add("styleMatch", 0);
	        ItemVO accessory;
			if (imod.Equipped.TryGetValue(ItemConsts.ACCESSORY, out accessory)) //Has an Accessory been worn at all?
	        {
				if (imod.CurrentStyle == accessory.States["Style"] as string)
	            {
					parameters["accessory"] = 30;
	            }
	            if (omod.PartyOutfit.style == GameData.partyAccessory.States["Style"] as string)
	            {
					parameters["styleMatch"] = 30;
	            }
	        }
	        parameters.Add("faction", faction.ConfidenceBonus);
	        parameters.Add("general", gmod.ConfidenceBonus);

	        foreach(int value in parameters.Values)
	        	total += value;

	        parameters.Add("total", total);
			model.Confidence = model.StartConfidence = model.MaxConfidence = total;
			AmbitionApp.OpenDialog<Dictionary<string, int>>(DialogConsts.CONFIDENCE_TALLY, parameters);
		}
	}
}