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
			PartyVO party = model.Party;
			if (intoxication >= party.MaxIntoxication)
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
                        model.Party.Rewards.Add(new CommodityVO(CommodityType.Reputation, party.Faction, -Util.RNG.Generate(20, 51)));
		                break;
                    // Outfit penalized
		            case 2:
                        item = inventory.GetEquipped(ItemConsts.OUTFIT);
                        if (item is OutfitVO)
                        {
                            ((OutfitVO)item).Novelty -= Util.RNG.Generate(20, 51);
                        }
		                break;
                    // Outfit Ruined
		            case 3:
                        item = inventory.GetEquipped(ItemConsts.OUTFIT);
                        if (item is OutfitVO)
                        {
                            model.Party.Rewards.Add(new CommodityVO(CommodityType.Item, item.Name, -1));
                        }
		                break;
                    // Accessory Lost
		            case 4:
                        item = inventory.GetEquipped(ItemConsts.ACCESSORY);
                        if (item != null)
		                {
                            model.Party.Rewards.Add(new CommodityVO(CommodityType.Item, item.Name, -1));
                        }
                        else
		                {
		                    model.Party.Rewards.Add(new CommodityVO(CommodityType.Livre, -Util.RNG.Generate(30, 61)));
		                }
		                break;
                    // Livre Lost
                    case 5:
						model.Party.Rewards.Add(new CommodityVO(CommodityType.Livre, -Util.RNG.Generate(30, 61)));
		                break;
                    // Enemy made
		            case 6:
                        model.Party.Rewards.Add(new CommodityVO(CommodityType.Enemy, party.Faction));
                        break;
                   // Forgot gossip
		            case 7:
                        model.Party.Rewards.RemoveAll(r => r.Type == CommodityType.Gossip);
						{
                            model.Party.Rewards.Add(new CommodityVO(CommodityType.Enemy, party.Faction));
                        }
                        break;
		            case 8:
		            	switch (Util.RNG.Generate(0, 6))
		            	{
							case 1:
								model.Party.Rewards.Add(new CommodityVO(CommodityType.Reputation, Util.RNG.Generate(20, 51)));
			                    break;
			                case 2:
                                model.Party.Rewards.Add(new CommodityVO(CommodityType.Reputation, party.Faction, Util.RNG.Generate(20, 51)));
			                    break;
			                case 3:
								model.Party.Rewards.Add(new CommodityVO(CommodityType.Livre, Util.RNG.Generate(30, 61)));
			                    break;
			                case 4:
								model.Party.Rewards.Add(new CommodityVO(CommodityType.Gossip, party.Faction, 1));
			                    break;
			                default:
                                EnemyVO enemy = Util.RNG.TakeRandom(party.Enemies);
                                if (enemy != null)
                                    model.Party.Rewards.Add(new CommodityVO(CommodityType.Enemy, enemy.Name, -1));
                                else
                                    model.Party.Rewards.Add(new CommodityVO(CommodityType.Gossip, party.Faction, 1));
			                    break;
   			            	}
			                break;
				}
			}
		}
	}
}