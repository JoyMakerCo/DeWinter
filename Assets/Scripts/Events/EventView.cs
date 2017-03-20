using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Dialog;

namespace DeWinter
{
	public class EventView : DialogView, IDialog<EventVO>
	{
	    public Text titleText;
	    public Text descriptionText;

	    private EventVO _event;

		public void OnOpen(EventVO e)
	    {
	    	_event = e;
	    }

	    void Start()
	    {
			DeWinterApp.Subscribe<EventVO>(HandleEventUpdate);
			DeWinterApp.SendMessage<EventVO>(_event);
	    }

		private void HandleEventUpdate(EventVO e)
		{
			_event = e;

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
				DeWinterApp.SendMessage<EventVO>(_event);

				// Set descriptive text
				descriptionText.text = _event.currentStage.Description;

				// Grant rewards
				if (_event.currentStage.Rewards != null)
				{
					foreach(RewardVO reward in _event.currentStage.Rewards)
					{
						DeWinterApp.SendMessage<RewardVO>(reward);
					}
				}
			}
			else
			{
				// That's an error.
				descriptionText.text = "";
			}
	    }
	}
}