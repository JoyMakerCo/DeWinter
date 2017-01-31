using System;
using Core;

namespace DeWinter
{
	public class DegradeOutfitCmd : ICommand<int>
	{
		public void Execute (int partyOutfitID)
		{
			//Reduce Novelty of Outfit. If Outfit has been used twice in a row then it's lowered double.
	        if (partyOutfitID != GameData.lastPartyOutfitID)
	        {
	            OutfitInventory.outfitInventories["personal"][partyOutfitID].novelty += GameData.noveltyDamage;
	            GameData.woreSameOutfitTwice = false;
	        }
	        else
	        {
	            OutfitInventory.outfitInventories["personal"][partyOutfitID].novelty += GameData.noveltyDamage * 2;
	            GameData.woreSameOutfitTwice = true;
	        }
	        //Now that the calculations are finished, the outfit now becomes the last used outfit.
	        GameData.lastPartyOutfitID = partyOutfitID;
        }
	}
}