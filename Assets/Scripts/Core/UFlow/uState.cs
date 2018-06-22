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
	public class UState : IDisposable
	{
		public string ID { get { return _node.ID; } }
		internal UMachine _machine;
		internal UStateNode _node;

		// Overload for instructions
		public virtual void OnEnterState() {}
		public virtual void OnExitState() {}
		public virtual void Dispose() {}
	}

	public abstract class UState<T> : UState
	{
		public abstract void SetData(T node);
	}
}
