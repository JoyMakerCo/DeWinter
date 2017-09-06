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
	public abstract class UState
	{
		internal UFlowSvc _uflow;
		protected UFlowSvc _UFlow { get { return _uflow; } }

		public string ID
		{
			get;
			internal set;
		}

		public UMachine Machine
		{
			get;
			internal set;
		}

		// Overload for instructions
		public abstract void OnEnterState();

		public void EndState()
		{
			if (this is IPersistentState)
			{
				((IPersistentState)this).OnExitState();
			}
			if (Machine != null)
				Machine.NextState();
		}
	}

	public interface IPersistentState
	{
		void OnExitState();
	}
}
