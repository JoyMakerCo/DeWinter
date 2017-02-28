using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DeWinter;

// TODO: EventVO
public class Event
{
    public int eventWeight;
    public string eventTitle;

    //Stuff for the Event Stages
    public List<EventStage> eventStages = new List<EventStage>();
    public int currentStage = 0;

    public Event(int weight, string title, params EventStage[] stages)
    {
        eventWeight = weight;
        eventTitle = title;
        foreach (EventStage s in stages)
	        eventStages.Add(s);
    }
    public void EventStageRewards()
    {
		AdjustValueVO vo = new AdjustValueVO(BalanceTypes.LIVRE, eventStages[currentStage].stageMoneyChange);
		DeWinterApp.SendMessage<AdjustValueVO>(vo);

		vo.Type = BalanceTypes.REPUTATION;
		vo.Amount = eventStages[currentStage].stageRepChange;
		vo.IsRequest = true;
		DeWinterApp.SendMessage<AdjustValueVO>(vo);

        if (eventStages[currentStage].stageEnemyAdd != null)
        {
            EnemyInventory.AddEnemy(eventStages[currentStage].stageEnemyAdd);
        }
    }
}