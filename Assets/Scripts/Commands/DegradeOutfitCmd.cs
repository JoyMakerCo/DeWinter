using System;
using Core;

namespace Ambition
{
	public class DegradeOutfitCmd : ICommand
	{
		public void Execute()
		{
            InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
            ItemVO item;
            if (inventory.Equipped.TryGetValue(ItemConsts.OUTFIT, out item) && item is OutfitVO)
            {
                ((OutfitVO)item).Novelty -= inventory.NoveltyDamage;
            }
        }
	}
}
