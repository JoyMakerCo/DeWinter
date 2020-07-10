using UnityEngine;
using System;
namespace Ambition
{
    public static class IncidentReq
    {
        public static bool Check(RequirementVO req)
        {
            // req.ID incident name
            return AmbitionApp.GetModel<IncidentModel>().IsComplete(req.ID);
        }
    }
}
