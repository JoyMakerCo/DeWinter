namespace Ambition
{
    public static class ItemReq
    {
        public static bool Check(RequirementVO req)
        {
            InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
            foreach (ItemVO item in inventory.Inventory)
            {
                if (item.ID == req.ID) return true;
            }
            foreach(ItemVO item in inventory.Equipped.Values)
            {
                if (item?.ID == req.ID) return true;
            }
            return false;
        }
    }
}
