using UnityEngine;
using System;
namespace Ambition
{
    public static class FactionAllegianceReq
    {
        public static bool Check(RequirementVO req)
        {
            //Debug.LogFormat("FactionAllegianceReq check");
            FactionModel factions = AmbitionApp.GetModel<FactionModel>();
            int allegiance = 0;
            if (Enum.TryParse(req.ID, ignoreCase:true, out FactionType factionID))
            {
                allegiance = factions.Factions.TryGetValue(factionID, out FactionVO faction)
                    ? faction.Allegiance : 0;

                //Debug.LogFormat("Faction {0} allegiance is {1}, checking {2} {3}",req.ID, allegiance, req.Operator, req.Value);
            }
            else
            {
                Debug.LogErrorFormat("Bad faction ID {0} in FactionAllegianceReq",req.ID);
            }

            return RequirementsSvc.Check(req, allegiance);
        }
    }
}
