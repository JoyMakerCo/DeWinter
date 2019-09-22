using System.Linq;
namespace Ambition
{
    public static class ServantReq
    {
        // Checks both the "equipped" servant titles and the names of the servants employed
        public static bool Check(RequirementVO req)
        {
            ServantModel servants = AmbitionApp.GetModel<ServantModel>();
            return servants.Servants.Values.Any(s => s.Name == req.ID || s.Slot == req.ID || s.Type == req.ID);
        }
    }
}
