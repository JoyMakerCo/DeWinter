using System;
using Core;

namespace Ambition
{
	public class ExchangeRemarkCmd : ICommand<RemarkVO>
	{
		public void Execute (RemarkVO remark)
		{
			PartyModel pmod = AmbitionApp.GetModel<PartyModel>();
			int index = Array.IndexOf(pmod.Remarks, pmod.Remark);
			if (index >= 0)
			{
				pmod.Remarks[index]=null;
				AmbitionApp.SendMessage(PartyMessages.ADD_REMARK);
				AmbitionApp.SendMessage(PartyMessages.END_TURN);
			}
		}
	}
}
