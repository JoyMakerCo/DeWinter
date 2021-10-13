using UnityEngine;
using System.Collections;
using System;

namespace Ambition
{
    public class QuestJournal : SortableList<QuestVO>
    {
        protected override QuestVO[] FetchListData() => AmbitionApp.Gossip.Quests.ToArray();

        protected override Comparison<QuestVO> GetComparer(int sortIndex)
        {
            switch(sortIndex)
            {
                case 1: return _Value;
                case 2: return _Freshness;
            }
            return _Faction;
        }

        private int _Faction(QuestVO a, QuestVO b)
        {
            int A = (int)a.Faction;
            int B = (int)b.Faction;
            return A < B ? -1 : A == B ? 0 : 1;
        }

        private int _Value(QuestVO a, QuestVO b)
        {
            return a.Reward.Value < b.Reward.Value
                ? 1
                : b.Reward.Value == a.Reward.Value
                ? 0
                : -1;
        }

        private int _Freshness(QuestVO a, QuestVO b)
        {
            return a.Created < b.Created
                ? 1
                : b.Created == a.Created
                ? 0
                : -1;
        }
    }
}
