using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class ConfidenceMeterView : ImageFillMessageView
    {
        private PartyModel _model;
        void OnEnable()
        {
            _model = AmbitionApp.GetModel<PartyModel>();
            AmbitionApp.Subscribe<int>(GameConsts.CONFIDENCE, HandleConfidence);
            HandleConfidence(_model.Confidence);
        }

        void OnDisable()
        {
            AmbitionApp.Unsubscribe<int>(GameConsts.CONFIDENCE, HandleConfidence);
        }

        private void HandleConfidence(int value)
        {
            base.HandlePercent(((float)value)/_model.MaxConfidence);
        }
    }
}
