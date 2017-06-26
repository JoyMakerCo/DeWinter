using UnityEngine;
using System.Collections;
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
			DeWinterApp.Subscribe<EventVO>(HandleEventUpdate);
	    }

	    void OnDestroy()
	    {
			DeWinterApp.Unsubscribe<EventVO>(HandleEventUpdate);
	    }

		private void HandleEventUpdate(EventVO e)
	    {
			EventStage stage = e.currentStage;
			bool show = (option < stage.Options.Length && stage.Options[option].optionButtonText != null);
			if (show)
			{
				EventOption eventOption = stage.Options[option];
				if (show && eventOption.servantRequired != null)
				{
					ServantModel smod = DeWinterApp.GetModel<ServantModel>();
					show = smod.Hired.ContainsKey(eventOption.servantRequired);
				}
				if (show) myText.text = eventOption.optionButtonText;
			}
			this.gameObject.SetActive(show);
	    }
	}
}