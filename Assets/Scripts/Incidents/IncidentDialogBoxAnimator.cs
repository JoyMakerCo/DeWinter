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
            AmbitionApp.Subscribe<IncidentVO>(IncidentMessages.END_INCIDENT, HandleEndIncident);
        }

        void OnDestroy ()
		{
            AmbitionApp.Unsubscribe<IncidentVO>(IncidentMessages.END_INCIDENT, HandleEndIncident);
            AmbitionApp.Unsubscribe<MomentVO>(HandleMoment);
		}

		private void HandleMoment(MomentVO moment)
		{
            _animator.SetBool(ACTIVE, moment != null);
		}

        private void HandleEndIncident(IncidentVO incident)
        {
            _animator.SetBool(ACTIVE, false);
        }
    }
}
