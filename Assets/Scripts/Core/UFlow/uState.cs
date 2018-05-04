using System;
using System.Collections.Generic;
using Core;

namespace UFlow
{
	// Base class for all states.
	// States work like commands, except they persist in memory.
	// OnEnterState and OnExitState are invoked by the UFlowSvc,
	// and are ideal places to execute asynchronous instructions
	// and set up delegates. The State is exited when End() is called.
	public class UState
	{
		internal UMachine _machine;
		public string ID
		{
			get;
			internal set;
		}

		// Overload for instructions
		public virtual void OnEnterState() {}
		public virtual void OnExitState() {}
	}
}
