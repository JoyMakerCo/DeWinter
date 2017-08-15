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
		public string ID
		{
			get;
			internal set;
		}

		internal UFlowSvc _uflow;
		protected UFlowSvc _UFlow { get { return _uflow; } }

		public UMachine Machine
		{
			get;
			internal set;
		}

		// Overload for instructions
		public virtual void OnEnterState() {}
		public virtual void OnExitState() {}

		public static UState operator++(UState state)
		{
			return state.Machine.NextState();
		}

		public void End()
		{
			Machine.NextState();
		}
	}

	// Decision decorator for Decision States.
	// The Choice string should be set in OnEnterState or a
	// delegate within the Decision State.
	public interface IDecision
	{
		string Choice { get; set; }
	}
}
