using System;
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
	        FactionVO faction = AmbitionApp.GetModel<FactionModel>()[party.Faction];
	        GameModel gmod = AmbitionApp.GetModel<GameModel>();
	        int total=0;

	        //Calculate Confidence Values Here------------
	        //Faction Outfit Likes (Military doesn't know anything, so they use the Average Value)
			Dictionary<string, int> parameters = new Dictionary<string, int>();
            OutfitVO outfit = imod.Equipped[ItemConsts.OUTFIT] as OutfitVO;

            //TODO: We can't configure this??
            if (party.Faction != "military")
	        {
	            int modestyLike = faction.Modesty;
				int luxuryLike = faction.Luxury;
	            int outfitModesty = outfit.Modesty;
	            int outfitLuxury = outfit.Luxury;
	            float outfitNovelty = outfit.Novelty * 0.01f;

	            // TODO: Fix this formula   
	            parameters.Add("outfit", (int)((((400 - (Math.Abs(modestyLike - outfitModesty) + Math.Abs(luxuryLike - outfitLuxury))))*0.5f)* outfitNovelty));
	        } else {
				parameters.Add("outfit", 100);
	        }

	        //Is it in Style?
	        parameters.Add("outfitStyle",  imod.CurrentStyle == outfit.Style ? 30 : 0);

	        //Is the Accessory in Style and is there a Match?
	        parameters.Add("accessory", 0);
			parameters.Add("styleMatch", 0);
	        ItemVO accessory;
			if (imod.Equipped.TryGetValue(ItemConsts.ACCESSORY, out accessory)) //Has an Accessory been worn at all?
	        {
				if (imod.CurrentStyle == accessory.State["Style"] as string)
	            {
					parameters["accessory"] = 30;
	            }
                if (outfit.Style == accessory.State["Style"] as string)
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