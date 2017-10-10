using System;
using System.Collections.Generic;
using Core;
using Newtonsoft.Json;
using Util;

namespace Ambition
{
	public class GameModel : DocumentModel
	{
		private PlayerReputationVO _reputation;
		private int _livre;

		public string Allegiance;

		[JsonProperty("livre")]
		public int Livre
		{
			get { return _livre; }
			set
			{
				_livre = value;
				AmbitionApp.SendMessage<int>(GameConsts.LIVRE, _livre);
			}
		}

		[JsonProperty("reputation")]
		public int Reputation
		{
			get { return _reputation.Reputation; }
			set
			{
				_reputation.Reputation = value;
				if (ReputationLevels != null)
				{
					int numLevels = ReputationLevels.Length;
					for(int i=0; i < numLevels; i++)
					{
						if (_reputation.Reputation <= ReputationLevels[i].Reputation)
						{
							if (_reputation.Level != i+1)
							{
								_reputation.Level = i+1;
								_reputation.ReputationMax = ReputationLevels[i].Reputation;
								_reputation.Title = ReputationLevels[i].Title;
							}
							AmbitionApp.SendMessage<PlayerReputationVO>(_reputation);
							return;
						}
					}
				}
			}
		}

		public int ConfidenceBonus
		{
			get { return ReputationLevels[Level].Confidence; }
		}

		public int Level
		{
			get { return _reputation.Level; }
		}

		public GameModel() : base("GameData") {}

		// TODO: Localization model would be handy here
		public string BenefitsList
		{
			get
			{
				string str = "";
				for (int i=_reputation.Level-1; i>=0; i--)
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
				return ReputationLevels[Level].PartyInviteImportance;
			}
		}

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
