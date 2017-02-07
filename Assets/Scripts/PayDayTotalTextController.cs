using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
        foreach (string k in GameData.servantDictionary.Keys)
        {
            ServantVO s = GameData.servantDictionary[k];
            if (s.Hired())
            {
                wageTotal += s.Wage();
            }
        }
        payDayText += "\nTotal: " + wageTotal.ToString("£" + "#,##0") + "/Week";
        int payDayTime = 7 - ((GameData.currentDay + 1) % 7); //Servants get paid every 7 days. The +1 is to throw off the fact that days are zero-indexed
        payDayText += "\nNext Pay Day is in " + payDayTime + " Days";
        myText.text = payDayText;
    }
}
