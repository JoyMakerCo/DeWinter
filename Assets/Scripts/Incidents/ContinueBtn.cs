using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class ContinueBtn : MonoBehaviour
	{
        private Button _btn;
        private IncidentModel _model;
		private TransitionVO _transition;
		void Awake () {
            _model = AmbitionApp.GetModel<IncidentModel>();
            AmbitionApp.Subscribe<TransitionVO[]>(HandleTransitions);
			_btn = gameObject.GetComponent<Button>();
			_btn.onClick.AddListener(Next);
		}
		
		void OnDestroy () {
			AmbitionApp.Unsubscribe<TransitionVO[]>(HandleTransitions);
			_btn.onClick.RemoveAllListeners();
		}

		private void HandleTransitions(TransitionVO [] transitions)
		{
			Debug.LogFormat("ContinueBtn.HandleTransitions");

            _transition = null;
            switch (transitions.Length)
            {
                case 0:
                    _btn.interactable = true;
                    break;
                case 1:
                    TransitionVO[] linkdata = _model.Incident?.LinkData;
                    if (linkdata == null)
                    {
                        _btn.interactable = true;
                    }
                    else
                    {
                        _transition = transitions[0];
                        int index = Array.IndexOf(linkdata, _transition);
                        string txt = AmbitionApp.Localize(_model.Incident.LocalizationKey + ".link." + index.ToString());
                        _btn.interactable = string.IsNullOrWhiteSpace(txt);
                    }
                    break;
                default:
                    _btn.interactable = false;
                    break;
            }
		}

		private void Next()
		{
			Debug.LogFormat("ContinueBtn.Next");
			AmbitionApp.SendMessage(IncidentMessages.TRANSITION, _transition);
			FMODUnity.RuntimeManager.PlayOneShot("event:/One Shot SFX/Mouse_click"); //Literally only ever plays this sound. It will never need to play anything else.
        }
	}
}
