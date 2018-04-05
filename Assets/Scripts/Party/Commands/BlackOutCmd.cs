using System;
using System.Collections.Generic;
using Core;

namespace Ambition
{
	public class BlackOutCmd : ICommand<int>
	{
		public void Execute (int intoxication)
		{
			PartyModel model = AmbitionApp.GetModel<PartyModel>();
			PartyVO party = model.Party;
			if (intoxication >= party.MaxIntoxication)
			{
			        //Determine Random Effect
					GameModel gm = AmbitionApp.GetModel<GameModel>();
			        switch (Util.RNG.Generate(0, 10))
			        {
			            case 0:
			                party.blackOutEffect = "Reputation Loss";
			                party.blackOutEffectAmount = -Util.RNG.Generate(20, 51);
			                model.Party.Rewards.Add(new CommodityVO(CommodityType.Reputation, party.blackOutEffectAmount));
			                break;
			            case 1:
			                party.blackOutEffect = "Faction Reputation Loss";
			                party.blackOutEffectAmount = -Util.RNG.Generate(20, 51);
							model.Party.Rewards.Add(new CommodityVO(CommodityType.Faction, party.Faction, party.blackOutEffectAmount));
			                break;
			            case 2:
			                party.blackOutEffect = "Outfit Novelty Loss";
			                party.blackOutEffectAmount = -Util.RNG.Generate(20, 51);
							gm.Outfit.Novelty = UnityEngine.Mathf.Clamp(gm.Outfit.Novelty - party.blackOutEffectAmount, 0, 100);
			                break;
			            case 3:
			                party.blackOutEffect = "Outfit Ruined";
			                AmbitionApp.SendMessage<OutfitVO>(InventoryMessages.REMOVE_ITEM, gm.Outfit);
			                break;
			            case 4:
			                if (GameData.partyAccessory != null) //If the Player actually wore and Accessory to this Party
			                {
								party.blackOutEffect = "Accessory Ruined";
								AmbitionApp.SendMessage<ItemVO>(InventoryMessages.REMOVE_ITEM, GameData.partyAccessory);
			                }
			                else
			                {
			                    party.blackOutEffect = "Livre Lost";
			                    party.blackOutEffectAmount = -Util.RNG.Generate(30, 61);
			                    model.Party.Rewards.Add(new CommodityVO(CommodityType.Livre, party.blackOutEffectAmount));
			                }
			                break;
			            case 5:
			                party.blackOutEffect = "Livre Lost";
			                party.blackOutEffectAmount = -Util.RNG.Generate(30, 61);
							model.Party.Rewards.Add(new CommodityVO(CommodityType.Livre, party.blackOutEffectAmount));
			                break;
			            case 6:
			                party.blackOutEffect = "New Enemy";
			                AmbitionApp.SendMessage<string>(GameMessages.CREATE_ENEMY, party.Faction);
			                break;
			            case 7:
							if (model.Party.Rewards.RemoveAll(r => r.Type == CommodityType.Gossip) > 0)
							{
				                party.blackOutEffect = "Forgot All Gossip";
							}
			                else //If they have no Gossip to Lose
			                {
			                    party.blackOutEffect = "New Enemy";
								AmbitionApp.SendMessage<string>(GameMessages.CREATE_ENEMY, party.Faction);
			                }
			                break;
			            case 8:
			            	switch (Util.RNG.Generate(0, 6))
			            	{
								case 1:
				                    party.blackOutEffect = "Reputation Gain";
				                    party.blackOutEffectAmount = Util.RNG.Generate(20, 51);
									model.Party.Rewards.Add(new CommodityVO(CommodityType.Reputation, party.blackOutEffectAmount));
				                    break;
				                case 2:
				                    party.blackOutEffect = "Faction Reputation Gain";
				                    party.blackOutEffectAmount = Util.RNG.Generate(20, 51);
									model.Party.Rewards.Add(new CommodityVO(CommodityType.Faction, party.Faction, party.blackOutEffectAmount));
				                    break;
				                case 3:
				                    party.blackOutEffect = "Livre Gained";
				                    party.blackOutEffectAmount = Util.RNG.Generate(30, 61);
									model.Party.Rewards.Add(new CommodityVO(CommodityType.Livre, party.blackOutEffectAmount));
				                    break;
				                case 4:
				                    party.blackOutEffect = "New Gossip";
									model.Party.Rewards.Add(new CommodityVO(CommodityType.Gossip, party.Faction, party.blackOutEffectAmount));
				                    break;
				                default:
									if (party.Enemies != null && party.Enemies.Length > 0)
				                    {
										EnemyVO enemy = party.Enemies[Util.RNG.Generate(0, party.Enemies.Length)];
										party.blackOutEffect = enemy.Name + " no longer an Enemy";
										EnemyInventory.enemyInventory.Remove(enemy);
				                    }
				                    else
				                    {
										party.blackOutEffect = "New Gossip";
										model.Party.Rewards.Add(new CommodityVO(CommodityType.Gossip, party.Faction, party.blackOutEffectAmount));
				                    }
				                    break;
       			            	}
			                break;
				}

		        //Close Window
		        AmbitionApp.CloseDialog(DialogConsts.ROOM);

		        //Send to After Party Report Screen
		        party.blackOutEnding = true;
		        AmbitionApp.SendMessage(PartyMessages.LEAVE_PARTY);
			}
		}
	}
}