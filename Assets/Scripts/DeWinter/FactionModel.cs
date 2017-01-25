using System;
using System.Collections.Generic;
using Core;
using Newtonsoft.Json;

namespace DeWinter
{
	public class FactionModel : DocumentModel
	{
		private Dictionary<string, FactionVO> _factions;

		public FactionModel () : base("Factions") {}


		[JsonProperty("Factions")]
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
			set
			{
				_factions[faction] = value;
			}
			get
			{
				return _factions[faction];
			}
		}
	}
}