﻿using System;
using System.Collections.Generic;
using Core;
using Newtonsoft.Json;
using Util;

namespace DeWinter
{
	public class GameModel : DocumentModel, IInitializable, IDisposable
	{
		private int _reputation;
		private int _livre;
		private int _level;

		public string Allegiance;

		[JsonProperty("livre")]
		public int Livre
		{
			get { return _livre; }
			set
			{
				_livre = value;
				DeWinterApp.SendMessage<AdjustBalanceVO>(new AdjustBalanceVO(BalanceTypes.LIVRE, _livre, false));
			}
		}

		[JsonProperty("reputation")]
		public int Reputation
		{
			get { return _reputation; }
			set
			{
				_reputation = value;
				if (_reputation < 0)
					_reputation = 0;

				_level=_reputationLevels.Length-1;
				while (_level>=0 && _reputation >= _reputationLevels[_level].Reputation)
					_level--;
				PlayerReputationVO msg = new PlayerReputationVO(_reputation, _level+1);
				DeWinterApp.SendMessage<PlayerReputationVO>(msg);
			}
		}

		public int ConfidenceBonus
		{
			get { return _reputationLevels[ReputationLevel].Confidence; }
		}

		public int ReputationLevel
		{
			get { return _level+1; }
		}

		public GameModel() : base("GameData") {}

		public void Initialize()
		{
			DeWinterApp.Subscribe<AdjustBalanceVO>(HandleAdjustBalance);
		}

		public void Dispose()
		{
			DeWinterApp.Unsubscribe<AdjustBalanceVO>(HandleAdjustBalance);
		}

		// TODO: Localization model would be handy here
		public string BenefitsList
		{
			get
			{
				string str = "";
				for (int i=_level; i>=0; i--)
				{
					str += _reputationLevels[i].Description + "\n";
				}
				return str;
			}
		}

		[JsonProperty("reputationLevels")]
		private FactionReputationLevel[] _reputationLevels;

		public int PartyInviteImportance
		{
			get {
				return _reputationLevels[ReputationLevel].PartyInviteImportance;
			}
		}

		private void HandleAdjustBalance(AdjustBalanceVO msg)
		{
			if (msg.IsRequest)
			{
				switch (msg.Type)
				{
					case BalanceTypes.LIVRE:
						Livre += (int)(msg.Amount);
						msg.IsRequest = false;
						DeWinterApp.SendMessage<AdjustBalanceVO>(msg);
						break;

					case BalanceTypes.REPUTATION:
						Reputation += (int)(msg.Amount);
						msg.IsRequest = false;
						DeWinterApp.SendMessage<AdjustBalanceVO>(msg);
						break;
				}
			}
		}
	}
}