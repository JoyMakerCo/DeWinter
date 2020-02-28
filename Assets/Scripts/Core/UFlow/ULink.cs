using System;

namespace UFlow
{
	/// <summary>
	/// Tranisitions between UStates. If the Machine's current state is State,
	/// and receives this Link's input, this Link will be followed.
	/// </summary>

	public abstract class ULink : IDisposable
	{
		// Set by UFlow
		internal UMachine _machine;
		internal int _target;
		internal int _origin;

		// The current State of the machine from which this link originates
		public virtual bool Validate() => false; // Called upon instantiation
		public virtual void Initialize() {}
		public virtual void Dispose() {}
        internal void Activate() => _machine.Activate(this);
    }

	public abstract class ULink<T> : ULink
	{
        public abstract void SetValue(T data);
    } 
}
