using System;
using System.Collections.Generic;
using Core;

namespace UFlow
{
	public sealed class UMachine : UState
	{
		private UState _state;

		internal UFlowSvc _uflow;
		internal ULink[] _transitions;
		internal string _initialState;

		public string State
		{
			get { return _state != null ? _state.ID : null; }
			set { SetState(value != null ? _uflow.BuildState(value,ID) : null); }
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
			else 
			{
				OnExitState();
				if (_machine != null)
					_machine.BuildTransitions();
			}
		}

		public override void OnEnterState ()
		{
			State = _initialState;
		}

		public override void OnExitState ()
		{
			_state = null;
			ClearTransitions();
			_initialState = null;
			_uflow._machines.Remove(this.ID);
		}

		private void BuildTransitions()
		{
			if (_state == null) return;

			_transitions = _uflow.BuildTransitions(ID, _state.ID);
			if (_transitions != null && _transitions.Length > 0)
			{
				ULink link = Array.Find(_transitions, t=>t.InitializeAndValidate());
				if (link != null) State = link.TargetState;
			}
			else State = null;
		}

		private void ClearTransitions()
		{
			if (_transitions == null) return;
			foreach(ULink trans in _transitions)
				trans.Dispose();
			_transitions = null;
		}
	}
}
