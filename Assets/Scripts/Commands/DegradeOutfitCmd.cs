using System;
using Core;

namespace DeWinter
{
	public class DegradeOutfitCmd : ICommand
	{
		public void Execute()
		{
			if (OutfitInventory.PartyOutfit != null)
			{
				InventoryModel model = DeWinterApp.GetModel<InventoryModel>();
				OutfitInventory.PartyOutfit.novelty -= model.NoveltyDamage;
				if (OutfitInventory.PartyOutfit == OutfitInventory.LastPartyOutfit)
					OutfitInventory.PartyOutfit.novelty -= model.NoveltyDamage;
			}
			OutfitInventory.LastPartyOutfit = OutfitInventory.PartyOutfit;
        }
	}
}