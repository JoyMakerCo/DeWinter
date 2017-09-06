using System;
using System.Collections.Generic;
using Core;

namespace UFlow
{
	public sealed class UMachine : UState, IPersistentState
	{
		private UState _state;
		internal string[][] _states;

		public override void OnEnterState ()
		{
			SetState(_states[0][0]);
		}

		public void OnExitState ()
		{
			if (_state is IPersistentState)
				((IPersistentState)_state).OnExitState();
			if (_state is IDisposable)
				((IDisposable)_state).Dispose();
			_state = null;
			_uflow._machines.Remove(this.ID);
		}

		internal void SetState(string stateID)
		{
			// Go to the specified state within the machine
			if (stateID != null) do
			{
				if (_state is IDisposable)
					((IDisposable)_state).Dispose();
				_state = _uflow.BuildState(stateID,ID);
				_state.OnEnterState();
				stateID = GetNextStateID();
			}
			while (!(_state is IPersistentState) && stateID != null);

			// Exit the current machine if the incoming state is null
			else EndState();
		}

		public void NextState()
		{
			SetState(GetNextStateID());
		}

		private string GetNextStateID()
		{
			string [] result = Array.Find(_states, s=>s[0]==_state.ID);
			return (result != null && result.Length > 1) ? result[1] : null;
		}
	}
}
