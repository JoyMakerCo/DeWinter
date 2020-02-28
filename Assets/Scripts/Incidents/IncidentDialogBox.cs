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
        private IncidentVO _incident;
        private Image _image;
        private TransitionVO _trans;
		void Awake ()
		{
            _model = AmbitionApp.GetModel<IncidentModel>();
            _image = GetComponent<Image>();
            AmbitionApp.Subscribe<IncidentVO>(IncidentMessages.START_INCIDENT, HandleIncident);
            AmbitionApp.Subscribe<IncidentVO>(IncidentMessages.END_INCIDENT, HandleEndIncident);
            AmbitionApp.Subscribe<TransitionVO[]>(HandleTransitions);
        }

        void OnDestroy ()
		{
            AmbitionApp.Unsubscribe<IncidentVO>(IncidentMessages.START_INCIDENT, HandleIncident);
            AmbitionApp.Unsubscribe<IncidentVO>(IncidentMessages.END_INCIDENT, HandleEndIncident);
            AmbitionApp.Unsubscribe<TransitionVO[]>(HandleTransitions);
        }

        public void OnPointerClick(PointerEventData eventData)
		{
			Next();
        }

		public void Next()
		{
            _image.raycastTarget = false;
            if (_trans != null) AmbitionApp.SendMessage(_trans);
            else AmbitionApp.SendMessage(IncidentMessages.END_INCIDENT, _model.Incident);
            FMODUnity.RuntimeManager.PlayOneShot("event:/One Shot SFX/Mouse_click"); //Literally only ever plays this sound. It will never need to play anything else.
        }

        void HandleIncident(IncidentVO incident) => _incident = incident;
        void HandleEndIncident(IncidentVO incident) => _incident = null;
        void HandleTransitions(TransitionVO[] transitions)
		{
            _trans = transitions.Length > 0 ? transitions[0] : null;
            _image.raycastTarget = transitions.Length <= 1;
            FMODUnity.RuntimeManager.PlayOneShot("event:/One Shot SFX/Mouse_click"); //Literally only ever plays this sound. It will never need to play anything else.
        }
    }
}
