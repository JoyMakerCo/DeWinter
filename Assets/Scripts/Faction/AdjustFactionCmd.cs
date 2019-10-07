using System;
using System.Linq;
using Core;
using UnityEngine;

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

				// TODO get this called when reputation is modded via CommodityVO 
				faction.Level = -1;
				var threshold = -9999;
				//Debug.LogFormat( "Faction rep is now {0}", faction.Reputation );
				for (int i = 0; i < Levels.Length; i++)
				{
					//Debug.LogFormat( "entry {0} requirement {1} threshold {2}", i, Levels[i].Requirement, threshold);
					if ((faction.Reputation >= Levels[i].Requirement) && (Levels[i].Requirement > threshold))
					{	
						//Debug.LogFormat( "Bumping to level {0}", i );
						threshold = Levels[i].Requirement;
						faction.Level = i;
					}
				}

				var levelData = _model.GetFactionLevel( faction.Level );
				faction.LargestAllowableParty = levelData.LargestAllowableParty;
                faction.DeckBonus = levelData.DeckBonus;
				faction.Priority = levelData.Importance;
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
