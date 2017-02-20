using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DeWinter;

public class NextStyleTracker : MonoBehaviour {

    private Text myText;

    void Start()
    {
        myText = this.GetComponent<Text>();
        DeWinterApp.Subscribe<CalendarDayVO>(CalendarConsts.DAY_START, HandleCalendarDay);
        HandleCalendarDay();
    }

    void OnDestroy()
    {
		DeWinterApp.Unsubscribe(CalendarConsts.DAY_START, HandleCalendarDay);
    }

	private void HandleCalendarDay()
    {
    	ServantModel servantModel = DeWinterApp.GetModel<ServantModel>();
		if (servantModel.Servants.ContainsKey("Seamstress"))
		{
			myText.text = DeWinterApp.GetModel<InventoryModel>().nextStyle;
		}
		else
		{
			myText.text = "Unknown";
		}
    }
}
