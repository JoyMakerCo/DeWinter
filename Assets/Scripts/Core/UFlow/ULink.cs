using System;

namespace UFlow
{
    public class ULink
	{
		// Set by UFlow
		internal UMachine _machine;
		internal int _target;
		internal int _origin;

		// The current State of the machine from which this link originates
		public virtual bool Validate() => false; // Called upon instantiation
        protected void Activate() => _machine.Activate(this);
    }
}
