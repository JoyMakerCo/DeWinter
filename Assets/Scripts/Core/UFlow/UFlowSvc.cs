using System;
using System.Linq;
using System.Collections.Generic;
using Core;
using Util;

namespace UFlow
{
	public class UFlowSvc : IAppService
	{
		private Dictionary<Type, Func<UState>> _states = new Dictionary<Type, Func<UState>>();
		private Dictionary<Type, Func<ULink>> _links = new Dictionary<Type, Func<ULink>>();

// TODO: Instead of an internal list of machines, this data will live in a scriptableObject
private Dictionary<string, UMachine> _allMachines = new Dictionary<string, UMachine>();
		// Machines that have been instantiated.
		internal List<UMachineState> _machines = new List<UMachineState>();

		public bool IsActiveState(string stateID)
		{
			return _machines.Exists(s=>s.Name==stateID)
				|| _machines.Any(s=>s._states.Keys.Any(t=>t.Name==stateID));
		}

private UMachine _getMachine(string machineID)
{
	UMachine machine;
	if (!_allMachines.TryGetValue(machineID, out machine))
	{
		machine = new UMachine();
		_allMachines.Add(machineID, machine);
	}
	return machine;
}

public void RegisterState<S>(string machineID, string stateID) where S : UState, new()
{
	UMachine machine = _getMachine(machineID);
	UStateMap<S> map = new UStateMap<S>();
	if (machine.Nodes == null) machine.Nodes = new UStateMap[0];
	map.Name = stateID;
	machine.Nodes = machine.Nodes.Append(map).ToArray();
}

public void RegisterState(string machineID, string stateID)
{
	RegisterState<UState>(machineID, stateID);
}

public void RegisterState<S, T>(string machineID, string stateID, T data) where S : UState<T>, new()
{
	UMachine machine = _getMachine(machineID);
	UStateMap<S, T> map = new UStateMap<S, T>();
	if (machine.Nodes == null) machine.Nodes = new UStateMap[0];
	map.Name = stateID;
	map.Data = data;
	machine.Nodes = machine.Nodes.Append(map).ToArray();
}

public void RegisterLink<T>(string machineID, string originState, string targetState, string input=null) where T : ULink, new()
{
	UMachine machine = _getMachine(machineID);
	ULinkMap<T> link = new ULinkMap<T>();
	if (machine.Links == null) machine.Links = new ULinkMap[0];
	link.Origin = Array.FindIndex(machine.Nodes, n=>n.Name == originState);
	link.Target = Array.FindIndex(machine.Nodes, n=>n.Name == targetState);
	link.Input = input;
	machine.Links = machine.Links.Append(link).ToArray();
}

public void RegisterLink(string machineID, string originState, string targetState, string input=null)
{
	RegisterLink<ULink>(machineID, originState, targetState, input);
}

public void RegisterLink<T,U>(string machineID, string originState, string targetState, U data, string input=null) where T : ULink<U>, new()
{
	UMachine machine = _getMachine(machineID);
	ULinkMap<T,U> link = new ULinkMap<T,U>();
	if (machine.Links == null) machine.Links = new ULinkMap[0];
	link.Origin = Array.FindIndex(machine.Nodes, n=>n.Name == originState);
	link.Target = Array.FindIndex(machine.Nodes, n=>n.Name == targetState);
	link.Input = input;	
	link.Data = data;
	machine.Links = machine.Links.Append(link).ToArray();
}
		public UMachineState InvokeMachine(string machineID)
		{
			UMachine machine;
			if (!_allMachines.TryGetValue(machineID, out machine))
				return null;
			UMachineState state = new UMachineState();
			state.Data = machine;
			state.Name = machineID;
			state._uflow = this;
			_machines.Add(state);
			state.OnEnterState();
			return state;
		}
	}
}
