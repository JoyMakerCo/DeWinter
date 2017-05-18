using System;
using Core;

namespace Ambition
{
	public class StartTurnCmd : ICommand
	{
		public void Execute ()
		{
			// TODO: Clunky as hell. NEED a buff system.
			InventoryModel imod = AmbitionApp.GetModel<InventoryModel>();
			ItemVO accessory;
			float t = AmbitionApp.GetModel<PartyModel>().TurnTime;
			if (imod.Equipped.TryGetValue(ItemConsts.ACCESSORY, out accessory) && accessory.Name == "Fan")
			{
				t *= 1.1f;
			}
			AmbitionApp.SendMessage<float>(PartyMessages.START_TIMERS, t);
		}
	}
}
