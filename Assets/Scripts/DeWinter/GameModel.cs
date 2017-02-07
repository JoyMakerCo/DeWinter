using System;
using System.Collections.Generic;
using Core;
using Newtonsoft.Json;

namespace DeWinter
{
	public class GameModel : DocumentModel
	{
		[JsonProperty("livre")]
		public int Livre;

		[JsonProperty("reputation")]
		public int Reputation
		{
			get { return _reputation; }
			set
			{
				_reputation = value;
				if (_reputation < 0)
					_reputation = 0;
				PlayerReputationVO msg = new PlayerReputationVO(_reputation, ReputationLevel);
				DeWinterApp.SendMessage<PlayerReputationVO>(msg);
			}
		}

		public int ReputationLevel
		{
			get
			{
				for(int i=_reputationLevels.Length-1; i>=0; i--)
				{
					if (_reputation >= _reputationLevels[i])
					{
						return i+1;
					}
				}
				return 1;
			}
		}

		public string Allegiance;

		public Party CurrentParty;

		public GameModel() : base("GameData") {}

		[JsonProperty("reputationLevels")]
		private int[] _reputationLevels;

		private int _reputation;
	}
}