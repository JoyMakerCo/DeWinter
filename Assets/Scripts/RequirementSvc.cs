using System;
using Core;

namespace Ambition
{
    public class RequirementSvc : IAppService
    {
        public bool CheckRequirements(CommodityVO[] requirments)
        {
            GameModel model = AmbitionApp.GetModel<GameModel>();
            foreach(CommodityVO req in requirments)
            {
                switch(req.Type)
                {
                    case CommodityType.Confidence:
                        if (AmbitionApp.GetModel<PartyModel>().Confidence < req.Value)
                            return false;
                        break;
                    case CommodityType.Faction:
                        FactionModel factions = AmbitionApp.GetModel<FactionModel>();
                        FactionVO faction;
                        factions.Factions.TryGetValue(req.ID, out faction);
                        if (faction == null || faction.Reputation < req.Value) return false;
                        break;
                    case CommodityType.Item:
                        InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
                        ItemVO item = inventory.Inventory.Find(i => i.Name == req.ID);
                        if (item == null || item.Quantity < req.Value) return false;
                        break;
                    case CommodityType.Livre:
                        if (model.Livre < req.Value) return false;
                        break;
                    case CommodityType.Location:
                        ParisModel paris = AmbitionApp.GetModel<ParisModel>();
                        if (!paris.Locations.Contains(req.ID)) return false;
                        break;
                    case CommodityType.Reputation:
                        if (model.Reputation < req.Value) return false;
                        break;
                    case CommodityType.Servant:
                        ServantModel servants = AmbitionApp.GetModel<ServantModel>();
                        if (!servants.Servants.ContainsKey(req.ID)) return false;
                        break;
                    case CommodityType.Date:
                        CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
                        if (calendar.Today.Ticks < req.Value) return false;
                        break;
                }
            }
            return true;
        }
    }
}
