using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MonthSelector : MonoBehaviour {

    public Text displayMonthText;

    // Update is called once per frame
    void Start()
    {
        ViewCurrentMonth();
    }

    void Update () {
        displayMonthText.text = GameData.calendar.monthList[GameData.displayMonthInt].name;
	}

    public void ViewMonthAhead()
    {
        if (GameData.displayMonthInt < (GameData.gameLengthMonths+GameData.startMonthInt))
        {
            GameData.displayMonthInt++;
        }
    }

    public void ViewMonthBehind()
    {
        if (GameData.displayMonthInt > GameData.startMonthInt)
        {
            GameData.displayMonthInt--;
        }    
    }

    public void ViewCurrentMonth()
    {
        GameData.displayMonthInt = GameData.currentMonth;
    }
}
