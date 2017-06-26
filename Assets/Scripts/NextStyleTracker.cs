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
        DeWinterApp.Subscribe<DateTime>(HandleCalendarDay);
        HandleCalendarDay(default(DateTime));
    }

    void OnDestroy()
    {
		DeWinterApp.Unsubscribe<DateTime>(HandleCalendarDay);
    }

	private void HandleCalendarDay(DateTime day)
    {
    	ServantModel servantModel = DeWinterApp.GetModel<ServantModel>();
		if (servantModel.Servants.ContainsKey("Seamstress"))
		{
			myText.text = DeWinterApp.GetModel<InventoryModel>().NextStyle;
		}
		else
		{
			myText.text = "Unknown";
		}
    }
}
