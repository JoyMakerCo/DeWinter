using System;
namespace Ambition
{
    public static class ExhaustionReq
    {
        // Checks the Paris model for known locations or currently available explorable locations
        public static bool Check(RequirementVO req)
        {
            int value = AmbitionApp.GetModel<GameModel>().Exhaustion.Value;
            return RequirementsSvc.Check(req, value);
        }
    }
}
