using System;
using System.Collections.Generic;
using Core;

namespace Ambition
{
	public class RestockMerchantCmd : ICommand<DateTime>
	{
		public void Execute(DateTime day)
		{
			InventoryModel model = AmbitionApp.GetModel<InventoryModel>();
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

			List<Outfit> outfits = new List<Outfit>();
			outfits.Add(new Outfit(model.CurrentStyle));
			outfits.Add(Outfit.Create());
			outfits.Add(Outfit.Create());
			if (AmbitionApp.GetModel<FactionModel>().Factions["Bourgeoisie"].ReputationLevel >= 3)
			{
				outfits.Add(Outfit.Create());
			}
			AmbitionApp.GetModel<OutfitInventoryModel>().Merchant = outfits; 
		} 
	}
}