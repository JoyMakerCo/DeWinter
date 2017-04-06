using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Globalization;

namespace DeWinter
{
	public class DateTracker : MonoBehaviour
	{
	    public Text myText;
	    private CalendarModel _cmod;

	    void Start()
	    {
			_cmod = DeWinterApp.GetModel<CalendarModel>();
	        updateDate();
	    }

	    public void updateDate()
	    {
	        myText.text = _cmod.GetDateString();
	    }
	}
}