using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace DeWinter
{
	public class EndScreenTextController : MonoBehaviour {

	    public Text titleText;
	    public Text bodyText;

	    void Start()
	    {
			GameModel model = DeWinterApp.GetModel<GameModel>();
			if (model.Reputation < 0)
			{
				titleText.text = "Nobody Likes You!";
	            bodyText.text = "Your Reputation dropped to 0 and you were cast out of society.";
	        }
			else if (model.Livre <= 0)
			{
	            titleText.text = "You're Broke!";
	            bodyText.text = "You ran our of Money and friends to give you loans. You die penniless in the streets.";
	        }
			else if (model.Allegiance != DeWinterApp.GetModel<FactionModel>().VictoriousPower)
			{
				titleText.text = "You Lose!";
	            bodyText.text = "You ended up on the wrong side of history. You're executed as a traitor.";
			}
	        else
	        {
	            titleText.text = "You Win!";
	            bodyText.text = "You ended up on the right side of history. You live a happy and easy life.";
	        }
	    }
	}
}