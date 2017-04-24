using System;
using System.Collections.Generic;
using Core;

namespace DeWinter
{
	public class PayDayCmd : ICommand<DateTime>
	{
		public void Execute (DateTime day)
		{
			if (day.Day%7 == 0)
			{
				ServantModel smod = DeWinterApp.GetModel<ServantModel>();
				int numservants=0;
				ServantVO servant;
				float livre = 0;
				foreach (KeyValuePair<string, ServantVO[]> kvp in smod.Servants)
	            {
	            	servant = Array.Find(kvp.Value, s => s.Hired);
	            	if (servant != null)
		            	livre += servant.Wage;
	            }

	            if (livre > 0)
	            {
	            	DeWinterApp.AdjustValue(GameConsts.LIVRE, -livre);
					Dictionary<string, string> substitutions = new Dictionary<string, string>(){
						{"$NUMSERVANTS",numservants.ToString()},
						{"$TOTALWAGES", livre.ToString()},
						{"$LIVRE",GameData.moneyCount.ToString()}};
					DeWinterApp.OpenMessageDialog(DialogConsts.PAY_DAY_DIALOG, substitutions);
				}
			}
		}
	}
}