using UnityEngine;
using System;
namespace Ambition
{
    public static class ActiveQuestReq
    {
        public static bool Check(RequirementVO req)
        {
			var active = AmbitionApp.GetModel<QuestModel>().ActiveQuestState;
            Debug.LogFormat("ActiveQuestReq.Check quest is {0}",active);
            return RequirementsSvc.Check(req, active);
        }
    }
}
