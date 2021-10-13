using System;
namespace Ambition
{
    public static class CredReq
    {
        public static bool Check(RequirementVO req)
        {
            int value = AmbitionApp.GetModel<GameModel>().Credibility;
            int roll = Util.RNG.Generate(100);
            return (roll >= 95) ? true
                : (roll < 5) ? false
                : RequirementsSvc.Check(req, roll + value);
        }
    }
}
