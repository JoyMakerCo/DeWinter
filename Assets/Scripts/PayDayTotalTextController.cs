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
	    void Awake()
	    {
	        myText = this.GetComponent<Text>();
		}

		void OnEnable()
	   	{
	        AmbitionApp.Subscribe<List<ItemVO>>(HandleInventory);
	    }

	    void OnDisable()
	   	{
			AmbitionApp.Unsubscribe<List<ItemVO>>(HandleInventory);
	   	}

	    // Update is called once per frame
		private void HandleInventory(List<ItemVO> inventory)
	    {
	        string payDayText = "Pay Day Totals:";
	        int wageTotal = 0;
	        List<ItemVO> servants = inventory.FindAll(i=>i.Type == ItemType.Servant);
	        foreach (ItemVO servant in servants)
	        {
                wageTotal += servant.Price;
	        }
	        payDayText += "\nTotal: " + wageTotal.ToString("£" + "#,##0") + "/Week";
	        int payDayTime = 7 - ((int)(AmbitionApp.GetModel<GameModel>().Date.DayOfWeek) % 7);
	        payDayText += "\nNext Pay Day is in " + payDayTime + " Days";
	        myText.text = payDayText;
	    }
	}
}
