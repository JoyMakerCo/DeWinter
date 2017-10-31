using System;
using Core;

namespace Ambition
{
	public class EndGameShunnedCmd : ICommand<ReputationVO>
	{
		public void Execute (ReputationVO rep)
		{
			if (rep.Reputation <= -20)
			{
				AmbitionApp.SendMessage<string>(GameMessages.LOAD_SCENE,"Game_EndScreen");
			}
		}
	}
}
