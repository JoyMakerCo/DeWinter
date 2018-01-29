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
		ServantModel model = AmbitionApp.GetModel<ServantModel>();
		ServantVO seamstress;
		if (model.Servants.TryGetValue(ServantConsts.CLOTHIER, out seamstress) && seamstress.Type == ServantConsts.SEAMSTRESS)
		{
			myText.text = AmbitionApp.GetModel<InventoryModel>().NextStyle;
		}
		else
		{
			myText.text = "Unknown";
		}
    }
}
