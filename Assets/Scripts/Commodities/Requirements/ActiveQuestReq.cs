using UnityEngine;
using System;
using System.Collections.Generic;
namespace Ambition
{
    public static class ActiveQuestReq
    {
        public static bool Check(RequirementVO req) => RequirementsSvc.Check(req, AmbitionApp.Gossip.Quests.Count);
    }
}
