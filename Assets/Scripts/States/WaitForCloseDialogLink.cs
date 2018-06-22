using System;
using UFlow;

namespace Ambition
{
	public class WaitForCloseDialogLink : AmbitionValueLink<string>
	{
		override public void SetValue(string data)
		{
			ValueID = GameMessages.DIALOG_CLOSED;
			ValidateOnCallback = true;
			Value = data;
		}
	}
}
