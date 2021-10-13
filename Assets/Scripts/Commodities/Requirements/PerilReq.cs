using System;
namespace Ambition
{
    public static class PerilReq
    {
        public static bool Check(RequirementVO req)
        {
            int value = AmbitionApp.GetModel<GameModel>().Peril;
            return RequirementsSvc.Check(req, value);
        }
    }
}
