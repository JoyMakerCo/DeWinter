using System;
using Core;

namespace Ambition
{
	public class AdjustFactionCmd : ICommand<AdjustFactionVO>
	{
		private FactionModel _model = AmbitionApp.GetModel<FactionModel>();

		public void Execute(AdjustFactionVO vo)
		{
			FactionVO faction = _model[vo.Faction];
			if (faction != null)
			{
				FactionLevelVO[] Levels = _model.Levels;
				faction.Allegiance = Clamp(faction.Allegiance + vo.Allegiance, -100, 100);
				faction.Power = Clamp(faction.Power + vo.Power, 0, 100);
				faction.Reputation += vo.Reputation;
				faction.Level = Levels.Length-1;
				while (faction.Reputation >= Levels[faction.Level].Requirement)
					faction.Level--;
				faction.LargestAllowableParty = Levels[faction.Level].LargestAllowableParty;
				faction.ConfidenceBonus = Levels[faction.Level].Confidence;
				faction.Priority = Levels[faction.Level].Importance;
				AmbitionApp.SendMessage<FactionVO>(faction);
			}
		}

		private int Clamp(int value, int min, int max)
		{
			return (max < value) 
				? max
				: (min > value)
				? min
				: value;
		}
	}
}
