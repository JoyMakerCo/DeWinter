using System;
using Core;

namespace Ambition
{
	public class RestockMerchantCmd : ICommand<DateTime>
	{
		public void Execute(DateTime day)
		{
			InventoryModel model = DeWinterApp.GetModel<InventoryModel>();
			Random rnd = new Random();
			int count = model.ItemDefinitions.Length;
			string style;
			ItemVO item;

			model.Market.Clear();
			while (model.Market.Count < model.NumMarketSlots)
			{
				item = model.ItemDefinitions[rnd.Next(count)].Clone();
				item.States[ItemConsts.STYLE] = style = model.Styles[rnd.Next(model.Styles.Length)];
				item.Name = style + " " + item.Name;
				model.Market.Add(item);
			}

			OutfitInventory.StockInventory();
			OutfitInventory.RestockMerchantInventory();
		} 
	}
}