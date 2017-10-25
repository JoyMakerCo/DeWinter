using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Dialog;

namespace Ambition
{
	public class EventView : DialogView
	{
	    public Text titleText;
	    public Text descriptionText;

	    private EventModel _model;
	    private Core.MessageSvc _messageSvc = Core.App.Service<Core.MessageSvc>();

	    public override void OnOpen ()
		{
			_model = AmbitionApp.GetModel<EventModel>();
			_messageSvc.Subscribe<EventVO>(HandleEventUpdate);
		}

		public override void OnClose ()
		{
			_messageSvc.Unsubscribe<EventVO>(HandleEventUpdate);
	    }

 		private void HandleEventUpdate(EventVO e)
		{
			if (e != null && e.currentStage != null)
			{
				titleText.text = e.Name;
				descriptionText.text = e.currentStage.Description;
			}
		}

	    public void EventOptionSelect(int option)
	    {
			EventOption opt = _model.Event.currentStage.Options[option];
			_model.Event.currentStageIndex = (opt != null)
				? opt.ChooseNextStage()
				: -1;

			// Advance to the next stage
			AmbitionApp.SendMessage<EventVO>(_model.Event);
		}
	}
}
