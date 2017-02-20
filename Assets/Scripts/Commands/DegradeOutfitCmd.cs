using System;
using Core;

namespace DeWinter
{
	public class DegradeOutfitCmd : ICommand<int>
	{
		public void Execute (int partyOutfitID)
		{
			InventoryModel model = DeWinterApp.GetModel<InventoryModel>();
			//Reduce Novelty of Outfit. If Outfit has been used twice in a row then it's lowered double.
			model.woreSameOutfitTwice = (partyOutfitID != model.lastPartyOutfitID);
			OutfitInventory.outfitInventories["personal"][partyOutfitID].novelty += 
				(model.woreSameOutfitTwice ? model.NoveltyDamage : model.NoveltyDamage*2);
	        //Now that the calculations are finished, the outfit now becomes the last used outfit.
			model.lastPartyOutfitID = partyOutfitID;
        }
	}
}