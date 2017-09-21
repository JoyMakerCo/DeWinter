using System;
using System.Collections.Generic;
using Core;

namespace UFlow
{
	public sealed class UMachine : UState
	{
		private UState _state;

		internal UFlowSvc _uflow;
		internal UTransition[] _transitions;
		internal string _initialState;

		public string State
		{
			get { return _state != null ? _state.ID : null; }
			set { SetState(_uflow.BuildState(value,ID)); }
		}

		internal void SetState(UState state)
		{
			if (_state != null)
				_state.OnExitState();

			ClearTransitions();

			// Go to the specified state within the machine
			_state = state;

			// If the new state is non-null, enter the new state
			if (_state != null)
			{
				_state.OnEnterState();
				if (!(_state is UMachine))
					BuildTransitions();
			}

			// Exit the current machine if the incoming state is null
			else if (_machine != null)
				_machine.BuildTransitions();
		}

		public override void OnEnterState ()
		{
			State = _initialState;
		}

		public override void OnExitState ()
		{
			_state = null;
			ClearTransitions();
			_uflow._machines.Remove(this.ID);
		}

		private void BuildTransitions()
		{
			if (_state == null) return;

			_transitions = _uflow.BuildTransitions(ID, _state.ID);
			foreach(UTransition trans in _transitions)
			{
				if (trans.InitializeAndValidate())
				{
					State = trans._targetState;
					return;
				}
			}
		}

		private void ClearTransitions()
		{
			if (_transitions == null) return;
			foreach(UTransition trans in _transitions)
				trans.Dispose();
			_transitions = null;
		}
	}
}
