using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Dialog;

namespace DeWinter
{
	public class EventViewMediator : DialogView, IDialog<EventVO>
	{
	    public Text titleText;
	    public Text descriptionText;

	    private EventVO _event;

		void Awake()
	    {
			DeWinterApp.Subscribe<EventVO>(HandleEventUpdate);
	    }

		void OnDestroy()
	    {
			DeWinterApp.Unsubscribe<EventVO>(HandleEventUpdate);
	    }

		public void OnOpen(EventVO e)
	    {
			DeWinterApp.SendMessage<EventVO>(e);
	    }

 		private void HandleEventUpdate(EventVO e)
		{
			_event = e;

			titleText.text = e.Name;
			descriptionText.text = e.currentStage.Description;
		}

	    public void EventOptionSelect(int option)
	    {
			EventOption opt = _event.currentStage.Options[option];
			int nextStage = opt != null ? opt.ChooseNextStage() : -1;
			_event.currentStageIndex = nextStage;

	        // Advance to the next stage
			if (_event.currentStage != null)
	        {
	        	// Inform the app
				DeWinterApp.SendMessage<EventVO>(_event);

				// Set descriptive text
				descriptionText.text = _event.currentStage.Description;

				// Grant rewards
				foreach(RewardVO reward in _event.currentStage.Rewards)
				{
					DeWinterApp.SendMessage<RewardVO>(reward);
				}
			}

			// End of event.
			else
			{
				DeWinterApp.SendMessage<EventVO>(EventMessages.END_EVENT, _event);
				Close();
			}
	    }
	}
}