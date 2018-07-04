using System;
using Core;

namespace Ambition
{
	public class ItemFactory : IFactory<string, ItemVO>
	{
		public ItemVO Create (string itemID)
		{
			InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
			ItemVO item = Array.Find(inventory.ItemDefinitions, i=>i.ID == itemID);
			if (item == null) return null;
			switch(item.Type)
			{
				case ItemConsts.OUTFIT:
					return new OutfitVO(item);
			}
			return new ItemVO(item);
		}
	}
}
