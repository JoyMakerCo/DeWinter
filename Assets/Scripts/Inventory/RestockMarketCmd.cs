using System;
using System.Collections.Generic;

namespace DeWinter
{
	public class RestockMarketCmd
	{
		public void Execute()
		{
			InventoryModel model = DeWinterApp.GetModel<InventoryModel>();
			Random rnd = new Random();
// TODO: More than just accessories
			List<ItemVO> itemTable = model.Inventory[ItemConsts.ACCESSORY];
			int count = itemTable.Count;
			string style;
			ItemVO item;

			model.Market.Clear();
			while (model.Market.Count < model.NumMarketSlots)
			{
				item = itemTable[rnd.Next(count)].Clone() as ItemVO;
				item.States[ItemConsts.STYLE] = style = model.Styles[rnd.Next(model.Styles.Length)];
				item.Name = style + " " + item.Name;
				model.Market.Add(item);
			}
		}
	}
}

