using System;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace DeWinter
{
	public class BuyItemCmd : ICommand<ItemVO>
	{
		public void Execute (ItemVO item)
		{
			InventoryModel model = DeWinterApp.GetModel<InventoryModel>();
			AdjustBalanceVO msg = new AdjustBalanceVO("Livre", -item.Price);
			item.SellPrice = (int)((float)item.Price * model.SellbackMultiplier);
			if (!model.Inventory.ContainsKey(item.Type))
				model.Inventory.Add(item.Type, new List<ItemVO>());
			model.Inventory[item.Type].Add(item);
			model.Market.Remove(item);
			DeWinterApp.SendMessage<AdjustBalanceVO>(msg);
			Debug.Log(item.Name + " Bought for £" + msg.Amount.ToString());
		}
	}
}