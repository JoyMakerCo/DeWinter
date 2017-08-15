using System;
using System.Collections.Generic;
using Core;

namespace UFlow
{
	public class UMachine : UState
	{
		private UState _state;
		internal string[][] _states;

		public UState State
		{
			get { return _state; }
			set {
				// Cleanup the current state
				if (_state != null)
				{
					_state.OnExitState();
					if (_state is IDisposable)
						((IDisposable)_state).Dispose();
				}
				// Exit the current machine if the incoming state is null
				if (value == null)
				{
					this.OnExitState();
					Abort();
					if (Machine != null)
						Machine.NextState();
				}
				// Go to the specified state within the machine
				else
				{
					_state = value;
					_state.Machine = this;
					_state.OnEnterState();
					if (_state is UMachine) 
						State = _UFlow.BuildState(((UMachine)_state)._states[0][0], _state.ID);
				}
			}
		}

		internal UState NextState()
		{
			try
			{
				string stateID =  (_state is IDecision)
					? ((IDecision)_state).Choice
					: Array.Find(_states, s=>s[0]==_state.ID)[1];
				return State = _UFlow.BuildState(ID, stateID);
			}
			catch(Exception e)
			{
				Abort();
				throw new Exception("Invalid state for Machine \'" + ID + "\'; Aborting;", e);
			}
		}

		public void Abort()
		{
			if (_state is IDisposable) ((IDisposable)_state).Dispose();
			_state = null;
			if (this is IDisposable) ((IDisposable)this).Dispose();
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
