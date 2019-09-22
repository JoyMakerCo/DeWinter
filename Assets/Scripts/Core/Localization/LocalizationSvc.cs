using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using System.Linq;

namespace Core
{
	public class LocalizationSvc : IAppService
	{
		public const string LOCALIZATIONS_DIRECTORY = "Localization/";
		public const string DEFAULT_CONFIG = "Default";

		public string LanguageCode;
		protected Dictionary<string, string> _localizations;

		public LocalizationSvc ()
		{
			LanguageCode = UnityEngine.Application.systemLanguage.ToString();
			_localizations = new Dictionary<string, string>();

			TextAsset file = Resources.Load<TextAsset>(LOCALIZATIONS_DIRECTORY + LanguageCode);
			if (file == null)
			{
				LanguageCode = DEFAULT_CONFIG;
				file = Resources.Load<TextAsset>(LOCALIZATIONS_DIRECTORY + DEFAULT_CONFIG);
			}
			if (file != null)
			{
				JsonConvert.PopulateObject(file.text, _localizations);
			}
		}

		public string this[string key]
		{
			get
			{
				string value;
				return _localizations.TryGetValue(key, out value) ? value : null;
			}
		}

		public string[] GetList(string key)
		{
			return _localizations.Where(k=>k.Key.StartsWith(key)).Select(v=>v.Value).ToArray();
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
