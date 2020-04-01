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
			Debug.LogFormat("AdjustFactionCmd.Execute: {0}", vo.ToString() );
			FactionVO faction = _model[vo.Faction];
			if (faction != null)
			{
				faction.Allegiance = Clamp(faction.Allegiance + vo.Allegiance, -100, 100);
				faction.Power = Clamp(faction.Power + vo.Power, 0, 100);
				faction.Reputation += vo.Reputation;

				FactionUtilities.UpdateFactionStats(faction);
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
