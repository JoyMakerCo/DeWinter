using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class IncidentDialogBoxAnimator : MonoBehaviour
	{
		private const string ACTIVE = "Active"; //corresponds with Animation Parameter

		private Animator _animator;

		void Awake ()
		{
			_animator = GetComponent<Animator>();
			AmbitionApp.Subscribe<MomentVO>(HandleMoment);
		}
		
		void OnDestroy ()
		{
			AmbitionApp.Unsubscribe<TransitionVO[]>(HandleTransitions);
			AmbitionApp.Unsubscribe<MomentVO>(HandleMoment);
		}

		private void HandleTransitions(TransitionVO[] transitions)
		{
			_animator.SetBool(ACTIVE, transitions != null && transitions.Length > 0);
			AmbitionApp.Unsubscribe<TransitionVO[]>(HandleTransitions);
		}

		private void HandleMoment(MomentVO moment)
		{
			if (moment == null) _animator.SetBool(ACTIVE, false);
			else if (moment.Text != null && moment.Text.Length > 0) _animator.SetBool(ACTIVE, true);
			else AmbitionApp.Subscribe<TransitionVO[]>(HandleTransitions);
		}
	}
}
