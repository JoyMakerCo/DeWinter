using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
	public class ConfidenceTextView : TextMessageView<int>
	{
		private Core.ModelSvc _models = Core.App.Service<Core.ModelSvc>();
		private PartyModel _model;

		public ConfidenceTextView()
		{
			_model = _models.GetModel<PartyModel>();
			ValueID = GameConsts.CONFIDENCE;
		}

		void Start()
		{
			HandleValue(_model.Confidence);
		}

		protected override void HandleValue (int value)
		{
			Text = "Confidence: " + value.ToString("N0") + "/" + _model.MaxConfidence.ToString("N0");
		}
	}
}
