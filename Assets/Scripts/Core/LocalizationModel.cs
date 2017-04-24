using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Core
{
	public class LocalizationModel : IModel
	{
		public const string LOCALIZATIONS_DIRECTORY = "Localization/";
		public const string DEFAULT_CONFIG = "Default";

		protected string _language;
		protected Dictionary<string, string> _localizations;

		public LocalizationModel ()
		{
			_language = Application.systemLanguage.ToString();
			_localizations = new Dictionary<string, string>();

			TextAsset file = Resources.Load<TextAsset>(LOCALIZATIONS_DIRECTORY + _language);
			if (file == null)
			{
				_language = DEFAULT_CONFIG;
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

		public string GetString(string key)
		{
			string value;
			return _localizations.TryGetValue(key, out value) ? value : null;
		}

		public string GetString(string key, Dictionary<string,string> substitutions)
		{
			string value;
			_localizations.TryGetValue(key, out value);
			if (value != null)
			{
				foreach(KeyValuePair<string, string> kvp in substitutions)
				{
					value = value.Replace(kvp.Key, kvp.Value);
				}
			}
			return value;
		}
	}
}