using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
	    }

	    void OnDestroy()
	    {
			DeWinterApp.Unsubscribe<AdjustValueVO>(HandleBalanceUpdate);
	    }

		private void HandleBalanceUpdate(AdjustValueVO vo)
		{
			if (!vo.IsRequest && vo.Type == GameConsts.LIVRE)
			{
				myText.text = vo.Amount.ToString("£" + "#,##0");

				// TODO: Commandify Out of funds
				//If your Money drops to 0 or below then you lose (for now)
		        if (vo.Amount <= 0d)
		        {
		            screenFader.gameObject.SendMessage("CreateOutOfMoneyModal");
		        }
		    }
		}
	}
}