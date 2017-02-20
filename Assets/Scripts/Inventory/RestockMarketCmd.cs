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
			List<ItemVO> itemTable = model.Inventory.Values;
			int count = itemTable.Count;
			string style;
			ItemVO item;

			for (model.Market.Clear(); model.Market.Count < _numMarketSlots; model.Market.Add(item))
			{
				item = itemTable[rnd.Next(count)].Clone();
				item.States["Style"] = style = Styles[rnd.Next(Styles.Length)];
				item.Name = style + " " + item.Name;
			}
		}
	}
}

