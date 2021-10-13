using System;
using System.Collections.Generic;
namespace Ambition
{
    public class UpdateGossipModelCmd : Core.ICommand<CalendarModel>
    {
        public void Execute(CalendarModel calendar)
        {
            List<QuestVO> quests = AmbitionApp.Gossip.Quests;
            List<QuestVO> failed = new List<QuestVO>();
            QuestVO quest;

            for (int i=quests.Count-1; i>=0; --i)
            {
                quest = quests[i];
                if (calendar.Day > quest.Due)
                {
                    failed.Insert(0, quest);
                    AmbitionApp.Gossip.Quests.Remove(quest);
                }
            }
            failed.ForEach(q => AmbitionApp.SendMessage(QuestMessages.QUEST_FAILED, q));
            AmbitionApp.Gossip.Broadcast();
        }
    }
}
