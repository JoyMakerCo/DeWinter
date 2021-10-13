namespace Ambition
{
    public static class ItemReq
    {
        public static bool Check(RequirementVO req) => AmbitionApp.Inventory.Inventory.Exists(i => i.ID == req.ID);
    }
}
