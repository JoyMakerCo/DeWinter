using UnityEngine;
using System.Collections;
using Ambition;
using Util;

public class EventOption
{
    public string Label;
    //Determines the next stage the Event will hop to. If it's -1 then the Event finishes
    public int[] NextStage;
    public string servantRequired = null;

	// Default Constructor
    public EventOption() {}

	//Full Constructor, Servant Based Option with chance based outcome
	public EventOption(string servantType, string label, params int[] nextStage)
    {
        Label = label;
		servantRequired = servantType;
		NextStage = nextStage;
    }

    //Full Constructor, no chance based outcome
	public EventOption(string label, params int[] nextStage) : this (null, label, nextStage) {}
}