using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Event
{
    public int eventWeight;
    public string eventTitle;

    //Stuff for the Event Stages
    public List<EventStage> eventStages = new List<EventStage>();
    public int currentStage = 0;

    //2 Stage Constructor (1 Choice Stage and 1 Aftermath Stage)
    public Event(int weight, string title, EventStage s00, EventStage s01)
    {
        eventWeight = weight;
        eventTitle = title;
        eventStages.Add(s00);
        eventStages.Add(s01);
    }
    //3 Stage Constructor (1 Choice Stages and 2 Aftermath Stages)
    public Event(int weight, string title, EventStage s00, EventStage s01, EventStage s02)
    {
        eventWeight = weight;
        eventTitle = title;
        eventStages.Add(s00);
        eventStages.Add(s01);
        eventStages.Add(s02);
    }

    //4 Stage Constructor (1 Choice Stages and 3 Aftermath Stages)
    public Event(int weight, string title, EventStage s00, EventStage s01, EventStage s02, EventStage s03)
    {
        eventWeight = weight;
        eventTitle = title;
        eventStages.Add(s00);
        eventStages.Add(s01);
        eventStages.Add(s02);
        eventStages.Add(s03);
    }

    //5 Stage Constructor (Example: 2 Choice Stages and 3 Aftermath Stages)
    public Event(int weight, string title, EventStage s00, EventStage s01, EventStage s02, EventStage s03, EventStage s04)
    {
        eventWeight = weight;
        eventTitle = title;
        eventStages.Add(s00);
        eventStages.Add(s01);
        eventStages.Add(s02);
        eventStages.Add(s03);
        eventStages.Add(s04);
    }

    //6 Stage Constructor (Example: 2 Choice Stages and 4 Aftermath Stages)
    public Event(int weight, string title, EventStage s00, EventStage s01, EventStage s02, EventStage s03, EventStage s04, EventStage s05)
    {
        eventWeight = weight;
        eventTitle = title;
        eventStages.Add(s00);
        eventStages.Add(s01);
        eventStages.Add(s02);
        eventStages.Add(s03);
        eventStages.Add(s04);
        eventStages.Add(s05);
    }

    //7 Stage Constructor
    public Event(int weight, string title, EventStage s00, EventStage s01, EventStage s02, EventStage s03, EventStage s04, EventStage s05, EventStage s06)
    {
        eventWeight = weight;
        eventTitle = title;
        eventStages.Add(s00);
        eventStages.Add(s01);
        eventStages.Add(s02);
        eventStages.Add(s03);
        eventStages.Add(s04);
        eventStages.Add(s05);
        eventStages.Add(s06);
    }

    //8 Stage Constructor
    public Event(int weight, string title, EventStage s00, EventStage s01, EventStage s02, EventStage s03, EventStage s04, EventStage s05, EventStage s06, EventStage s07)
    {
        eventWeight = weight;
        eventTitle = title;
        eventStages.Add(s00);
        eventStages.Add(s01);
        eventStages.Add(s02);
        eventStages.Add(s03);
        eventStages.Add(s04);
        eventStages.Add(s05);
        eventStages.Add(s06);
        eventStages.Add(s07);
    }

    //9 Stage Constructor
    public Event(int weight, string title, EventStage s00, EventStage s01, EventStage s02, EventStage s03, EventStage s04, EventStage s05, EventStage s06, EventStage s07, EventStage s08)
    {
        eventWeight = weight;
        eventTitle = title;
        eventStages.Add(s00);
        eventStages.Add(s01);
        eventStages.Add(s02);
        eventStages.Add(s03);
        eventStages.Add(s04);
        eventStages.Add(s05);
        eventStages.Add(s06);
        eventStages.Add(s07);
        eventStages.Add(s08);
    }

    //10 Stage Constructor (Example: The Intro Event)
    public Event(int weight, string title, EventStage s00, EventStage s01, EventStage s02, EventStage s03, EventStage s04, EventStage s05, EventStage s06, EventStage s07, EventStage s08, EventStage s09)
    {
        eventWeight = weight;
        eventTitle = title;
        eventStages.Add(s00);
        eventStages.Add(s01);
        eventStages.Add(s02);
        eventStages.Add(s03);
        eventStages.Add(s04);
        eventStages.Add(s05);
        eventStages.Add(s06);
        eventStages.Add(s07);
        eventStages.Add(s08);
        eventStages.Add(s09);
    }

    //12 Stage Constructor (Example: The Intro Event)
    public Event(int weight, string title, EventStage s00, EventStage s01, EventStage s02, EventStage s03, EventStage s04, EventStage s05, EventStage s06, EventStage s07, EventStage s08, EventStage s09, EventStage s10, EventStage s11)
    {
        eventWeight = weight;
        eventTitle = title;
        eventStages.Add(s00);
        eventStages.Add(s01);
        eventStages.Add(s02);
        eventStages.Add(s03);
        eventStages.Add(s04);
        eventStages.Add(s05);
        eventStages.Add(s06);
        eventStages.Add(s07);
        eventStages.Add(s08);
        eventStages.Add(s09);
        eventStages.Add(s10);
        eventStages.Add(s11);
    }

    public void EventStageRewards()
    {
        GameData.reputationCount += eventStages[currentStage].stageRepChange;
        GameData.moneyCount += eventStages[currentStage].stageMoneyChange;
        if (eventStages[currentStage].stageEnemyAdd != null)
        {
            GameData.enemyList.Add(eventStages[currentStage].stageEnemyAdd);
        }
    }
}
