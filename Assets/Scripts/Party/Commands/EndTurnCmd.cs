using System;
using Core;

namespace Ambition
{
	public class EndTurnCmd : ICommand
	{
		public void Execute ()
		{
			PartyModel model = AmbitionApp.GetModel<PartyModel>();
			model.TurnsLeft--;
		}
	}
}
