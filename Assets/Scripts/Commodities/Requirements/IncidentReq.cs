using UnityEngine;
using System;
namespace Ambition
{
    public static class IncidentReq
    {
        public static bool Check(RequirementVO req)
        {
            // req.ID incident name
            // req.Value vx number of times incident played

            IncidentModel _model = AmbitionApp.GetModel<IncidentModel>();
            if (!_model.PlayCount.TryGetValue(req.ID, out int count))
            {
                count = 0;
            }
            return RequirementsSvc.Check(req, count);
        }
    }
}
