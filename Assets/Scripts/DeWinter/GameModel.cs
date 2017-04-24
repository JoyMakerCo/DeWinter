using System;
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
				DeWinterApp.SendMessage<int>(GameConsts.LIVRE, _livre);
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
			DeWinterApp.Subscribe<RequestAdjustValueVO<int>>(HandleAdjustValue);
		}

		public void Dispose()
		{
			DeWinterApp.Unsubscribe<RequestAdjustValueVO<int>>(HandleAdjustValue);
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
		private ReputationLevel[] ReputationLevels;

		public int PartyInviteImportance
		{
			get {
				return ReputationLevels[ReputationLevel].PartyInviteImportance;
			}
		}

		private void HandleAdjustValue(RequestAdjustValueVO<int> vo)
		{
			switch (vo.Type)
			{
				case GameConsts.LIVRE:
					Livre += vo.Value;
					break;

				case GameConsts.REPUTATION:
					Reputation += vo.Value;
					break;
			}
		}
	}
}