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
			int count = model.Inventory.Count;
			string style;
			ItemVO item;

			model.Market.Clear();
			while (model.Market.Count < model.NumMarketSlots)
			{
				item = model.Inventory[rnd.Next(count)].Clone();
				item.States[ItemConsts.STYLE] = style = model.Styles[rnd.Next(model.Styles.Length)];
				item.Name = style + " " + item.Name;
				model.Market.Add(item);
			}
		}
	}
}

