using System;
using UFlow;

namespace Ambition
{
	public class WaitForMessageState : UState, Util.IInitializable<string>
	{
		private string _messageID;

		public void Initialize(string messageID)
		{
			_messageID = messageID;
			AmbitionApp.Subscribe(messageID, End);
		}

		public override void OnExitState()
		{
			AmbitionApp.Unsubscribe(_messageID, End);
		}
	}
}
