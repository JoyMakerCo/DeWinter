using System.Linq;
namespace Ambition
{
    public static class ItemReq
    {
        public static bool Check(RequirementVO req)
        {
            InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
            return inventory.Inventory.Keys.Any(i => i.ToString() == req.ID);
        }
    }
}
