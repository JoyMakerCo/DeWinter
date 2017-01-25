using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventStage : MonoBehaviour {

    public string description;
    public List<EventOption> stageEventOptions = new List<EventOption>();
    //This is the Rep and Money Change that occurs upon this Stage BEGINNING
    public int stageRepChange;
    public int stageMoneyChange;
    public Enemy stageEnemyAdd;
    public Gossip stageGossipAdd;
    public FactionVO stageFaction1; //The Faction that's being altered
    public string stageFaction1Value; //Is is the Player's Faction Rep that's being changed? Or is it the Faction Allegiance or Power?
    public int stageFaction1Change; //How much is it being changed by?
    public FactionVO stageFaction2; //Is there a second faction being affected?
    public string stageFaction2Value; //Same as the first
    public int stageFaction2Change; //Also same as the first

    //1 Option Constructor
    public EventStage(string desc, int sRepChng, int sMonChng, EventOption op0)
    {
        description = desc;
        stageRepChange = sRepChng;
        stageMoneyChange = sMonChng;
        stageEventOptions.Add(op0);
        stageEventOptions.Add(new EventOption());
        stageEventOptions.Add(new EventOption());
        stageEventOptions.Add(new EventOption());
    }
    
    //2 Option Constructor
    public EventStage(string desc, int sRepChng, int sMonChng, EventOption op0, EventOption op1)
    {
        description = desc;
        stageRepChange = sRepChng;
        stageMoneyChange = sMonChng;
        stageEventOptions.Add(op0);
        stageEventOptions.Add(op1);
        stageEventOptions.Add(new EventOption());
        stageEventOptions.Add(new EventOption());
    }
    
    //3 Option Constructor
    public EventStage(string desc, int sRepChng, int sMonChng, EventOption op0, EventOption op1, EventOption op2)
    {
        description = desc;
        stageRepChange = sRepChng;
        stageMoneyChange = sMonChng;
        stageEventOptions.Add(op0);
        stageEventOptions.Add(op1);
        stageEventOptions.Add(op2);
        stageEventOptions.Add(new EventOption());
    }

    //4 Option Constructor
    public EventStage(string desc, int sRepChng, int sMonChng, EventOption op0, EventOption op1, EventOption op2, EventOption op3)
    {
        description = desc;
        stageRepChange = sRepChng;
        stageMoneyChange = sMonChng;
        stageEventOptions.Add(op0);
        stageEventOptions.Add(op1);
        stageEventOptions.Add(op2);
        stageEventOptions.Add(op3);
    }

    //1 Option Constructor that Adds an Enemy
    public EventStage(string desc, int sRepChng, int sMonChng, Enemy sEAdd, EventOption op0)
    {
        description = desc;
        stageRepChange = sRepChng;
        stageMoneyChange = sMonChng;
        stageEnemyAdd = sEAdd;
        stageEventOptions.Add(op0);
        stageEventOptions.Add(new EventOption());
        stageEventOptions.Add(new EventOption());
        stageEventOptions.Add(new EventOption());
    }

    //1 Option Constructor that Adds Gossip
    public EventStage(string desc, int sRepChng, int sMonChng, Gossip sGAdd, EventOption op0)
    {
        description = desc;
        stageRepChange = sRepChng;
        stageMoneyChange = sMonChng;
        stageGossipAdd = sGAdd;
        stageEventOptions.Add(op0);
        stageEventOptions.Add(new EventOption());
        stageEventOptions.Add(new EventOption());
        stageEventOptions.Add(new EventOption());
    }

    //1 Option Constructor that alters a Faction
    public EventStage(string desc, int sRepChng, int sMonChng, FactionVO fac1, string fac1Value, int fac1Chnge, EventOption op0)
    {
        description = desc;
        stageRepChange = sRepChng;
        stageMoneyChange = sMonChng;
        stageFaction1 = fac1;
        stageFaction1Value = fac1Value;
        stageFaction1Change = fac1Chnge;
        stageEventOptions.Add(op0);
        stageEventOptions.Add(new EventOption());
        stageEventOptions.Add(new EventOption());
        stageEventOptions.Add(new EventOption());
    }

    //1 Option Constructor that alters two Factions
    public EventStage(string desc, int sRepChng, int sMonChng, FactionVO fac1, string fac1Value, int fac1Chnge, FactionVO fac2, string fac2Value, int fac2Chnge, EventOption op0)
    {
        description = desc;
        stageRepChange = sRepChng;
        stageMoneyChange = sMonChng;
        stageFaction1 = fac1;
        stageFaction1Value = fac1Value;
        stageFaction1Change = fac1Chnge;
        stageFaction2 = fac2;
        stageFaction2Value = fac2Value;
        stageFaction2Change = fac2Chnge;
        stageEventOptions.Add(op0);
        stageEventOptions.Add(new EventOption());
        stageEventOptions.Add(new EventOption());
        stageEventOptions.Add(new EventOption());
    }
}
