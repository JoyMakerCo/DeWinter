using System;
namespace Ambition
{
    public static class MiscReq
    {
        public static bool Check(RequirementVO req)
        {
            if (!AmbitionApp.Game.Misc.TryGetValue(req.ID, out int value))
                value = 0;
            return RequirementsSvc.Check(req, value);
        }
    }
}
