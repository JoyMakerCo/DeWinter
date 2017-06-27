using System;
using System.Collections.Generic;
using Core;
using Newtonsoft.Json;
using Util;

namespace Ambition
{
	public class FactionModel : DocumentModel, IInitializable, IDisposable
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
			AmbitionApp.Subscribe<AdjustFactionVO>(HandleAdjustFaction);
		}

		public void Dispose()
		{
			AmbitionApp.Unsubscribe<AdjustFactionVO>(HandleAdjustFaction);
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
				return _factions["Crown"].Power >= _factions["Third Estate"].Power
					? "Crown"
					: "Third Estate";
			}
		}

		private void HandleAdjustFaction(AdjustFactionVO vo)
		{
			FactionVO faction;
			if (Factions.TryGetValue(vo.Faction, out faction))
			{
				Factions[vo.Faction].Allegiance += vo.Allegiance;
				Factions[vo.Faction].Power += vo.Power;
				Factions[vo.Faction].playerReputation += vo.Reputation;
				AmbitionApp.SendMessage<FactionVO>(Factions[vo.Faction]);
			}
		}
	}
}