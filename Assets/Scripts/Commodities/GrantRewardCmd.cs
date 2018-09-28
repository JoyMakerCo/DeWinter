using Core;
using System;

namespace Ambition
{
	public class GrantRewardCmd : ICommand<CommodityVO>
	{
		public void Execute(CommodityVO reward)
		{
            switch (reward.Type)
            {
                case CommodityType.Livre:
                    AmbitionApp.GetModel<GameModel>().Livre += reward.Value;
                    break;

                case CommodityType.Reputation:
                    AmbitionApp.GetModel<GameModel>().Reputation += reward.Value;
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
                            Quantity = reward.Value
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
                    AdjustFactionVO vo = new AdjustFactionVO(reward.ID, reward.Value);
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
                    PartyModel model = AmbitionApp.GetModel<PartyModel>();
                    PartyVO party = model.LoadParty(reward.ID);
                    if (party != null)
                    {
                        CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
                        party.RSVP = (RSVP)reward.Value;
                        party.InvitationDate = calendar.Today;

                        if (!default(DateTime).Equals(party.Date))
                            calendar.Schedule(party);

                        if (party.RSVP == RSVP.Accepted)
                            model.Party = party;
                        else
                            calendar.Schedule(party, calendar.Today);
                    }
                    break;
			}
		}
	}
}
