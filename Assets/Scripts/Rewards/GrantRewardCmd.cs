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
			switch(reward.Type)
			{
				case RewardType.Livre:
					AmbitionApp.GetModel<GameModel>().Livre += reward.Amount;
					break;

				case RewardType.Reputation:
					AmbitionApp.GetModel<GameModel>().Reputation += reward.Amount;
					break;

				case RewardType.Gossip:
					InventoryModel imod = AmbitionApp.GetModel<InventoryModel>();
					imod.GossipItems.Add(new Gossip(reward.ID));
					break;

				case RewardType.Item:
					RewardItem(reward.ID, reward.Amount);
					break;

				case RewardType.Enemy:
					AmbitionApp.SendMessage<string>(GameMessages.CREATE_ENEMY, reward.ID);
					break;

				case RewardType.Devotion:
					// TODO: Implement seduction
					break;

				case RewardType.Faction:
					AdjustFactionVO vo = new AdjustFactionVO(reward.ID, reward.Amount);
					AmbitionApp.SendMessage<AdjustFactionVO>(vo);
					break;

				case RewardType.Servant:
//					AmbitionApp.SendMessage(reward.Amount);
					break;

				case RewardType.Message:
					AmbitionApp.SendMessage(reward.ID);
					break;
			}
		}

		private void RewardItem(string type, int quantity)
		{
			InventoryModel imod = AmbitionApp.GetModel<InventoryModel>();
			if (imod.Inventory.Count < imod.NumSlots)
			{
				ItemVO[] itemz = Array.FindAll(imod.ItemDefinitions, i=>i.Type == type);
				ItemVO item = new ItemVO(itemz[new Random().Next(itemz.Length)]);
				item.Quantity = quantity;
				imod.Inventory.Add(item);
			}
		}
	}
}
