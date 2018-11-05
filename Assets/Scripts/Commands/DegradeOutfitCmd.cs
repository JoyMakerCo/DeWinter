using System;
using Core;

namespace Ambition
{
	public class DegradeOutfitCmd : ICommand
	{
		public void Execute()
		{
            InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
            OutfitVO outfit = inventory.GetEquipped(ItemConsts.OUTFIT) as OutfitVO;
            if (outfit != null) outfit.Novelty -= inventory.NoveltyDamage;
        }
	}
}
