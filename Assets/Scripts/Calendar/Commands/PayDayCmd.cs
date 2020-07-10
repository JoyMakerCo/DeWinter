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
				float cost = 0;
                int count = 0;
                foreach(ItemVO item in inventory.Inventory)
                {
                    if (item.Type == ItemType.Servant)
                    {
                        ++count;
                        cost += item.Price;
                    }
                }

	            if (cost > 0)
	            {
					game.Livre.Value-=(int)cost;
					Dictionary<string, string> substitutions = new Dictionary<string, string>(){
						{"$NUMSERVANTS", count.ToString()},
						{"$TOTALWAGES", cost.ToString()},
						{"$LIVRE", game.Livre.ToString()}};
					AmbitionApp.OpenDialog(DialogConsts.PAY_DAY_DIALOG, null, substitutions);
				}
			}
		}
	}
}
