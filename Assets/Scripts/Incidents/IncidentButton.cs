using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Ambition;
using Core;

namespace Ambition
{
    [Serializable]
    public class IncidentButton : MonoBehaviour, IPointerClickHandler
    {
        public Image[] Flags;
        public Text[] Tooltips;
        public IncidentFlagIcon[] Icons;
        public Text _text;

        private TransitionVO _transition;

        public void OnPointerClick(PointerEventData eventData)
        {
            AmbitionApp.SendMessage(IncidentMessages.TRANSITION, _transition);
        }

        public string Text => _text.text;

        public void SetTransition(TransitionVO transition, string text)
        {
            IncidentFlag flag;

            _transition = transition;
            _text.text = text;
            for (int i=Flags.Length-1; i>=0; i--)
            {
                bool active = i < (_transition.Flags?.Length ?? 0);
                Flags[i].gameObject.SetActive(active);
                if (active)
                {
                    flag = _transition.Flags[i];
                    Flags[i].sprite = Array.Find(Icons, f => f.Type == flag.Type).Icon;
                    Flags[i].SetNativeSize();
                    Tooltips[i].text = AmbitionApp.GetString(
                        string.IsNullOrWhiteSpace(flag.Phrase) ? flag.Type.ToString() : flag.Phrase,
                        new Dictionary<string, string>() { { "#", flag.Value.ToString() } }
                    );
                }
            }
        }
    }
}
