using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndScreenTextController : MonoBehaviour {

    public Text titleText;
    public Text bodyText;

    void Start()
    {
        if (GameData.playerVictoryStatus == "Political Victory")
        {
            titleText.text = "You Win!";
            bodyText.text = "You ended up on the right side of history. You live a happy and easy life.";
        } else if (GameData.playerVictoryStatus == "Political Loss")
        {
            titleText.text = "You Lose!";
            bodyText.text = "You ended up on the wrong side of history. You're executed as a traitor.";
        } else if (GameData.playerVictoryStatus == "Reputation Loss")
        {
            titleText.text = "Nobody Likes You!";
            bodyText.text = "Your Reputation dropped to 0 and you were cast out of society.";
        } else if (GameData.playerVictoryStatus == "Financial Loss")
        {
            titleText.text = "You're Broke!";
            bodyText.text = "You ran our of Money and friends to give you loans. You die penniless in the streets.";
        } else
        {
            titleText.text = "Something Went Wrong!";
            bodyText.text = "One of the variables (probably GameData.playerVictoryStatus) wasn't assigned correctly. Fix it!";
        }
    }
}
