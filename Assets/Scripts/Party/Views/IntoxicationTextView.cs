using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
	public class IntoxicationTextView : TextMessageView<int>
	{
		private PartyModel _model;
		public IntoxicationTextView() : base(GameConsts.INTOXICATION) {}

		override protected void InitValue()
		{
			_model = AmbitionApp.GetModel<PartyModel>();
			HandleValue(AmbitionApp.GetModel<PartyModel>().Intoxication);
		}

		protected override void HandleValue (int value)
		{
			Text = "Intoxication: " + value.ToString("N0") + (_model == null ? "" : ("/" + _model.Party.MaxIntoxication.ToString()));
		}
	}
}
