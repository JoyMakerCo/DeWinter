using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Ambition
{
	public class PayDayTotalTextController : MonoBehaviour {

	    Text myText;

	    // Use this for initialization
	    void Start()
	    {
	        myText = this.GetComponent<Text>();
	    }

	    // Update is called once per frame
	    void Update()
	    {
	        string payDayText = "Pay Day Totals:";
	        int wageTotal = 0;
	        foreach (ServantVO[] servants in GameData.servantDictionary.Values)
	        {
				ServantVO s = Array.Find(servants, x => x.Hired);
	            if (s != null)
	            {
	                wageTotal += s.Wage;
	            }
	        }
	        payDayText += "\nTotal: " + wageTotal.ToString("£" + "#,##0") + "/Week";
	        int payDayTime = 7 - ((int)(DeWinterApp.GetModel<CalendarModel>().Today.DayOfWeek) % 7);
	        payDayText += "\nNext Pay Day is in " + payDayTime + " Days";
	        myText.text = payDayText;
	    }
	}
}