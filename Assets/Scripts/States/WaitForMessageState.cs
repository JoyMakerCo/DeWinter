using System;
using UFlow;

namespace Ambition
{
	public class WaitForMessageState : UState, IPersistentState, Util.IInitializable<string>
	{
		private string _messageID;

		public void Initialize(string messageID)
		{
			_messageID = messageID;
		}

		public override void OnEnterState ()
		{
			AmbitionApp.Subscribe(_messageID, EndState);
		}

		public void OnExitState()
		{
			AmbitionApp.Unsubscribe(_messageID, EndState);
		}
	}
}
