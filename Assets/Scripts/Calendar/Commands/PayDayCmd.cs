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
                inventory.Inventory.TryGetValue(ItemType.Servant, out List<ItemVO> servants);
				float cost = 0;
                servants?.ForEach(s => cost += s.Price);

	            if (cost > 0)
	            {
					game.Livre.Value-=(int)cost;
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
