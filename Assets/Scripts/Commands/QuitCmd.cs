using System;
using Core;

namespace Ambition
{
	public class QuitCmd : ICommand
	{
		public void Execute ()
		{
			AmbitionApp.OpenDialog(DialogConsts.EXIT_GAME);
		}
	}
}

