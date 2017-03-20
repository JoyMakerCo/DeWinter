using UnityEngine;
using System.Collections;
using DeWinter;
using Util;

public class EventOption
{
    public string optionButtonText;
    //Determines the next stage the Event will hop to. If it's -1 then the Event finishes
    public WeightedArray<int> Stages = new WeightedArray<int>();
    public string servantRequired = null;

	// Default Constructor
    public EventOption() {}

	//Full Constructor, Servant Based Option with chance based outcome
	public EventOption(string servantType, string buttonText, params int[] stages)
    {
        optionButtonText = buttonText;
		servantRequired = servantType;

		if (stages.Length == 1)
			Stages.Add(stages[0], 100);

        else for (int i=0; i+1<stages.Length; i+=2)
        {
        	Stages.Add(stages[i], stages[i+1]);
        }
    }

    //Full Constructor, no chance based outcome
	public EventOption(string buttonText, params int[] stages) : this (null, buttonText, stages) {}

    public int ChooseNextStage()
    {
    	return Stages.Choose();
    }
}