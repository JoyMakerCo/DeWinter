﻿using System;
using System.Linq;
using Core;
using UnityEngine;

namespace Ambition
{
	public class SetFactionValuesCmd : ICommand<AdjustFactionVO>
	{
		private FactionModel _model = AmbitionApp.GetModel<FactionModel>();

		public void Execute(AdjustFactionVO vo)
		{
			FactionVO faction = _model[vo.Faction];
			if (faction != null)
			{
				faction.Allegiance = Clamp(vo.Allegiance, -100, 100);
				faction.Power = Clamp(vo.Power, 0, 100);
				faction.Reputation = vo.Reputation;

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
