using System;
using System.Collections.Generic;
using Core;
using Newtonsoft.Json;
using Util;

namespace Ambition
{
	public class GameModel : DocumentModel
	{
		private ReputationVO _reputation;

		public string Allegiance;

		public string PlayerName = "Yvette DeWinter";

		[JsonProperty("livre")]
		private int _livre;
		public int Livre
		{
			get { return _livre; }
			set
			{
				_livre = value;
				AmbitionApp.SendMessage<int>(GameConsts.LIVRE, _livre);
			}
		}

		[JsonProperty("reputation", Order = 10)]
		public int Reputation
		{
			get { return _reputation.Reputation; }
			set
			{
				int level = _reputation.Level = Array.FindIndex(_levels, r => r > value);
				bool lowest = level == 0;
				_reputation.Reputation = lowest ? value - _levels[level-1] : value;
				_reputation.ReputationMax = lowest ? _levels[level] : _levels[level] - _levels[level-1];
				AmbitionApp.SendMessage<ReputationVO>(_reputation);
			}
		}

		[JsonProperty("confidence")]
		private int[] _confidence;
		public int ConfidenceBonus
		{
			get { return _confidence[Level]; }
		}

		[JsonProperty("vip")]
		private int[] _vip;
		public int PartyInviteImportance
		{
			get { return _vip[Level]; }
		}

		public int Level
		{
			get { return _reputation.Level; }
		}

		public GameModel() : base("GameData") {}

		[JsonProperty("levels")]
		private int[] _levels;

		private OutfitVO _outfit;
		public OutfitVO Outfit
		{
			get { return _outfit; }
			set {
				_outfit = value;
				AmbitionApp.SendMessage<OutfitVO>(_outfit);
			}
		}

		public OutfitVO LastOutfit;
	}
}
