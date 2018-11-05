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
                        FactionModel factions = AmbitionApp.GetModel<FactionModel>();
                        return (req.ID != null && factions.Factions.ContainsKey(req.ID))
                            ? factions[req.ID].Reputation >= req.Value
                            : model.Reputation >= req.Value;
                    case CommodityType.Servant:
                        ServantModel servants = AmbitionApp.GetModel<ServantModel>();
                        if (!servants.Servants.ContainsKey(req.ID)) return false;
                        break;
                    case CommodityType.Date:
                        CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
                        if (calendar.Today.Ticks < req.Value) return false;
                        break;
                    case CommodityType.Mark:
                        MapModel map = AmbitionApp.GetModel<MapModel>();
                        return Array.Exists(map.Map.Rooms, r => r.HostHere && r.Cleared);
                }
            }
            return true;
        }
    }
}
