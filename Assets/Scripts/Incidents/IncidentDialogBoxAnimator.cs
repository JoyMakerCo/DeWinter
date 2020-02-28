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
            AmbitionApp.Subscribe<IncidentVO>(IncidentMessages.START_INCIDENT, HandleStartIncident);
            AmbitionApp.Subscribe(IncidentMessages.END_INCIDENT, HandleEndIncident);
        }

        void OnDestroy ()
		{
            AmbitionApp.Unsubscribe<IncidentVO>(IncidentMessages.START_INCIDENT, HandleStartIncident);
            AmbitionApp.Unsubscribe(IncidentMessages.END_INCIDENT, HandleEndIncident);
        }

        private void HandleStartIncident(IncidentVO incident)
		{
            _animator.SetBool(ACTIVE, true);
		}

        private void HandleEndIncident()
        {
            _animator.SetBool(ACTIVE, false);
        }
    }
}
