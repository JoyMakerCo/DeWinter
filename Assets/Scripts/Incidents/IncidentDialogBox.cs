using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ambition
{
	public class IncidentDialogBox : MonoBehaviour, IPointerClickHandler
	{
        private IncidentModel _model;
		private Image _image;
        private TransitionVO _trans;
		void Awake ()
		{
<<<<<<< Updated upstream
            _model = AmbitionApp.GetModel<IncidentModel>();
            _image = GetComponent<Image>();
			AmbitionApp.Subscribe<TransitionVO[]>(HandleTransitions);
		}
		
		void OnDestroy ()
		{
			AmbitionApp.Unsubscribe<TransitionVO[]>(HandleTransitions);
		}
=======
            AmbitionApp.Subscribe<IncidentVO>(IncidentMessages.START_INCIDENT, HandleIncident);
            AmbitionApp.Subscribe<IncidentVO>(IncidentMessages.END_INCIDENT, HandleEndIncident);
            AmbitionApp.Subscribe<TransitionVO[]>(HandleTransitions);
        }
>>>>>>> Stashed changes

		public void OnPointerClick(PointerEventData eventData)
		{
<<<<<<< Updated upstream
			Next();
=======
            AmbitionApp.Unsubscribe<IncidentVO>(IncidentMessages.START_INCIDENT, HandleIncident);
            AmbitionApp.Unsubscribe<IncidentVO>(IncidentMessages.END_INCIDENT, HandleEndIncident);
            AmbitionApp.Unsubscribe<TransitionVO[]>(HandleTransitions);
>>>>>>> Stashed changes
        }

		public void Next()
		{
<<<<<<< Updated upstream
            _image.raycastTarget = false;
            if (_trans != null) AmbitionApp.SendMessage(_trans);
            else AmbitionApp.SendMessage(IncidentMessages.END_INCIDENT, _model.Incident);
            FMODUnity.RuntimeManager.PlayOneShot("event:/One Shot SFX/Mouse_click"); //Literally only ever plays this sound. It will never need to play anything else.
        }

		private void HandleTransitions(TransitionVO[] transitions)
=======
            if (_interactive)
            {
                if (_trans != null) AmbitionApp.SendMessage(_trans);
                else AmbitionApp.SendMessage(IncidentMessages.END_INCIDENT);
                FMODUnity.RuntimeManager.PlayOneShot("event:/One Shot SFX/Mouse_click"); //Literally only ever plays this sound. It will never need to play anything else.
            }
        }

        void HandleIncident(IncidentVO incident) => _incident = incident;
        void HandleEndIncident(IncidentVO incident) => _incident = null;
        void HandleTransitions(TransitionVO[] transitions)
>>>>>>> Stashed changes
		{
            _trans = transitions.Length > 0 ? transitions[0] : null;
            _image.raycastTarget = transitions.Length <= 1;
		}
	}
}
