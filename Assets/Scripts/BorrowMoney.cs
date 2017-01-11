using UnityEngine;
using System.Collections;

public class BorrowMoney : MonoBehaviour {

    public void GetTheMoney()
    {
        GameData.moneyCount += 200;
        GameData.reputationCount -= 20;
    }
}
