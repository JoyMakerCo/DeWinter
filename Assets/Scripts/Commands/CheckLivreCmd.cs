using System;
using Core;

namespace Ambition
{
	public class CheckLivreCmd : ICommand<int>
	{
		public void Execute (int livre)
		{
			if (livre <= 0)
			{
				AmbitionApp.OpenDialog("OutOfMoneyPopUpModal");
			}
		}
	}
}