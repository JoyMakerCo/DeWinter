using UnityEngine;
using System.Collections;

public class EstateEventSummoner : MonoBehaviour {

    public SceneFadeInOut screenFader;

    // Use this for initialization
    void Start()
    {
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
    }
}
