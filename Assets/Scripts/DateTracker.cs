using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using System.Globalization;

namespace Ambition
{
	public class DateTracker : MonoBehaviour
	{
	    private Text _text;

	    void Awake()
	    {
			_text = GetComponent<Text>();
			AmbitionApp.Subscribe<DateTime>(HandleDate);
			HandleDate(AmbitionApp.GetModel<GameModel>().Date);
	    }

	    void OnDestroy()
	    {
			AmbitionApp.Unsubscribe<DateTime>(HandleDate);
	    }

		public void HandleDate(DateTime date)
	    {
            _text.text = AmbitionApp.GetModel<LocalizationModel>().Date;
        }
	}
}
