using UnityEngine;
using System.Collections;

public class AlterOufitPopUpController : MonoBehaviour {

    public Outfit outfit;

    public void AlterModesty(int amount)
    {
        outfit.Alter("Modesty", amount);
        GameData.moneyCount -= 20;
    }

    public void AlterLuxury(int amount)
    {
        outfit.Alter("Luxury", amount);
        GameData.moneyCount -= 20;
    }
}
