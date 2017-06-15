using System;
using System.Collections.Generic;

namespace uflow
{
	// Catch-all state not defined in bindings.
	public class uState
	{
		public string ID;
		public uMachine Machine;
		public uState NextState;
		public void OnEnterState(uState prevState) {}
		public void OnExitState() {}
	}

	// All decisions pass through this state.
	public class UDecisionState : uState
	{
		// Set upon state machine creation
		public Dictionary<string, uState> Decisions;
		public string Decision
		{
			set {
				// This will throw an exception if the argument decision doesn't exist.
				NextState = Decisions[value];
			}
		}
	}
}
