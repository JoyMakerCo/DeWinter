using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
	public class IntoxicationMeterView : ImageFillMessageView<int>
	{
		private PartyModel _model;

		void Start()
		{
			_model = AmbitionApp.GetModel<PartyModel>();
			ValueID = GameConsts.INTOXICATION;
			Percent = CalculatePercent(_model.Intoxication);
		}

		protected override float CalculatePercent (int value)
		{
			return (float)value/(float)_model.Party.MaxIntoxication;
		}
	}
}
