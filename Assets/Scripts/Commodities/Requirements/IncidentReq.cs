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
            var incidentCount = _model.GetPlayCount(req.ID);

            return RequirementsSvc.Check(req, incidentCount);
        }
    }
}
