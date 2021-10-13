using System;
using System.Collections.Generic;
namespace Ambition
{
    public class CompleteQuestCmd : Core.ICommand<QuestVO>
    {
        private const string COMPLETE_TUTORIAL_QUEST_INCIDENT = "pierresInformation";

        public void Execute(QuestVO quest)
        {
            if (AmbitionApp.GetModel<GossipModel>().Quests.Remove(quest))
            {
                AmbitionApp.SendMessage(quest.Reward);
                AmbitionApp.Story.Schedule(COMPLETE_TUTORIAL_QUEST_INCIDENT);
            }
        }
    }
}
