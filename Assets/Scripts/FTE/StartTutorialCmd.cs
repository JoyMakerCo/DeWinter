using System;
using Core;

namespace Ambition
{
	public class StartTutorialCmd : ICommand
	{
		public void Execute ()
		{
			AmbitionApp.InvokeMachine("TutorialController");
		}
	}
}
