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
			int count = model.ItemDefinitions.Length;
			ItemVO item;
			string style=null;

			model.Market.Clear();
			while (model.Market.Count < model.NumMarketSlots)
			{
				item = new ItemVO(model.ItemDefinitions[UnityEngine.Random.Range(0,count)]);
				item.State[ItemConsts.STYLE] = style = model.Styles[UnityEngine.Random.Range(0,model.Styles.Length)];
				item.Name = style + " " + item.Name;
				model.Market.Add(item);
			}

			OutfitVO outfit = OutfitVO.Create();
			outfit.Style = style;
			outfit.GenerateName();

			List<ItemVO> outfits = new List<ItemVO>() { outfit, OutfitVO.Create(), OutfitVO.Create() };
			if (AmbitionApp.GetModel<FactionModel>()["bourgeoisie"].Level >= 3)
			{
				outfits.Add(OutfitVO.Create());
			}
			model.Market.RemoveAll(i=>i.Type == ItemConsts.OUTFIT);
			model.Market.AddRange(outfits); 
		} 
	}
}
