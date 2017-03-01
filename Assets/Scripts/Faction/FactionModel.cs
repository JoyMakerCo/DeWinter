﻿using System;
using System.Collections.Generic;
using Core;
using Newtonsoft.Json;
using Util;

namespace DeWinter
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
			DeWinterApp.Subscribe<AdjustValueVO>(HandleFactionReputation);
			DeWinterApp.Subscribe<AdjustValueVO>(FactionConsts.ADJUST_FACTION_ALLEGIANCE, HandleFactionAllegiance);
			DeWinterApp.Subscribe<AdjustValueVO>(FactionConsts.ADJUST_FACTION_POWER, HandleFactionPower);
		}

		public void Dispose()
		{
			DeWinterApp.Unsubscribe<AdjustValueVO>(HandleFactionReputation);
			DeWinterApp.Unsubscribe<AdjustValueVO>(FactionConsts.ADJUST_FACTION_ALLEGIANCE, HandleFactionAllegiance);
			DeWinterApp.Unsubscribe<AdjustValueVO>(FactionConsts.ADJUST_FACTION_POWER, HandleFactionPower);
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

		private void HandleFactionReputation(AdjustValueVO vo)
		{
			if (vo.IsRequest && _factions.ContainsKey(vo.Type))
			{
				_factions[vo.Type].playerReputation += (int)vo.Amount;
				vo.IsRequest = false;
				DeWinterApp.SendMessage<AdjustValueVO>(vo);
			}
		}

		private void HandleFactionAllegiance(AdjustValueVO vo)
		{
			if (vo.IsRequest && _factions.ContainsKey(vo.Type))
			{
				_factions[vo.Type].Allegiance += (int)vo.Amount;
				vo.IsRequest = false;
				DeWinterApp.SendMessage<AdjustValueVO>(FactionConsts.ADJUST_FACTION_ALLEGIANCE, vo);
			}
		}

		private void HandleFactionPower(AdjustValueVO vo)
		{
			if (vo.IsRequest && _factions.ContainsKey(vo.Type))
			{
				_factions[vo.Type].Power += (int)vo.Amount;
				vo.IsRequest = false;
				DeWinterApp.SendMessage<AdjustValueVO>(FactionConsts.ADJUST_FACTION_POWER, vo);
			}
		}
	}
}