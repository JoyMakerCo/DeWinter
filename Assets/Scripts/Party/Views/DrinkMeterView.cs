using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
	public class DrinkMeterView : ImageFillMessageView<int>
	{
		private PartyModel _model;

		void Start()
		{
			_model = AmbitionApp.GetModel<PartyModel>();
			ValueID = GameConsts.DRINK;
			Percent = CalculatePercent(_model.Drink);
		}

		protected override float CalculatePercent (int value)
		{
			return (float)value/(float)_model.MaxDrinkAmount;
		}
	}
}
