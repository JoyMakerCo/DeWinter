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
            Servant s = GameData.servantDictionary[k];
            if (s.Hired())
            {
                payDayText += "\n" + s.NameAndTitle() + " " + s.Wage().ToString("£" + "#,##0") + "/Week";
                wageTotal += s.Wage();
            }
        }
        payDayText += "\nTotal: " + wageTotal.ToString("£" + "#,##0") + "/Week";
        myText.text = payDayText;
    }
}
