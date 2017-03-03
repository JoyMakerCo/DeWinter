using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace DeWinter
{
	public class MoneyTracker : MonoBehaviour
	{
	    private Text myText;
	    public GameObject screenFader;

	    // Use this for initialization
	    void Start()
	    {
	        myText = GetComponent<Text>();
			DeWinterApp.Subscribe<AdjustValueVO>(HandleBalanceUpdate);
			DeWinterApp.Subscribe<CalendarDayVO>(HandleDateUpdate);
	    }

		private void HandleBalanceUpdate(AdjustValueVO vo)
		{
			if (!vo.IsRequest && vo.Type == GameConsts.LIVRE)
			{
				myText.text = vo.Amount.ToString("£" + "#,##0");

// TODO: This check should be in a command.
				//If your Money drops to 0 or below then you lose (for now)
		        if (vo.Amount <= 0d)
		        {
		            screenFader.gameObject.SendMessage("CreateOutOfMoneyModal");
		        }
		    }
		}

// TODO: Use Dialog System via PayDayCmd.cs
		private void HandleDateUpdate(CalendarDayVO day)
		{
			if (day.Day%7 == 0)
				screenFader.gameObject.SendMessage("CreatePayDayPopUp");
		}
	}
}