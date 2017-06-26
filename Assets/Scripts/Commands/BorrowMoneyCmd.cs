using System;
using Core;

namespace Ambition
{
	public class BorrowMoneyCmd : ICommand
	{
		public void Execute ()
		{
			GameModel model = DeWinterApp.GetModel<GameModel>();
			model.Livre += 200;
			model.Reputation -= 20;
		}
	}
}

