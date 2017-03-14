using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

namespace DeWinter
{
	public class StartEventCommand : ICommand<string>
	{
		public void Execute(string eventName)
		{
/*
			int randomRangeMax = 100;
        //Intro Event
        if (GameData.currentDay == 0)
        {
            screenFader.gameObject.SendMessage("CreateEventPopUp", "intro");
        }
        //All the Other Events
        if (GameData.currentDay > 2 && (Random.Range(1, randomRangeMax + 1) < GameData.eventChance))
        {
            screenFader.gameObject.SendMessage("CreateEventPopUp", "night");
        }
*/
		}
	}
}