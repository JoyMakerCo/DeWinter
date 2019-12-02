using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
    public class ActiveQuestReward : Core.ICommand<CommodityVO>
    {
        public void Execute(CommodityVO reward)
        {
            AmbitionApp.GetModel<QuestModel>().ActiveQuestState = reward.Value;
        }
    }
}
