using System;
using System.Globalization;

namespace Ambition
{
    public static class ChanceReq
    {
        public static bool Check(RequirementVO req) => RequirementsSvc.Check(req, Util.RNG.Generate(100));
    }
}
