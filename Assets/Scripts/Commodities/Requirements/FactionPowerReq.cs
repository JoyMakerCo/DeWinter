using UnityEngine;
using System;
namespace Ambition
{
    public static class FactionPowerReq
    {
        public static bool Check(RequirementVO req)
        {
            FactionModel factions = AmbitionApp.GetModel<FactionModel>();

            var sep = new char[] { ',',' ' };

            var fax = req.ID.Split( sep );
            if (fax.Length > 2)
            {
                Debug.LogErrorFormat("Bad parse of factions {0} in FactionPowerReq",req.ID);
                return false;
            }

            Enum.TryParse( fax[0], out FactionType factionId0 );
            Enum.TryParse( fax[1], out FactionType factionId1 );

            if (!factions.Factions.TryGetValue( factionId0, out FactionVO faction0 ))
            {
                Debug.LogErrorFormat("Faction {0} lookup failed",factionId0);
                return false;
            }

            if (!factions.Factions.TryGetValue( factionId1, out FactionVO faction1 ))
            {
                Debug.LogErrorFormat("Faction {0} lookup failed",factionId1);
                return false;
            }

            // this is SO shady
            req.Value = faction1.Power;

            return RequirementsSvc.Check(req, faction0.Power);
        }
    }
}
