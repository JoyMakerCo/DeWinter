using System;
using System.Collections.Generic;
using Core;
using Newtonsoft.Json;
using Util;

namespace Ambition
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
				DeWinterApp.SendMessage<AdjustValueVO>(new AdjustValueVO(GameConsts.LIVRE, _livre, false));
			}
		}

		[JsonProperty("reputation")]
		public int Reputation
		{
			get { return _reputation; }
			set
			{
				_reputation = value;
				if (ReputationLevels != null)
				{
					for(_level=0; _level < ReputationLevels.Length; _level++)
					{
						if (_reputation < ReputationLevels[_level].Reputation)
						{
							PlayerReputationVO msg = new PlayerReputationVO(_reputation, _level);
							DeWinterApp.SendMessage<PlayerReputationVO>(msg);
							return;
						}
					}
				}
			}
		}

		public int ConfidenceBonus
		{
			get { return ReputationLevels[ReputationLevel].Confidence; }
		}

		public int ReputationLevel
		{
			get { return _level+1; }
		}

		public GameModel() : base("GameData") {}

		public void Initialize()
		{
			DeWinterApp.Subscribe<AdjustValueVO>(HandleAdjustBalance);
		}

		public void Dispose()
		{
			DeWinterApp.Unsubscribe<AdjustValueVO>(HandleAdjustBalance);
		}

		// TODO: Localization model would be handy here
		public string BenefitsList
		{
			get
			{
				string str = "";
				for (int i=_level; i>=0; i--)
				{
					str += ReputationLevels[i].Description + "\n";
				}
				return str;
			}
		}

		[JsonProperty("reputationLevels")]
		public ReputationLevel[] ReputationLevels;

		public int PartyInviteImportance
		{
			get {
				return ReputationLevels[ReputationLevel].PartyInviteImportance;
			}
		}

		private void HandleAdjustBalance(AdjustValueVO msg)
		{
			if (msg.IsRequest)
			{
				switch (msg.Type)
				{
					case GameConsts.LIVRE:
						Livre += (int)(msg.Amount);
						msg.IsRequest = false;
						DeWinterApp.SendMessage<AdjustValueVO>(msg);
						break;

					case GameConsts.REPUTATION:
						Reputation += (int)(msg.Amount);
						msg.IsRequest = false;
						DeWinterApp.SendMessage<AdjustValueVO>(msg);
						break;
				}
			}
		}
	}
}