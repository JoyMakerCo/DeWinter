using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Dialog;

namespace DeWinter
{
	public class EventView : DialogView, IDialog<EventVO>
	{
	// TODO: Make UI respond to Events; move logic into command
	    public Text titleText;
	    public Text descriptionText;

	    private EventVO _event;

		public void OnOpen(EventVO e)
	    {
			Debug.Log("Starting Event: " + e.Name);

			_event = e;

	        //Text and Title
			titleText.text = e.Name;
			descriptionText.text = e.currentStage.Description;
	    }

	    public void EventOptionSelect(int option)
	    {
			int nextStage = _event.currentStage.Options[option].ChooseNextStage();
	        //Step 0: Did this just complete the event? If so then dismiss the Pop-Up and End the Event
	        if (nextStage == -1)
	        {
	        	Close();
	        }

	        //Step 1: Which stage do I advance to?
			_event.currentStageIndex = nextStage;
	        Debug.Log("This stage is now " + nextStage);

			if (_event.currentStage != null)
			{
				// For normal, non-tutorial partes
				// TODO: Make this not completely hacky
				if (_event.currentStage.Options[option].eventOptionParty == null)
				{
			        //Step 2: Grant Rewards
					foreach(RewardVO reward in _event.currentStage.Rewards)
					{
						DeWinterApp.SendMessage<RewardVO>(reward);
					}

			        //Step 3: What's the Description Text say now? The Event Option Buttons should update on their own
					descriptionText.text = _event.currentStage.Description;
				}
				// Start tutorial Party
				else
				{
					GameData.tonightsParty = _event.currentStage.Options[option].eventOptionParty;
					DeWinterApp.SendMessage<string>(GameMessages.LOAD_SCENE, "Game_PartyLoadOut"); 
				}
			}
			else
			{
				descriptionText.text = "";
			}
	    }
	}
}