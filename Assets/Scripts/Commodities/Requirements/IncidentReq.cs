using UnityEngine;
using System;
namespace Ambition
{
    public static class IncidentReq
    {
        public static bool Check(RequirementVO req)
        {
            int isComplete = AmbitionApp.Story.IsComplete(req.ID, false) ? 1 : 0;
            return RequirementsSvc.Check(req, isComplete);
        }
    }
}
