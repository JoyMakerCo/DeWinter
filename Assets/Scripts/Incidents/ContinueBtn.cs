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
			if (transitions.Length == 0 || transitions.Length == 1 && transitions[0].Text.Length == 0)
			{
				_btn.interactable = true;
				if (transitions.Length == 1)
				{
					_transition = transitions[0];
				}
			}
			else
			{
				_btn.interactable = false;
			}
		}

		private void Next()
		{
			Debug.LogFormat("ContinueBtn.Next");

			AmbitionApp.SendMessage(_transition);
			/* 
            MomentVO[] neighbors = _model.Incident?.GetNeighbors(_model.Moment);
            if ((neighbors?.Length ?? 0) > 0)
			{
				Debug.LogFormat( "updating model moment to {0}", neighbors[0].ToString() );
				_model.Moment = neighbors[0];
			} 
            else
			{
				Debug.LogFormat( "No neighbors, ending incident");
				AmbitionApp.SendMessage(IncidentMessages.END_INCIDENT, _model.Incident);
			} 
            */
			FMODUnity.RuntimeManager.PlayOneShot("event:/One Shot SFX/Mouse_click"); //Literally only ever plays this sound. It will never need to play anything else.
        }
	}
}
