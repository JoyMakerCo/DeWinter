using System;
using UFlow;

namespace Ambition
{
	public class WaitForCloseDialogLink : AmbitionValueLink<string>
	{
		override public void Initialize()
		{
			MessageID = GameMessages.DIALOG_CLOSED;
			ValidateOnCallback = s => { return s == Data; };
			base.Initialize();
		}
	}
}
