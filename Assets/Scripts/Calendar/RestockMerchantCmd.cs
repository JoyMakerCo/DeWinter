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

			List<ItemVO> outfits = new List<ItemVO>();
			outfits.Add(new OutfitVO(model.CurrentStyle));
			outfits.Add(OutfitVO.Create());
			outfits.Add(OutfitVO.Create());
			if (AmbitionApp.GetModel<FactionModel>()["Bourgeoisie"].Level >= 3)
			{
				outfits.Add(OutfitVO.Create());
			}
			model.Market.RemoveAll(i=>i.Type == ItemConsts.OUTFIT);
			model.Market.AddRange(outfits); 
		} 
	}
}
