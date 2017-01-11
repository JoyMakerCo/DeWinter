using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour {

    public Text titleText;
    public Text descriptionText;
    public string eventTime;
    public EventInventory eventInventory;

    void Start()
    {
        EventStart();
    }
    
    void EventStart()
    {
        Debug.Log("Event Start!");
        switch (eventTime)
        {
            case "intro":
                eventInventory.StockIntroInventory();
                break;
            case "party":
                eventInventory.StockPartyInventory(GameData.tonightsParty);
                break;
            case "night":
                eventInventory.StockNightInventory();
                break;
        }
        //Select the Event
        WeightedSelection();
        //Reset the Event to its beginning
        GameData.selectedEvent.currentStage = 0;
        //Text and Title
        titleText.text = GameData.selectedEvent.eventTitle;
        descriptionText.text = GameData.selectedEvent.eventStages[0].description;
    }

    //TODO Implement an actual Weighted Selection System. Currently, all the Events have the same weight.
    public void WeightedSelection()
    {
        int randomSelection = 0;
        randomSelection = Random.Range(0, EventInventory.eventInventories[eventTime].Count); // Doesn't need a +1, as Random.Range with ints isn't maximally inclusive
        GameData.selectedEvent = EventInventory.eventInventories[eventTime][randomSelection];
    }

    public void EventOptionSelect(int option)
    {
        int nextStage = GameData.selectedEvent.eventStages[GameData.selectedEvent.currentStage].stageEventOptions[option].ChooseNextStage();
        //Step 0: Did this just complete the event? If so then dismiss the Pop-Up and End the Event
        if (nextStage == -1)
        {
            //Close the Pop-Up Here
            Dismiss();
        }
        
        //Step 1: Which stage do I advance to?
        GameData.selectedEvent.currentStage = nextStage;
        Debug.Log("This stage is now " + nextStage);
        
        //Step 2: Get the Money, Change the Rep
        GameData.selectedEvent.EventStageRewards();

        //Step 3: What's the Description Text say now? The Event Option Buttons should update on their own
        descriptionText.text = GameData.selectedEvent.eventStages[GameData.selectedEvent.currentStage].description;
    }

    void Dismiss()
    {
        Destroy(transform.gameObject);
        GameData.activeModals--;
        Debug.Log("Event Finished!");
    }
}
