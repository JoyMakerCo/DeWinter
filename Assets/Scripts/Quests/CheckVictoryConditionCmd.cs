using System;
using Core;

namespace Ambition
{
	public class CheckVictoryConditionCmd : ICommand<EndScreenTextController>
	{
		public void Execute (EndScreenTextController script)
		{
			GameModel gameModel = AmbitionApp.GetModel<GameModel>();

			if (GameData.playerVictoryStatus == "Political Victory")
	        {
				script.titleText.text = "You Win!";
				script.bodyText.text = "You ended up on the right side of history. You live a happy and easy life.";
	        } else if (GameData.playerVictoryStatus == "Political Loss")
	        {
				script.titleText.text = "You Lose!";
				script.bodyText.text = "You ended up on the wrong side of history. You're executed as a traitor.";
	        } else if (gameModel.Reputation <= 0)
	        {
				script.titleText.text = "Nobody Likes You!";
				script.bodyText.text = "Your Reputation dropped to 0 and you were cast out of society.";
	        } else if (GameData.playerVictoryStatus == "Financial Loss")
	        {
				script.titleText.text = "You're Broke!";
				script.bodyText.text = "You ran our of Money and friends to give you loans. You die penniless in the streets.";
	        }
		}
	}
}