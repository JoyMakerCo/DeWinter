using System;
namespace Ambition
{
    public static class ExhaustionReq
    {
        // Checks the Paris model for known locations or currently available explorable locations
        public static bool Check(RequirementVO req)
        {
            return RequirementsSvc.Check(req, AmbitionApp.Game.Exhaustion);
        }
    }
}
