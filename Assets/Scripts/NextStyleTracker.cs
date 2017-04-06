using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DeWinter;

public class NextStyleTracker : MonoBehaviour {

    private Text myText;

    void Start()
    {
        myText = this.GetComponent<Text>();
        DeWinterApp.Subscribe<CalendarDayVO>(HandleCalendarDay);
        HandleCalendarDay(null);
    }

    void OnDestroy()
    {
		DeWinterApp.Unsubscribe<CalendarDayVO>(HandleCalendarDay);
    }

	private void HandleCalendarDay(CalendarDayVO day)
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
