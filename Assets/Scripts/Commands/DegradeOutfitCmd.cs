using System;
using Core;

namespace Ambition
{
	public class DegradeOutfitCmd : ICommand<Outfit>
	{
		public void Execute(Outfit o)
		{
			InventoryModel model = AmbitionApp.GetModel<InventoryModel>();
			OutfitInventoryModel omod = AmbitionApp.GetModel<OutfitInventoryModel>();
			o.novelty -= model.NoveltyDamage;
			if (o == omod.LastPartyOutfit)
				o.novelty -= model.NoveltyDamage;

			omod.LastPartyOutfit = o;
        }
	}
}
