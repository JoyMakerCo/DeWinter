using System;
using System.Collections.Generic;
using Core;
using Util;

namespace UFlow
{
	public interface IUFlowBindings
	{
	}

	public class UFlowSvc : IAppService, IInitializable
	{
		private Dictionary<string, Func<UState>> _bindings = new Dictionary<string, Func<UState>>();
		private Dictionary<string, List<string[]>> _states = new Dictionary<string, List<string[]>>();
		private Dictionary<string, UMachine> _machines = new Dictionary<string, UMachine>();

		public void Initialize()
		{
			// TODO: Read the machine from config
			string file;
			IUFlowBindings bindings;
		}

		public void RegisterState<S>(string machineID, string stateID, string targetStateID) where S : UState, new()
		{
			UState state;
			if (!_bindings.ContainsKey(stateID))
			{
				_bindings[stateID] = (Func<UState>)(() => {
					state = new S();
					if (state is IInitializable)
						((IInitializable)state).Initialize();
					state._uflow = this;
					state.ID = stateID;
					return state;
				});
			}
			BuildConnection(machineID, stateID, targetStateID);
		}

		public void RegisterState<S, T>(string machineID, string stateID, string targetStateID, T arg) where S : UState, IInitializable<T>, new()
		{
			if (!_bindings.ContainsKey(stateID))
			{
				_bindings[stateID] = (Func<UState>)(() => {
					UState state = new S();
					((IInitializable<T>)state).Initialize(arg);
					state._uflow = this;
					state.ID = stateID;
					return state;
				});
			}
			BuildConnection(machineID, stateID, targetStateID);
		}

		private void BuildConnection(string machineID, string stateID, string targetStateID)
		{
			List<string[]> connections;
			string[] states;

			if (!_bindings.ContainsKey(machineID))
			{
				_bindings[machineID] = (Func<UState>)(() => {
					UMachine machine = new UMachine();
					machine._uflow = this;
					machine.ID = machineID;
					return machine;
				});
			}
			if (!_states.TryGetValue(machineID, out connections))
			{
				_states.Add(machineID, connections = new List<string[]>());
			}

			states = new string[2]{ stateID, targetStateID };
			connections.Add(states);
		}

		internal UState BuildState(string stateID)
		{
			UState s = _bindings[stateID]();
			if (s is UMachine)
			{
				_machines[stateID] = (UMachine)s;
				((UMachine)s)._states = _states[stateID].ToArray();
			}
			return s;
		}

		public void InvokeMachine(string machineID)
		{
			UState s = BuildState(machineID);
			s.OnEnterState();
		}
	}
}
