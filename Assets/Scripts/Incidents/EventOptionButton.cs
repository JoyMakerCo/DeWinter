using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Ambition;
using Core;

namespace Ambition
{
    public class EventOptionButton : MonoBehaviour
    {
        public Image[] Flags;
        public AmbitionLocalizedText[] Tooltips;
        public IncidentFlagIcon[] Icons;
        public Text Text;

        private int _option;
        private Button _btn;
        private TransitionVO _transition;
        private IncidentVO _incident;

        void Awake()
        {
            _option = gameObject.transform.GetSiblingIndex();
            _btn = gameObject.GetComponent<Button>();
            _btn.onClick.AddListener(OnClick);
            AmbitionApp.Subscribe<TransitionVO[]>(HandleTransitions);
            AmbitionApp.Subscribe<IncidentVO>( IncidentMessages.START_INCIDENT, HandleIncident );
            AmbitionApp.Subscribe(IncidentMessages.END_INCIDENT, HandleEndIncident);
        }

        void OnDestroy()
        {
            AmbitionApp.Unsubscribe<TransitionVO[]>(HandleTransitions);
            AmbitionApp.Unsubscribe<IncidentVO>(IncidentMessages.START_INCIDENT, HandleIncident);
            AmbitionApp.Unsubscribe(IncidentMessages.END_INCIDENT, HandleEndIncident);
            _btn.onClick.RemoveAllListeners();
        }

        private void HandleIncident(IncidentVO incident) => _incident = incident;
        private void HandleEndIncident() => _incident = null;
        private void HandleTransitions(TransitionVO[] transitions)
        {
            _transition = _option < transitions.Length ? transitions[_option] : null;
            bool active = _transition != null;
            if (active && _incident?.LinkData != null)
            {
                int index = Array.IndexOf(_incident.LinkData, _transition);
                Text.text = AmbitionApp.GetString(_incident.LocalizationKey + "link." + index.ToString());
                active = transitions.Length > 1 || !string.IsNullOrWhiteSpace(Text.text);
            }
            this.gameObject.SetActive(active);

            // Warning: Early Out
            if (!active) return;

            IncidentFlag flag;
            for (int i=Flags.Length-1; i>=0; i--)
            {
                active = i < (_transition.Flags?.Length ?? 0);
                Flags[i].gameObject.SetActive(active);
                if (active)
                {
                    flag = _transition.Flags[i];
                    Flags[i].sprite = Array.Find(Icons, f => f.Type == flag.Type).Icon;
                    Flags[i].SetNativeSize();
                    Tooltips[i].Localize(
                        string.IsNullOrWhiteSpace(flag.Phrase) ? flag.Type.ToString() : flag.Phrase,
                        new Dictionary<string, string>() { { "#", flag.Value.ToString() } }
                    );
                }
            }
        }

        private void OnClick() => AmbitionApp.SendMessage(_transition);
    }
}
