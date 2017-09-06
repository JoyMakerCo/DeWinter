using System;
using System.Collections.Generic;
using Core;
using Util;

namespace UFlow
{
	public class UFlowSvc : IAppService
	{
		private Dictionary<string, Func<UState>> _bindings = new Dictionary<string, Func<UState>>();
		private Dictionary<string, List<string[]>> _states = new Dictionary<string, List<string[]>>();
		internal Dictionary<string, UMachine> _machines = new Dictionary<string, UMachine>();

		public void RegisterDecision(string machineID, string stateID, Func<bool> condition, string targetStateID)
		{
			if (!_bindings.ContainsKey(stateID))
			{
				_bindings[stateID] = (Func<UState>)(() => {
					UDecision state = new UDecision();
					state._uflow = this;
					return state;
				});
			}
		}

		public void RegisterState<S>(string machineID, string stateID, string targetStateID) where S : UState, new()
		{
			if (!_bindings.ContainsKey(stateID))
			{
				_bindings[stateID] = (Func<UState>)(() => {
					UState state = new S();
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

		internal UState BuildState(string stateID, string machineID=null)
		{
			UState s = _bindings[stateID]();
			UMachine mac=null;
			if (machineID != null)
				_machines.TryGetValue(machineID, out mac);
			s.Machine = mac;
			s.ID = stateID;
			mac = s as UMachine;
			if (mac != null)
			{
				_machines[stateID] = mac;
				mac._states = _states[stateID].ToArray();
			}
			return s;
		}

		public void InvokeMachine(string machineID)
		{
			BuildState(machineID).OnEnterState();
		}

		public void RemoveMachine(string MachineID)
		{
			UMachine mac;
			if (_machines.TryGetValue(MachineID, out mac))
			{
				mac.EndState();
			}
		}
	}
}
