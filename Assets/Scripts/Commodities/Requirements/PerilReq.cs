using System;
namespace Ambition
{
    public static class PerilReq
    {
        public static bool Check(RequirementVO req)
        {
            int value = AmbitionApp.GetModel<GameModel>().Peril.Value;
            return RequirementsSvc.Check(req, value);
        }
    }
}
