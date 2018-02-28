using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Ambition;

namespace Ambition
{
	public class EventOptionButton : MonoBehaviour
	{
		private EventModel _model;

	    public int option;
	    public Text myText;

	    void Awake()
	    {
			_model = AmbitionApp.GetModel<EventModel>();
			AmbitionApp.Subscribe<MomentVO>(HandleMoment);
	    }

	    void OnDestroy()
	    {
			AmbitionApp.Unsubscribe<MomentVO>(HandleMoment);
	    }

		private void HandleMoment(MomentVO m)
	    {
			bool show = false;
			if (m != null && _model.Config != null)
			{
				EventVO config = _model.Config;
				int index = Array.IndexOf(config.Moments, m);
				EventConfigLinkVO [] links = Array.FindAll(config.Links, l=>l.Index == index);
				show = (option < links.Length);
				if (show) myText.text = links[option].Text;
			}
			if (!show && option == 0) myText.text = "<End>";
			this.gameObject.SetActive(show || option == 0);
	    }
	}
}
