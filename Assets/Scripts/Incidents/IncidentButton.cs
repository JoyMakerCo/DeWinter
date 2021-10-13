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
        private Font _baseFont;

        private void Awake()
        {
            _baseFont = _text.font;
            _text.font = AmbitionApp.GetService<LocalizationSvc>().GetFont(_baseFont);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            AmbitionApp.SendMessage(IncidentMessages.TRANSITION, _transition);
        }

        public string Text => _text.text;
        public TransitionVO Transition => _transition;

        public void SetTransition(TransitionVO transition, string text)
        {
            _transition = transition;
            _text.text = text;
            if (transition.Flags == null)
            {
                Array.ForEach(Flags, f => gameObject.SetActive(false));
            }
            else
            {
                IncidentFlag flag;
                int len = _transition.Flags.Length;
                Dictionary<string, string> subs = new Dictionary<string, string>();
                for (int i = Flags.Length - 1; i >= 0; i--)
                {
                    bool active = i < len;
                    Flags[i].gameObject.SetActive(active);
                    if (active)
                    {
                        flag = _transition.Flags[i];
                        Flags[i].sprite = Array.Find(Icons, f => f.Type == flag.Type).Icon;
                        Flags[i].SetNativeSize();
                        subs["#"] = flag.Value.ToString();
                        Tooltips[i].text = AmbitionApp.Localize((string.IsNullOrWhiteSpace(flag.Phrase)
                            ? flag.Type.ToString()
                            : flag.Phrase), subs);
                    }
                }
                StartCoroutine(ForceUpdate());
            }
        }

        IEnumerator ForceUpdate()
        {
            yield return new WaitForEndOfFrame();
            foreach(Text tooltip in Tooltips)
            {
                UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(tooltip.transform.parent as RectTransform);
            }
        }
    }
}
