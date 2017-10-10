using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Dialog;

namespace Ambition
{
	public class EventViewMediator : DialogView, Util.IInitializable<EventVO>
	{
	    public Text titleText;
	    public Text descriptionText;

	    private EventVO _event;
	    private Core.MessageSvc _messageSvc = Core.App.Service<Core.MessageSvc>();

		public override void OnClose ()
		{
			base.OnClose();
			_messageSvc.Unsubscribe<EventVO>(HandleEventUpdate);
	    }

		public void Initialize(EventVO e)
	    {
			_messageSvc.Subscribe<EventVO>(HandleEventUpdate);
			_messageSvc.Send<EventVO>(e);
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
				AmbitionApp.SendMessage<EventVO>(_event);

				// Set descriptive text
				descriptionText.text = _event.currentStage.Description;

				// Grant rewards
				foreach(RewardVO reward in _event.currentStage.Rewards)
				{
					AmbitionApp.SendMessage<RewardVO>(reward);
				}
			}

			// End of event.
			else
			{
				AmbitionApp.SendMessage<EventVO>(EventMessages.END_EVENT, _event);
				Close();
			}
	    }
	}
}