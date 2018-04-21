using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
	public class ConfidenceTextView : TextMessageView<int>
	{
		private PartyModel _model;
		public ConfidenceTextView() : base(GameConsts.CONFIDENCE) {}

		void Awake()
		{
			_model = AmbitionApp.GetModel<PartyModel>();
		}

		void OnEnable()
		{
			HandleValue(_model.Confidence);
		}

		protected override void HandleValue (int value)
		{
			Text = "Confidence: " + value.ToString("N0") + (_model != null ? ("/" + _model.MaxConfidence.ToString("N0")) : "");
		}
	}
}
