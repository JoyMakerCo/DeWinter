using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Core
{
	public class LocalizationSvc : IAppService
	{
		public string LanguageCode { get; private set; }
        protected Dictionary<string, string> _localizations;

		public LocalizationSvc()
		{
			LanguageCode = UnityEngine.Application.systemLanguage.ToString();
			_localizations = new Dictionary<string, string>();
		}

        public bool LoadLocFile(string langageCode, string json)
        {
            Dictionary<string, string> result = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            if (result != null)
            {
                LanguageCode = langageCode;
                _localizations = result;
                return true;
            }
            return false;
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
