using System;

namespace UFlow
{
	/// <summary>
	/// Tranisitions between UStates. If the Machine's current state is State,
	/// and receives this Link's input, this Link will be followed.
	/// </summary>

	public class ULink
	{
		// Set by UFlow
		internal UMachineState _machine;
		internal int _targetState;

		// The current State of the machine from which this link originates
		public string Input;
		public UState State 
		{
			internal set;
			get;
		}		
		/// <summary>
		/// Shortcut to sending this link's input to the machine.
		/// </summary>
		public void Activate() { _machine.Input(Input); }
		public virtual void Initialize() {}
		public virtual bool Validate() { return false; } // Called upon instantiation
		public virtual void Dispose() {}
	}

	public abstract class ULink<T> : ULink
	{
		public T Data;
	}
}
