using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using Ambition;

public class NextStyleTracker : MonoBehaviour {

    private Text myText;

    void Start()
    {
        myText = this.GetComponent<Text>();
        AmbitionApp.Subscribe<DateTime>(HandleCalendarDay);
        HandleCalendarDay(default(DateTime));
    }

    void OnDestroy()
    {
		AmbitionApp.Unsubscribe<DateTime>(HandleCalendarDay);
    }

	private void HandleCalendarDay(DateTime day)
    {
    	ServantModel servantModel = AmbitionApp.GetModel<ServantModel>();
		if (servantModel.Servants.ContainsKey("Seamstress"))
		{
			myText.text = AmbitionApp.GetModel<InventoryModel>().NextStyle;
		}
		else
		{
			myText.text = "Unknown";
		}
    }
}
