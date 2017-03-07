using System;
using Core;

namespace DeWinter
{
	public class DegradeOutfitCmd : ICommand<Outfit>
	{
		public void Execute(Outfit o)
		{
			InventoryModel model = DeWinterApp.GetModel<InventoryModel>();
			o.novelty -= model.NoveltyDamage;
			if (o == OutfitInventory.LastPartyOutfit)
				o.novelty -= model.NoveltyDamage;

			OutfitInventory.LastPartyOutfit = o;
        }
	}
}