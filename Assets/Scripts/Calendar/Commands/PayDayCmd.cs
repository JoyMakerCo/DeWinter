using System;
using System.Collections.Generic;
using Core;

namespace Ambition
{
	public class PayDayCmd : ICommand<DateTime>
	{
		public void Execute (DateTime day)
		{
			if (day.Day%7 == 0)
			{
				InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
				GameModel game = AmbitionApp.GetModel<GameModel>();
				List<ItemVO> servants = inventory.Inventory.FindAll(i=>i.Type == ItemConsts.SERVANT && i.Equipped);
				float cost = 0;

				foreach (ItemVO item in servants)
	            {
	            	cost += item.Price;
	            }

	            if (cost > 0)
	            {
					game.Livre-=(int)cost;
					Dictionary<string, string> substitutions = new Dictionary<string, string>(){
						{"$NUMSERVANTS",servants.Count.ToString()},
						{"$TOTALWAGES", cost.ToString()},
						{"$LIVRE", game.Livre.ToString()}};
					AmbitionApp.OpenMessageDialog(DialogConsts.PAY_DAY_DIALOG, substitutions);
				}
			}
		}
	}
}
