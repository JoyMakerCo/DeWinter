using System;
using System.Collections.Generic;
using Core;

namespace UFlow
{
	// Base class for all states.
	// States work like commands, except they persist in memory.
	// OnEnterState and OnExitState are invoked by the UFlowSvc,
	// and are ideal places to execute asynchronous instructions
	// and set up delegates.
	public class UState
	{
		internal UMachineState _machine;
		public string Name;

		public UState() {}
		public UState(string name) { Name = name; }
		public UState(UState state) { Name = state.Name; }
		
		// Overload for instructions
		public virtual void OnEnterState() {}
		public virtual void OnExitState() {}

		// Dispose is called independently of OnExitState().
		// Put cleanup here.
		public virtual void Dispose() {}
	}

	public class UState<T> : UState
	{
		public T Data;
	}
}
