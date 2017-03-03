using Core;
using System.Collections.Generic;

namespace DeWinter
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
					Enemy e;
//					// TODO: Create or find the enemy in the model
//					EnemyInventory.AddEnemy(e);
					break;

				case RewardConsts.DEVOTION:
					// TODO: Implement seduction
					break;

				case RewardConsts.FACTION:
					vo = new AdjustValueVO(reward.Type, reward.Quantity);
					DeWinterApp.SendMessage<AdjustValueVO>(vo);
					break;
			}
		}

		private void RewardItem(string type, int quantity)
		{
// TODO
			InventoryModel imod = DeWinterApp.GetModel<InventoryModel>();
		}
	}
}