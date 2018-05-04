using System;
using UFlow;

namespace Ambition
{
	public class WaitForMessageLink : ULink
	{
		protected string _messageID;

		public override bool InitializeAndValidate ()
		{
			System.Diagnostics.Debug.Assert(Parameters != null && Parameters.Length > 0);
			_messageID = Parameters[0] as String;
			AmbitionApp.Subscribe(_messageID, Validate);
			return false;
		}

		public override void Dispose ()
		{
			AmbitionApp.Unsubscribe(_messageID, Validate);
		}
	}

	public class WaitForMessageLink<T> : WaitForMessageLink
	{
		private T _value;

		public override bool InitializeAndValidate ()
		{
			System.Diagnostics.Debug.Assert(Parameters != null && Parameters.Length > 1);
			_messageID = Parameters[0] as String;
			_value = (T)(Parameters[1]);			
			AmbitionApp.Subscribe<T>(_messageID, CheckValue);
			return false;
		}

		private void CheckValue(T value)
		{
			if (((T)value).Equals(_value))
			{
				Validate();
			}
		}

		public override void Dispose ()
		{
			AmbitionApp.Unsubscribe<T>(_messageID, CheckValue);
		}
	}
}
