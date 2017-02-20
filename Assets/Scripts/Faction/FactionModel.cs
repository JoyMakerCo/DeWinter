using System;
using System.Collections.Generic;
using Core;
using Newtonsoft.Json;
using Util;

namespace DeWinter
{
	public class FactionModel : DocumentModel, IInitializable
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

		public void Initialize()
		{
			DeWinterApp.Subscribe<AdjustBalanceVO>(HandleFactionReputation);
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

		[JsonProperty("power")]
		public Dictionary<int, string> Power;

		[JsonProperty("allegiance")]
		public Dictionary<int, string> Allegiance;

		public string VictoriousPower
		{
			get
			{
				return _factions["Crown"].Power >= _factions["Revolution"].Power
					? "Crown"
					: "Revolution";
			}
		}

		private void HandleFactionReputation(AdjustBalanceVO vo)
		{
			if (vo.IsRequest && _factions.ContainsKey(vo.Type))
			{
				_factions[vo.Type].playerReputation += (int)vo.Amount;
				vo.IsRequest = false;
				DeWinterApp.SendMessage<AdjustBalanceVO>(vo);
			}
		}
	}
}