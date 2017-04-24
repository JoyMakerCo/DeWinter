using System;
using Core;

namespace DeWinter
{
	public class CheckLivreCmd : ICommand<int>
	{
		public void Execute (int livre)
		{
			if (livre <= 0)
			{
				DeWinterApp.OpenDialog("OutOfMoneyPopUpModal");
			}
		}
	}
}