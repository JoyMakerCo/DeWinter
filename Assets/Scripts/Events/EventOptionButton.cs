using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Ambition;

namespace Ambition
{
	public class EventOptionButton : MonoBehaviour
	{
	    public int option;

	    public Text myText;

	    void Awake()
	    {
			AmbitionApp.Subscribe<EventVO>(HandleEventUpdate);
	    }

	    void OnDestroy()
	    {
			AmbitionApp.Unsubscribe<EventVO>(HandleEventUpdate);
	    }

		private void HandleEventUpdate(EventVO e)
	    {
	    	if (e==null || e.currentStage == null) return;
			EventStage stage = e.currentStage;
			bool show = (option < stage.Options.Length && stage.Options[option].optionButtonText != null);
			if (show)
			{
				EventOption eventOption = stage.Options[option];
				if (eventOption.servantRequired != null)
				{
					ServantModel model = AmbitionApp.GetModel<ServantModel>();
					show = model.Servants.ContainsKey(eventOption.servantRequired);
				}
				if (show) myText.text = eventOption.optionButtonText;
			}
			this.gameObject.SetActive(show);
	    }
	}
}
