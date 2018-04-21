using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
	public class ConfidenceMeterView : ImageFillMessageView<int>
	{
		private PartyModel _model;
		private float _max;

		void Awake()
		{
			ValueID = GameConsts.CONFIDENCE;
			_model = AmbitionApp.GetModel<PartyModel>();
		}

		void OnEnable()
		{
			CalculatePercent(_model.Confidence);
		}

		protected override float CalculatePercent (int value)
		{
			return (float)value/(float)_model.MaxConfidence;
		}
	}
}
