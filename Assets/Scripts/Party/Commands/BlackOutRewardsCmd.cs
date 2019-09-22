using System;
using System.Collections.Generic;
using Core;

namespace Ambition
{
	public class BlackOutRewardsCmd : ICommand<int>
	{
		public void Execute (int intoxication)
		{
			PartyModel model = AmbitionApp.GetModel<PartyModel>();
			if (intoxication >= model.Party.MaxIntoxication)
			{
                InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
                ItemVO item;
                //Determine Random Effect
                switch (Util.RNG.Generate(0, 10))
                {
		            case 0:
		                model.Party.Rewards.Add(new CommodityVO(CommodityType.Reputation, -Util.RNG.Generate(20, 51)));
		                break;
		            case 1:
                        model.Party.Rewards.Add(new CommodityVO(CommodityType.Reputation, model.Party.Faction.ToString(), -Util.RNG.Generate(20, 51)));
		                break;
                    // Outfit penalized
		            case 2:
                        item = inventory.GetEquippedItem(ItemType.Outfit);
                        if (item != null && item.State != null)
                        {
                            int novelty = OutfitWrapperVO.GetNovelty(item) - Util.RNG.Generate(20, 51);
                            item.State[ItemConsts.NOVELTY] = (novelty < 0 ? 0 : novelty).ToString();
                        }
		                break;
                    // Outfit Ruined
		            case 3:
                        item = inventory.GetEquippedItem(ItemType.Outfit);
                        if (item != null) AmbitionApp.SendMessage(InventoryMessages.REMOVE_ITEM, item);
		                break;
                    // Accessory Lost
		            case 4:
                        if (!inventory.Remove(inventory.GetEquippedItem(ItemType.Accessory)))
                        {
		                    model.Party.Rewards.Add(new CommodityVO(CommodityType.Livre, -Util.RNG.Generate(30, 61)));
		                }
		                break;
                    // Livre Lost
                    case 5:
						model.Party.Rewards.Add(new CommodityVO(CommodityType.Livre, -Util.RNG.Generate(30, 61)));
		                break;
		            case 8:
		            	switch (Util.RNG.Generate(0, 6))
		            	{
							case 1:
								model.Party.Rewards.Add(new CommodityVO(CommodityType.Reputation, Util.RNG.Generate(20, 51)));
			                    break;
			                case 2:
                                model.Party.Rewards.Add(new CommodityVO(CommodityType.Reputation, model.Party.Faction.ToString(), Util.RNG.Generate(20, 51)));
			                    break;
			                case 3:
								model.Party.Rewards.Add(new CommodityVO(CommodityType.Livre, Util.RNG.Generate(30, 61)));
			                    break;
			                default:
                                model.Party.Rewards.Add(new CommodityVO(CommodityType.Gossip, model.Party.Faction.ToString(), 1));
			                    break;
   			            	}
			                break;
				}
			}
		}
	}
}