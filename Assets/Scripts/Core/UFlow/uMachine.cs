using System;
using System.Collections.Generic;
using Core;

namespace UFlow
{
	public sealed class UMachine : UState
	{
		private UState _state;
		internal string[][] _states;

		public override void OnEnterState ()
		{
UnityEngine.Debug.Log("Entering Machine: " + ID);
			State = _UFlow.BuildState(_states[0][0]);
		}

		public override void OnExitState ()
		{
			if (_state is IDisposable) ((IDisposable)_state).Dispose();
			_state = null;
			if (Machine != null) Machine.NextState();
		}

		public UState State
		{
			get { return _state; }
			set {
				// Cleanup the current state
				if (_state != null) _state.OnExitState();

				// Go to the specified state within the machine
				if (value != null)
				{
					if (_state is IDisposable)
						((IDisposable)_state).Dispose();
					_state = value;
					_state.Machine = this;
UnityEngine.Debug.Log("Entering State: " + _state.ID);
					_state.OnEnterState();
				}

				// Exit the current machine if the incoming state is null
				else End();
			}
		}

		public UState NextState()
		{
			
			string stateID =  (_state is IDecision)
				? ((IDecision)_state).Choice
				: Array.Find(_states, s=>s[0]==_state.ID)[1];
			return State = _UFlow.BuildState(stateID);
		}

		public override string ToString()
		{
			string val = "";
			foreach (string[] state in _states)
			{
				val += state[0] + " => " + string.Join(", ", state, 1, 100) + "\n";
			}
			return val;
		}
	}
}
