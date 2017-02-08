using System;
using System.Collections.Generic;
using Core;
using Newtonsoft.Json;

namespace DeWinter
{
	public class FactionModel : DocumentModel
	{
		private Dictionary<string, FactionVO> _factions;

		[JsonProperty("factions")]
		public Dictionary<string, FactionVO> Factions
		{
			get { return _factions; }
			private set {
				_factions = value;
				foreach (KeyValuePair<string,FactionVO> kvp in _factions)
				{
					_factions[kvp.Key].Name = kvp.Key;
				}
			}
		}

		public FactionModel () : base("FactionData") {}

		public FactionVO this[string faction]
		{
			get
			{
				return _factions[faction];
			}
		}

		[JsonProperty("preference")]

		public Dictionary<string, string[]> Preference;
	}
}