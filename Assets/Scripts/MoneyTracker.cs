using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MoneyTracker : MonoBehaviour {
    private Text myText;
    public GameObject screenFader;

    // Use this for initialization
    void Start()
    {
        myText = GetComponent<Text>();
        //Is it Pay Day?
        if ((GameData.currentDay+1) % 7 == 0) //Servants get paid every 7 days. The +1 is to throw off the fact that days are zero-indexed
        {
            //The Actual Transaction
            foreach (string k in GameData.servantDictionary.Keys)
            {
                Servant s = GameData.servantDictionary[k];
                if (s.Hired())
                {
                    GameData.moneyCount -= s.Wage();
                }
            }
            //Pop Up Window to explain the Transaction        
            screenFader.gameObject.SendMessage("CreatePayDayPopUp");
        }
    }

    void Update()
    {
        updateMoney();
    }

    public void updateMoney()
    {
        myText.text = GameData.moneyCount.ToString("£" + "#,##0");
        VictoryOrDefeatCheck();
    }

    void VictoryOrDefeatCheck()
    {
        //If your Money drops to 0 or below then you lose (for now)
        if (GameData.moneyCount < 0)
        {
            screenFader.gameObject.SendMessage("CreateOutOfMoneyModal");
        }
    }
}
