using System;
using System.Collections.Generic;
using Core;
using Newtonsoft.Json;

namespace DeWinter
{
	public class FactionModel : DocumentModel
	{
		private Dictionary<string, FactionVO> _factions;

		public FactionModel () : base("FactionData") {}

		[JsonProperty("factions")]
		public FactionVO[] Factions
		{
			set {
				FactionVO factions;
				_factions = new Dictionary<string, FactionVO>();
				foreach (FactionVO faction in value)
				{
					_factions[faction.Name] = faction;
				}
				int i=0;
			}
		}

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