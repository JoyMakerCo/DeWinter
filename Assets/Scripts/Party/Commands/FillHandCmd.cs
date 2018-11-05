using System;
using System.Collections.Generic;
using Core;

namespace Ambition
{
	public class FillHandCmd : ICommand
	{
		public void Execute()
		{
            ConversationModel model = AmbitionApp.GetModel<ConversationModel>();
            int count = Array.FindAll(model.Remarks, r => r == null).Length;
            AmbitionApp.SendMessage(PartyMessages.DRAW_REMARKS, count);
		}
	}
}
