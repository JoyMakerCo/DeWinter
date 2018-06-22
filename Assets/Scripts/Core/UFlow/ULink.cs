using System;

namespace UFlow
{
	/// <summary>
	/// Tranisitions between UStates. If the Machine's current state is State,
	/// and receives this Link's input, this Link will be followed.
	/// </summary>

	public abstract class ULink : Util.IInitializable, IDisposable
	{
		// Set by UFlow
		internal UMachine _machine;
		internal int _target;
		internal int _origin;

		// The current State of the machine from which this link originates
		public UState State { get { return _machine._states[_origin]; } }		
		public void Activate() { _machine.Activate(this); }
		public virtual bool Validate() { return false; } // Called upon instantiation
		public virtual void Initialize() { }
		public virtual void Dispose() {}
	}

	public sealed class UDefaultLink : ULink
	{
		public override bool Validate() { return true; } // Called upon instantiation
	}

	public abstract class ULink<T> : ULink
	{
		public abstract void SetValue(T data);
	} 
}
