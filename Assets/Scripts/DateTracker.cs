﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Globalization;

namespace Ambition
{
	public class DateTracker : MonoBehaviour
	{
	    public Text myText;
	    private CalendarModel _cmod;

	    void Start()
	    {
			_cmod = AmbitionApp.GetModel<CalendarModel>();
	        updateDate();
	    }

	    public void updateDate()
	    {
	        myText.text = _cmod.GetDateString();
	    }
	}
}