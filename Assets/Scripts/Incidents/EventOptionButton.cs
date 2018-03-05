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
		private IncidentModel _model;

	    public int option;
	    public Text myText;

	    void Awake()
	    {
				_model = AmbitionApp.GetModel<IncidentModel>();
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
				IncidentVO config = _model.Config;
				int index = Array.IndexOf(config.Moments, m);
				TransitionVO [] links = Array.FindAll(config.Transitions, l=>l.Index == index);
				show = (option < links.Length);
				if (show) myText.text = links[option].Text;
			}
			if (!show && option == 0) myText.text = "<End>";
			this.gameObject.SetActive(show || option == 0);
	    }
	}
}
