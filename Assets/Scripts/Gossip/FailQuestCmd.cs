using System;
using System.Collections.Generic;
namespace Ambition
{
    public class FailQuestCmd : Core.ICommand<QuestVO>
    {
        public void Execute(QuestVO quest)
        {
            GossipModel model = AmbitionApp.GetModel<GossipModel>();
            if (model.Quests.Remove(quest))
            {
            }
        }
    }

    public class FailLastQuestCmd : Core.ICommand
    {
        public void Execute()
        {
            List<QuestVO> quests = AmbitionApp.Gossip.Quests;
            if (quests.Count > 0)
            {
                AmbitionApp.Execute<FailQuestCmd, QuestVO>(quests[quests.Count - 1]);
            }
        }
    }
}
