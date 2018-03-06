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
	    private Text _text;
		private int _option;
		private Button _btn;

	    void Awake()
	    {
			_model = AmbitionApp.GetModel<IncidentModel>();
			_text = GetComponentInChildren<Text>();
			_option = gameObject.transform.GetSiblingIndex();
			_btn = gameObject.GetComponent<Button>();
			_btn.onClick.AddListener(OnClick);
			AmbitionApp.Subscribe<MomentVO>(HandleMoment);
	    }

	    void OnDestroy()
	    {
			AmbitionApp.Unsubscribe<MomentVO>(HandleMoment);
			_btn.onClick.RemoveAllListeners();
	    }

		private void HandleMoment(MomentVO m)
	    {
			bool show = false;
			if (m != null && _model.Config != null)
			{
				IncidentVO config = _model.Config;
				int index = Array.IndexOf(config.Moments, m);
				TransitionVO [] links = Array.FindAll(config.Transitions, l=>l.Index == index);
				show = (_option < links.Length);
				if (show) _text.text = links[_option].Text;
			}
			if (!show && _option == 0) _text.text = "<End>";
			this.gameObject.SetActive(show || _option == 0);
	    }

		private void OnClick()
		{
			AmbitionApp.SendMessage<int>(IncidentMessages.INCIDENT_OPTION, _option);
		}
	}
}
