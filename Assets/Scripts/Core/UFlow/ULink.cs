using System;

namespace UFlow
{
	/// <summary>
	/// Tranisitions between UStates. Once a transition is Validated, the Machine proceeds to NextState.
	/// </summary>
	public abstract class ULink : IDisposable
	{
		public object[] Parameters;

		internal UMachine _machine;
		internal string _targetState;

		public string TargetState
		{
			get { return _targetState; }
		}

		/// <summary>
		/// Initialize the Transition and return true if Transition takes immediate effect
		/// For Transitions from UMachines, this is called AFTER exiting the Machine.
		/// </summary>
		public abstract bool InitializeAndValidate();

		/// <summary>
		/// Proceed Immediately to NextState. Useful for Callbacks.
		/// </summary>
		public void Validate()
		{
			_machine.State = _targetState;
		}

		/// <summary>
		/// Clean up allocated memory associated with this transition. Called by the Machine.
		/// </summary>
		public virtual void Dispose() {}
	}

	public class UBasicLink : ULink
	{
		public UBasicLink(string targetState)
		{
			_targetState = targetState;
		}

		public override bool InitializeAndValidate ()
		{
			return true;
		}
	}
}
