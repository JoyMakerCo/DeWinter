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
				ServantModel smod = DeWinterApp.GetModel<ServantModel>();
				int numservants=0;
				AdjustValueVO vo = new AdjustValueVO(GameConsts.LIVRE, 0);
				ServantVO servant;
				foreach (KeyValuePair<string, ServantVO[]> kvp in smod.Servants)
	            {
	            	servant = Array.Find(kvp.Value, s => s.Hired);
	            	if (servant != null)
		            	vo.Amount -= servant.Wage;
	            }

	            if (vo.Amount < 0)
	            {
					DeWinterApp.SendMessage<AdjustValueVO>(vo);
					Dictionary<string, string> substitutions = new Dictionary<string, string>(){
						{"$NUMSERVANTS",numservants.ToString()},
						{"$TOTALWAGES",(-vo.Amount).ToString()},
						{"$LIVRE",GameData.moneyCount.ToString()}};
					DeWinterApp.OpenMessageDialog(DialogConsts.PAY_DAY_DIALOG, substitutions);
				}
			}
		}
	}
}