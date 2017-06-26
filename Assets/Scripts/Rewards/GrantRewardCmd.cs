using Core;
using System;
using System.Collections.Generic;
using Util;

namespace Ambition
{
	public class GrantRewardCmd : ICommand<RewardVO>
	{
		public void Execute(RewardVO reward)
		{
			AdjustValueVO vo;
			switch(reward.Category)
			{
				case RewardConsts.VALUE:
					vo = new AdjustValueVO(reward.Type, reward.Quantity);
					DeWinterApp.SendMessage<AdjustValueVO>(vo);
					break;

				case RewardConsts.GOSSIP:
					InventoryModel imod = DeWinterApp.GetModel<InventoryModel>();
					imod.GossipItems.Add(new Gossip(reward.Type));
					break;

				case RewardConsts.ITEM:
					RewardItem(reward.Type, reward.Quantity);
					break;

				case RewardConsts.ENEMY:
					Enemy e = new Enemy(reward.Type);
					EnemyInventory.AddEnemy(e);
//					// TODO: Create or find the enemy in the model
					break;

				case RewardConsts.DEVOTION:
					// TODO: Implement seduction
					break;

				case RewardConsts.FACTION:
					vo = new AdjustValueVO(reward.Type, reward.Quantity);
					DeWinterApp.SendMessage<AdjustValueVO>(vo);
					break;

				case RewardConsts.MESSAGE:
					DeWinterApp.SendMessage(reward.Type);
					break;
			}
		}

		private void RewardItem(string type, int quantity)
		{
			InventoryModel imod = DeWinterApp.GetModel<InventoryModel>();
			if (imod.Inventory.Count < imod.NumSlots)
			{
				ItemVO[] itemz = Array.FindAll(imod.ItemDefinitions, i=>i.Type == type);
				ItemVO item = itemz[new Random().Next(itemz.Length)].Clone();
				item.Quantity = quantity;
				imod.Inventory.Add(item);
			}
		}
	}
}