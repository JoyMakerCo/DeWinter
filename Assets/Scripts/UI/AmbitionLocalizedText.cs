using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Core;

namespace Ambition
{
    // Component for localizing a Text UI element on the same gameobject
    // Localization will default to the specified key if one exists;
    // Otherwise, the stored generated key for the element will be used instead.
    public class AmbitionLocalizedText : MonoBehaviour
    {
        [Serializable]
        public struct Substitution
        {
            public string token;
            public string text;
        }

        [Header("Check to use the current value in the Text object as the localization key.")]
        public bool UseTextAsKey = true;
        public Substitution[] Substitutions;

        [SerializeField, HideInInspector]
        private string _localizationKey;
        private Text _text;
        private Dictionary<string, string> _substitutions;

#if UNITY_EDITOR
        private LocalizationConfig _config;
#endif

        private void Awake()
        {
            _text = GetComponent<Text>();
            _substitutions = new Dictionary<string, string>();
            foreach (Substitution sub in Substitutions)
            {
                _substitutions[sub.token] = sub.text;
            }
#if DEBUG
            if (_text == null)
            {
                Debug.LogError("Error: No text object to localize on " + gameObject.name);
            }
#endif
#if UNITY_EDITOR
            _config = _config ?? Resources.Load<LocalizationConfig>("Localization Config");
            string key = UseTextAsKey
                ? _text.text
                : _config.GenerateLocalizationKey(this);

            _config.MoveKey(_localizationKey, key);
            _localizationKey = key;
#endif
            Localize(_localizationKey, _substitutions);
        }

        public string Localize(string key, Dictionary<string,string> substitutions=null)
        {
            string str;
            if (substitutions == null)
            {
               str = AmbitionApp.GetString(key);
            }
            else
            {
                str = AmbitionApp.GetString(key, substitutions);
            }
            return _text.text = str ?? _text.text;
        }

#if UNITY_EDITOR
        private void OnDisable()
        {
            if (_config == null)
            {
                Debug.LogError("null _config in LocalizedText on " + gameObject.name);
            }
            _config?.Post(_localizationKey, _text?.text);
        }

        private void OnDestroy()
        {
            _config = null;
        }
#endif
    }
}
