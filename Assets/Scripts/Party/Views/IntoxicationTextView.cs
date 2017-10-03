using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
	public class IntoxicationTextView : TextMessageView<int>
	{
		private Core.ModelSvc _models = Core.App.Service<Core.ModelSvc>();
		private PartyModel _model;

		public IntoxicationTextView()
		{
			_model = _models.GetModel<PartyModel>();
			ValueID = GameConsts.INTOXICATION;
		}

		void Start()
		{
			HandleValue(_model.Intoxication);
		}

		protected override void HandleValue (int value)
		{
			Text = "Intoxication: " + value.ToString("N0") + "/" + _model.Party.MaxIntoxication.ToString();
		}
	}
}
