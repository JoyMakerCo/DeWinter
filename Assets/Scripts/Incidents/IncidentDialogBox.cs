using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ambition
{
    public class IncidentDialogBox : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] Image _hitTarget;
        TransitionVO _trans;
        IncidentVO _incident;
        int _index;

        public IncidentButton[] _buttons;

        void Awake()
        {
            AmbitionApp.Subscribe<IncidentVO>(IncidentMessages.START_INCIDENT, HandleIncident);
            AmbitionApp.Subscribe<TransitionVO[]>(HandleTransitions);
        }

        void OnDestroy()
        {
            AmbitionApp.Unsubscribe<IncidentVO>(IncidentMessages.START_INCIDENT, HandleIncident);
            AmbitionApp.Unsubscribe<TransitionVO[]>(HandleTransitions);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_hitTarget.raycastTarget)
            {
                AmbitionApp.SendMessage(IncidentMessages.TRANSITION, _trans);
                FMODUnity.RuntimeManager.PlayOneShot("event:/One Shot SFX/Mouse_click"); //Literally only ever plays this sound. It will never need to play anything else.
            }
        }
        string Loc(TransitionVO trans) => AmbitionApp.Localize(trans == null ? "" : _incident.ID + ".link." + trans.index.ToString());
        void HandleIncident(IncidentVO incident) => _incident = incident;
        void HandleTransitions(TransitionVO[] transitions)
        {
            int len = transitions.Length;
            bool active;
            _trans = len > 0 ? transitions[0] : null;
            _hitTarget.raycastTarget = (_trans == null || transitions.Length == 1);
            for (int i=_buttons.Length-1; i>=0; --i)
            {
                active = i < len;
                _buttons[i].gameObject.SetActive(active);
                if (active) _buttons[i].SetTransition(transitions[i], Loc(transitions[i]));
            }
            if (transitions.Length == 1)
            {
                active = !string.IsNullOrEmpty(_buttons[0].Text);
                _buttons[0].gameObject.SetActive(active);
            }
        }
    }
}
