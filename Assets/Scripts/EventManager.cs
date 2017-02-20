using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DeWinter;

public class EventManager : MonoBehaviour
{
// TODO: Make UI respond to Events; move logic into command
    public Text titleText;
    public Text descriptionText;
    public string eventTime;

    private EventModel _eventModel;

    void Start()
    {
        Debug.Log("Event Start!");
		_eventModel = DeWinterApp.GetModel<EventModel>();

        //Select the Event
        WeightedSelection();
        //Reset the Event to its beginning
        _eventModel.SelectedEvent.currentStage = 0;
        //Text and Title
		titleText.text = _eventModel.SelectedEvent.eventTitle;
		descriptionText.text = _eventModel.SelectedEvent.eventStages[0].description;
    }

    //TODO Implement an actual Weighted Selection System. Currently, all the Events have the same weight.
    public void WeightedSelection()
    {
        int randomSelection = Random.Range(0, _eventModel.eventInventories[eventTime].Count);
		_eventModel.SelectedEvent = _eventModel.eventInventories[eventTime][randomSelection];
    }

    public void EventOptionSelect(int option)
    {
		int nextStage = _eventModel.SelectedEvent.eventStages[_eventModel.SelectedEvent.currentStage].stageEventOptions[option].ChooseNextStage();
        //Step 0: Did this just complete the event? If so then dismiss the Pop-Up and End the Event
        if (nextStage == -1)
        {
            //Close the Pop-Up Here
            Dismiss();
        }
        
        //Step 1: Which stage do I advance to?
		_eventModel.SelectedEvent.currentStage = nextStage;
        Debug.Log("This stage is now " + nextStage);
        
        //Step 2: Get the Money, Change the Rep
		_eventModel.SelectedEvent.EventStageRewards();

        //Step 3: What's the Description Text say now? The Event Option Buttons should update on their own
		descriptionText.text = _eventModel.SelectedEvent.eventStages[_eventModel.SelectedEvent.currentStage].description;
    }

    void Dismiss()
    {
        Destroy(transform.gameObject);
        Debug.Log("Event Finished!");
    }
}