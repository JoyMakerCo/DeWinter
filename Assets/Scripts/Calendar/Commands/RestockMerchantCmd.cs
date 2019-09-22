using System;
using System.Collections.Generic;
using Core;

namespace Ambition
{
	public class RestockMerchantCmd : ICommand<DateTime>
	{
		public void Execute(DateTime day)
		{
			//InventoryModel model = AmbitionApp.GetModel<InventoryModel>();
			//int count = model.Items.Length;
   //         FactionVO bourgeoisie = AmbitionApp.GetModel<FactionModel>()[FactionType.Bourgeoisie];
   //         ItemVO item;
   //         if (model.Market != null) model.Market.Clear();
   //         else model.Market = new Dictionary<ItemType, List<ItemVO>>();
   //         model.Market.Add(ItemType.Outfit, new List<ItemVO>());
			//for (int i= model.NumMarketSlots - model.Market.Count + (bourgeoisie.Level >= 3 ? 1 : 0); i>=0; i--)
			//{
   //             item = new ItemVO();
   //             AmbitionApp.SendMessage(InventoryMessages.GENERATE_OUTFIT, item);
			//	model.Market[ItemType.Outfit].Add(item);
			//}
		} 
	}
}
