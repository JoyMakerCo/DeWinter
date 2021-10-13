using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class LocalizationSvc : IAppService
    {
        public SystemLanguage Language { get; private set; }
        public SystemLanguage DefaultLanguage = SystemLanguage.Unknown;
        public Font DefaultFont;

        protected Dictionary<string, string> _localizations = new Dictionary<string, string>();
        protected Dictionary<string, Font> _fonts = new Dictionary<string, Font>();

        public SystemLanguage[] Languages { get; private set; }

        public LocalizationSvc()
        {
            Language = UnityEngine.Application.systemLanguage;
        }

        public void SetLanguageOptions(SystemLanguage[] languages)
        {
            if (languages != null) Languages = languages;
        }

        public bool LoadLocFile(SystemLanguage language, string json)
        {
            Dictionary<string, string> result = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            if (result == null) return false;
            Language = language;
            _localizations = result;
            return true;
        }

        public void ClearProxyFonts()
        {
            _fonts.Clear();
            DefaultFont = null;
        }

        public void SetProxyFont(Font proxyFont, Font substituteFont)
        {
            _fonts[proxyFont.name] = substituteFont;
        }

        public Font GetFont(Font proxyFont)
        {
            _fonts.TryGetValue(proxyFont.name, out Font result);
            return result ?? DefaultFont ?? proxyFont;
        }

        public string this[string key]
		{
			get
			{
				string value;
				return _localizations.TryGetValue(key, out value) ? value : null;
			}
		}
            
		public Dictionary<string, string> GetPhrases(string key)
		{
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach(string loc in _localizations.Keys)
            {
                if (loc.StartsWith(key))
                    result[loc] = _localizations[loc];
            }
            System.Linq.Enumerable.OrderBy(result, r => r.Key);
            return result;
		}

		public string GetString(string key)
		{
			string value;
			return _localizations.TryGetValue(key, out value) ? value : null;
		}

		public string GetString(string key, Dictionary<string,string> substitutions)
		{
            if (string.IsNullOrEmpty(key)) return null;

            string value;
			_localizations.TryGetValue(key, out value);
			if (value != null && substitutions != null)
			{				
				foreach(KeyValuePair<string, string> kvp in substitutions)
				{
					value = value.Replace(kvp.Key, kvp.Value);
				}
			}
			return value;
		}

        public void Dispose() => _localizations.Clear();
    }
}
