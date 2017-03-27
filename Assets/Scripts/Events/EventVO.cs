using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DeWinter;

namespace DeWinter
{
	public class EventVO
	{
	    public string Name;

		public int eventWeight;

	    //Stuff for the Event Stages
	    public EventStage[] eventStages;
	    public int currentStageIndex = 0;

		public EventStage currentStage
		{
			get { return (currentStageIndex >= 0 && currentStageIndex < eventStages.Length)
				? eventStages[currentStageIndex]
				: null;
			}
		}

	    public EventVO(int weight, string title, params EventStage[] stages)
	    {
	        eventWeight = weight;
	        Name = title;
			eventStages = stages;
	    }
	}
}