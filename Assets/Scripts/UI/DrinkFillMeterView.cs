using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class DrinkFillMeterView : ImageFillMessageView
    {
        private PartyModel _model;
        void OnEnable()
        {
            _model = AmbitionApp.GetModel<PartyModel>();
            AmbitionApp.Subscribe<int>(GameConsts.DRINK, HandleDrink);
        }

        void OnDisable()
        {
            AmbitionApp.Unsubscribe<int>(GameConsts.DRINK, HandleDrink);
        }

        private void HandleDrink(int value)
        {
            base.HandlePercent(((float)value)/(_model.MaxDrinkAmount));
        }
    }
}
