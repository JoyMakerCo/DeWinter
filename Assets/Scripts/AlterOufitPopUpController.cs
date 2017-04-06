using UnityEngine;
using System.Collections;
using DeWinter;

public class AlterOufitPopUpController : MonoBehaviour {

    public Outfit outfit;

    public void AlterModesty(int amount)
    {
        outfit.Alter("Modesty", amount);
        DeWinterApp.GetModel<GameModel>().Livre -= 20;
    }

    public void AlterLuxury(int amount)
    {
        outfit.Alter("Luxury", amount);
		DeWinterApp.GetModel<GameModel>().Livre -= 20;
    }
}
