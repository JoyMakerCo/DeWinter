using System;
using System.Collections.Generic;
using Core;

namespace DeWinter
{
	public class PayDayCmd : ICommand<CalendarDayVO>
	{
		public void Execute (CalendarDayVO day)
		{
			if (day.Day%7 == 0)
			{
				ServantModel smod = DeWinterApp.GetModel<ServantModel>();
				AdjustBalanceVO vo = new AdjustBalanceVO(BalanceTypes.LIVRE, 0);
				ServantVO servant;
				foreach (KeyValuePair<string, ServantVO[]> kvp in smod.Servants)
	            {
	            	servant = Array.Find(kvp.Value, s => s.Hired);
	            	if (servant != null)
		            	vo.Amount -= servant.Wage;
	            }

	            if (vo.Amount < 0)
	            {
		            DeWinterApp.SendMessage<AdjustBalanceVO>(vo);
		            //TODO: Pop Up Window to explain the Transaction        
	         	}
			}
		}
	}
}