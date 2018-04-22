using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class IntoxicationMeterView : ImageFillMessageView
    {
        private PartyModel _model;
        void OnEnable()
        {
            _model = AmbitionApp.GetModel<PartyModel>();
            AmbitionApp.Subscribe<int>(GameConsts.INTOXICATION, HandleIntoxication);
            HandleIntoxication(_model.Intoxication);
        }

        void OnDisable()
        {
            AmbitionApp.Unsubscribe<int>(GameConsts.INTOXICATION, HandleIntoxication);
        }

        private void HandleIntoxication(int value)
        {
            base.HandlePercent(((float)value)/(float)(_model.MaxIntoxication));
        }
    }
}
