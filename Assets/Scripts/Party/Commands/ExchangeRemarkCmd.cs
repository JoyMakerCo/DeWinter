using System;
using Core;

namespace Ambition
{
	public class ExchangeRemarkCmd : ICommand<RemarkVO>
	{
		public void Execute (RemarkVO remark)
		{
			PartyModel pmod = AmbitionApp.GetModel<PartyModel>();
			if (pmod.Hand.Remove(remark))
			{
				AmbitionApp.SendMessage(PartyMessages.ADD_REMARK);
				AmbitionApp.SendMessage(PartyMessages.END_TURN);
			}
		}
	}
}