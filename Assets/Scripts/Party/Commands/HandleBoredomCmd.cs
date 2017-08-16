using System;
using Core;

namespace Ambition
{
	public class HandleBoredomCmd : ICommand<GuestVO[]>
	{
		public void Execute (GuestVO[] guests)
		{
			foreach (GuestVO guest in guests)
			{
				
			}
		}
	}
}
