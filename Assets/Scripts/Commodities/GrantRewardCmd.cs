using Core;
using System;
using System.Collections.Generic;
using Util;

namespace Ambition
{
	public class GrantRewardCmd : ICommand<CommodityVO>
	{
		public void Execute(CommodityVO reward)
		{
            switch (reward.Type)
            {
                case CommodityType.Livre:
                    AmbitionApp.GetModel<GameModel>().Livre += reward.Amount;
                    break;

                case CommodityType.Reputation:
                    AmbitionApp.GetModel<GameModel>().Reputation += reward.Amount;
                    break;

                case CommodityType.Gossip:
                    InventoryModel imod = AmbitionApp.GetModel<InventoryModel>();
                    imod.GossipItems.Add(new Gossip(reward.ID));
                    break;

                case CommodityType.Item:
                    imod = AmbitionApp.GetModel<InventoryModel>();
                    if (imod.Inventory.Count < imod.NumSlots)
                    {
                        ItemVO[] itemz = Array.FindAll(imod.ItemDefinitions, i => i.Type == reward.ID);
                        ItemVO item = new ItemVO(itemz[Util.RNG.Generate(0, itemz.Length)])
                        {
                            Quantity = reward.Amount
                        };
                        imod.Inventory.Add(item);
                    }
                    break;

                case CommodityType.Enemy:
                    AmbitionApp.SendMessage<string>(GameMessages.CREATE_ENEMY, reward.ID);
                    break;

                case CommodityType.Devotion:
                    // TODO: Implement seduction
                    break;

                case CommodityType.Faction:
                    AdjustFactionVO vo = new AdjustFactionVO(reward.ID, reward.Amount);
                    AmbitionApp.SendMessage(vo);
                    break;

                case CommodityType.Servant:
                    //					AmbitionApp.SendMessage(reward.Amount);
                    break;

                case CommodityType.Message:
                    AmbitionApp.SendMessage(reward.ID);
                    break;
                case CommodityType.Incident:
                    AmbitionApp.SendMessage<string>(IncidentMessages.QUEUE_INCIDENT, reward.ID);
                    break;
                case CommodityType.Location:
                    AmbitionApp.SendMessage<string>(ParisMessages.ADD_LOCATION, reward.ID);
                    break;
                case CommodityType.Party:
                    PartyVO party = Array.Find(AmbitionApp.GetModel<PartyModel>().Parties, p => p.Name == reward.ID);
                    if (party != null)
                    {
                        party.RSVP = reward.Amount;
                        AmbitionApp.SendMessage(PartyMessages.RSVP, party);
                    }
                    break;
			}
		}
	}
}
