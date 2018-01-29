using UnityEngine;
using System.Collections;
using Ambition;

public class AlterOufitPopUpController : MonoBehaviour {

    public OutfitVO outfit;

    public void AlterModesty(int amount)
    {
        outfit.Alter("Modesty", amount);
        AmbitionApp.GetModel<GameModel>().Livre -= 20;
    }

    public void AlterLuxury(int amount)
    {
        outfit.Alter("Luxury", amount);
		AmbitionApp.GetModel<GameModel>().Livre -= 20;
    }
}
