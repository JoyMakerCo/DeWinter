using System;
using System.Collections.Generic;
namespace Ambition
{
    public static class FactionPowerReq
    {
        public static bool Check(RequirementVO req)
        {
            FactionModel model = AmbitionApp.GetModel<FactionModel>();
            char[] sep = new char[] { ',',' ' };
            string[] tokens = req.ID.Split( sep );
            List<FactionVO> factions = new List<FactionVO>();
            FactionType faction;
            foreach(string token in tokens)
            {
                if (Enum.TryParse(token, true, out faction))
                {
                    factions.Add(model.Factions[faction]);
                }
            }
            if (factions.Count > 1) req.Value = factions[1].Power;
            return RequirementsSvc.Check(req, factions[0].Power);
        }
    }
}
