using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ambition
{
	public class IncidentDialogBox : MonoBehaviour, IPointerClickHandler
	{
        TransitionVO _trans;
        IncidentVO _incident;
        int _index;
        bool _interactive;

        public IncidentButton[] _buttons;

		void Awake ()
		{
            AmbitionApp.Subscribe<IncidentVO>(IncidentMessages.START_INCIDENT, HandleIncident);
            AmbitionApp.Subscribe(IncidentMessages.END_INCIDENT, HandleEndIncident);
            AmbitionApp.Subscribe<TransitionVO[]>(HandleTransitions);
        }

        void OnDestroy ()
		{
            AmbitionApp.Unsubscribe<IncidentVO>(IncidentMessages.START_INCIDENT, HandleIncident);
            AmbitionApp.Unsubscribe(IncidentMessages.END_INCIDENT, HandleEndIncident);
            AmbitionApp.Unsubscribe<TransitionVO[]>(HandleTransitions);
        }

        public void OnPointerClick(PointerEventData eventData)
		{
            if (_interactive)
            {
                if (_trans != null) AmbitionApp.SendMessage(_trans);
                else AmbitionApp.SendMessage(IncidentMessages.END_INCIDENT, _incident);
                FMODUnity.RuntimeManager.PlayOneShot("event:/One Shot SFX/Mouse_click"); //Literally only ever plays this sound. It will never need to play anything else.
            }
        }

        void HandleIncident(IncidentVO incident) => _incident = incident;
        void HandleEndIncident() => _incident = null;
        void HandleTransitions(TransitionVO[] transitions)
		{
            int buttonIndex = 0;
            for (int i=0; i<transitions.Length; i++)
            {
                if (transitions[i] != null)
                {
                    _buttons[buttonIndex].SetTransition(transitions[i]);
                    _buttons[buttonIndex].gameObject.SetActive(true);
                    _buttons[buttonIndex].Text = AmbitionApp.Localize(_incident.LocalizationKey + ".link." + transitions[i].index.ToString());
                    buttonIndex++;
                    if (buttonIndex >= _buttons.Length) return;
                }
            }
            while (buttonIndex < _buttons.Length)
            {
                _buttons[buttonIndex++].gameObject.SetActive(false);
            }
            _interactive = transitions.Length <= 1;
            _trans = transitions.Length > 0 ? transitions[0] : null;
            if (transitions.Length == 1 && string.IsNullOrWhiteSpace(_buttons[0].Text))
            {
                _buttons[0].gameObject.SetActive(false);
            }
        }
	}
}
