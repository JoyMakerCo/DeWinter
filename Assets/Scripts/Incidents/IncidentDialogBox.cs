using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class IncidentDialogBox : MonoBehaviour
	{
		private Button _btn;

		void Awake ()
		{
			_btn = gameObject.GetComponent<Button>();
			_btn.onClick.AddListener(Next);
			AmbitionApp.Subscribe<TransitionVO[]>(HandleTransitions);
		}
		
		void OnDestroy ()
		{
			AmbitionApp.Unsubscribe<TransitionVO[]>(HandleTransitions);
			_btn.onClick.RemoveAllListeners();
		}

		public void Next()
		{
			AmbitionApp.SendMessage<int>(IncidentMessages.INCIDENT_OPTION, 0);
		}

		private void HandleTransitions(TransitionVO[] transitions)
		{
			_btn.interactable = transitions.Length <= 1;
		}
	}
}

