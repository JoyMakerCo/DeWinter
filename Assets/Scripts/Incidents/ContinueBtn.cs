using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class ContinueBtn : MonoBehaviour
	{
		private Button _btn;
		void Awake () {
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
			_btn.interactable = transitions.Length == 0 || transitions.Length == 1 && transitions[0].Text.Length == 0;
		}

		private void Next()
		{
			AmbitionApp.SendMessage<int>(IncidentMessages.INCIDENT_OPTION, 0);
		}
	}
}
