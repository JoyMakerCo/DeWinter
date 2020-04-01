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

            Enum.TryParse( fax[0], ignoreCase:true, out FactionType factionId0 );

            if (!factions.Factions.TryGetValue( factionId0, out FactionVO faction0 ))
            {
                Debug.LogErrorFormat("Faction {0} lookup failed",factionId0);
                return false;
            }

            if (fax.Length == 2)
            {
                // Two factions given, check faction 0 against faction 1
                Enum.TryParse( fax[1], ignoreCase:true, out FactionType factionId1 );

                if (!factions.Factions.TryGetValue( factionId1, out FactionVO faction1 ))
                {
                    Debug.LogErrorFormat("Faction {0} lookup failed",factionId1);
                    return false;
                }

                // this is SO shady
                req.Value = faction1.Power;
            }

            //Debug.LogFormat( "faction power {0}, target {1}", faction0.Power, req.Value);

            return RequirementsSvc.Check(req, faction0.Power);
        }
    }
}
