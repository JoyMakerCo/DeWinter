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

	public LevelManager levelManager; //Assigned via the pop up manager
	
    private EventModel _eventModel;

    void Start()
    {
        Debug.Log("Event Start!");
		_eventModel = DeWinterApp.GetModel<EventModel>();

        //Select the Event
        WeightedSelection();
        //Reset the Event to its beginning
        _eventModel.SelectedEvent.currentStageIndex = 0;
        //Text and Title
		titleText.text = _eventModel.SelectedEvent.Name;
		descriptionText.text = _eventModel.SelectedEvent.currentStage.Description;
    }

    //TODO Implement an actual Weighted Selection System. Currently, all the Events have the same weight.
    public void WeightedSelection()
    {
        int randomSelection = Random.Range(0, _eventModel.eventInventories[eventTime].Length);
		_eventModel.SelectedEvent = _eventModel.eventInventories[eventTime][randomSelection];
    }

    public void EventOptionSelect(int option)
    {
		int nextStage = _eventModel.SelectedEvent.currentStage.Options[option].ChooseNextStage();
        //Step 0: Did this just complete the event? If so then dismiss the Pop-Up and End the Event
        if (nextStage == -1)
        {
            //Close the Pop-Up Here
            Dismiss();
        }

        //Step 1: Which stage do I advance to?
		_eventModel.SelectedEvent.currentStageIndex = nextStage;
        Debug.Log("This stage is now " + nextStage);

		if (_eventModel.SelectedEvent.currentStage != null)
		{
			// For normal, non-tutorial partes
			// TODO: Make this not completely hacky
			if (_eventModel.SelectedEvent.currentStage.Options[option].eventOptionParty == null)
			{
		        //Step 2: Grant Rewards
				foreach(RewardVO reward in _eventModel.SelectedEvent.currentStage.Rewards)
				{
					DeWinterApp.SendMessage<RewardVO>(reward);
				}

		        //Step 3: What's the Description Text say now? The Event Option Buttons should update on their own
				descriptionText.text = _eventModel.SelectedEvent.currentStage.Description;
			}
			// Start tutorial Party
			else
			{
				GameData.tonightsParty = _eventModel.SelectedEvent.currentStage.Options[option].eventOptionParty;
	            levelManager.LoadLevel("Game_PartyLoadOut");
			}
		}
		else
		{
			descriptionText.text = "";
		}
    }

    void Dismiss()
    {
        Destroy(transform.gameObject);
        Debug.Log("Event Finished!");
    }
}