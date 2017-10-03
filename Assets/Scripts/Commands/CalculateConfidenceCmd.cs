using System;
using Core;

namespace Ambition
{
	public class CalculateConfidenceCmd : ICommand
	{
		private const int BASE = 35;

		public void Execute ()
		{
			PartyModel model = AmbitionApp.GetModel<PartyModel>();
			OutfitVO outfit = AmbitionApp.GetModel<GameModel>().Outfit;
			FactionVO faction = AmbitionApp.GetModel<FactionModel>()[model.Party.Faction];

			int outfitScore = 400 - Math.Abs(faction.Modesty - outfit.Modesty) - Math.Abs(faction.Luxury - outfit.Luxury);
			model.Confidence = model.MaxConfidence = model.StartConfidence = BASE + 
				(int)((float)(outfitScore >> 1) * (float)(outfit.Novelty) * 0.01f) +
				faction.ConfidenceBonus +
				AmbitionApp.GetModel<GameModel>().ConfidenceBonus;
		}
	}
}
