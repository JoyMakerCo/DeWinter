using System;
using System.Linq;
using System.Collections.Generic;
using Core;
using Util;

namespace UFlow
{
	public class UFlowSvc : IAppService, IInitializable
	{
		// Active machines.
		private List<UMachine> _active = new List<UMachine>();
		// Machine definitions.
		private Dictionary<UMachine, UController> _controllers = new Dictionary<UMachine, UController>();
		private Dictionary<string, UMachineGraph> _machines = new Dictionary<string, UMachineGraph>();
		private Dictionary<Type, Delegate> _instantiators = new Dictionary<Type, Delegate>();

		public void Initialize()
		{
			Func<UStateNode, UState> ustate = n => new UState();
			Func<UStateNode<UMachine, string>, UMachine> umachine = n => BuildMachine(n);
			Func<UGraphLink, UDefaultLink> ulink = n => new UDefaultLink();
			_instantiators.Add(typeof(UStateNode), ustate);
			_instantiators.Add(typeof(UStateNode<UMachine, string>), umachine);
			_instantiators.Add(typeof(UGraphLink), ulink);
		}

		public UMachine GetMachine(string MachineID)
		{
			return _active.Find(m => m.MachineID == MachineID);
		}

		internal UController GetController(UMachine machine)
		{
			UController result;
            while (machine._machine != null) machine = machine._machine;
			return _controllers.TryGetValue(machine, out result) ? result : null;
		}

		public string[] GetActiveMachines()
		{
			return _active.Select(m=>m.MachineID).ToArray();
		}

		public bool IsActiveState(string stateID)
		{
			return _active.Exists(m=>m.ID == stateID || m.GetActiveStates().Contains(stateID));
		}

		public bool IsActiveMachine(string machineID)
		{
			return _active.Exists(m=>m.MachineID == machineID);
		}

		private N AddNodeToGraph<N>(N node, string nodeID, string machineID) where N:UStateNode
		{
			UMachineGraph graph;
			if (!_machines.TryGetValue(machineID, out graph))
			{
				graph = new UMachineGraph();
				_machines.Add(machineID, graph);
			}
			node.ID = nodeID;
			if (graph.Nodes != null)
				graph.Nodes = graph.Nodes.Append(node).ToArray();
			else graph.Nodes = new UStateNode[]{node};
			return node;
		}

		public void RegisterState(string machineID, string stateID)
		{
			AddNodeToGraph(new UStateNode(), stateID, machineID);
		}

		public void RegisterState<S>(string machineID, string stateID) where S : UState, new()
		{
			AddNodeToGraph(new UStateNode<S>(), stateID, machineID);
			if (!_instantiators.ContainsKey(typeof(UStateNode<S>)))
			{
				Func<UStateNode<S>, S> Create = node => new S();
				_instantiators.Add(typeof(UStateNode<S>), Create);
			}
		}

		public void RegisterState<S, T>(string machineID, string stateID, T arg) where S : UState<T>, new()
		{
			UStateNode<S,T> n = AddNodeToGraph(new UStateNode<S,T>(), stateID, machineID);
			n.Data = arg;
			if (!_instantiators.ContainsKey(typeof(UStateNode<S,T>)))
			{
				Func<UStateNode<S,T>, S> Create = node =>
				{
					S s = new S();
					s._node = node;
					s.SetData(node.Data);
					return s;
				};
				_instantiators.Add(typeof(UStateNode<S,T>), Create);
			}
		}

		internal U Build<G,U>(G graphData)
		{
			Type t = graphData.GetType();
			U u = (U)(_instantiators[t].DynamicInvoke(graphData));
			return u;
		}

		private L AddLinkToGraph<L>(L link, string machineID, string originState, string targetState) where L:UGraphLink
		{
			UMachineGraph graph;
			if (!_machines.TryGetValue(machineID, out graph)) return null;

			link.Origin = Array.FindIndex(graph.Nodes, n=>n.ID == originState);
			link.Target = Array.FindIndex(graph.Nodes, n=>n.ID == targetState);
			if (link.Origin < 0 || link.Target < 0) return null;

			if (graph.Links != null)
				graph.Links = graph.Links.Append(link).ToArray();
			else graph.Links = new UGraphLink[]{link};
			return link;
		}

		public void RegisterLink(string machineID, string originState, string targetState)
		{
			AddLinkToGraph(new UGraphLink(), machineID, originState, targetState);
		}

		public void RegisterLink<T>(string machineID, string originState, string targetState) where T : ULink, new()
		{
			UGraphLink<T> link = AddLinkToGraph(new UGraphLink<T>(), machineID, originState, targetState);
			if (!_instantiators.ContainsKey(typeof(UGraphLink<T>)))
			{
				Func<UGraphLink<T>, T> Create = l=> new T();
				_instantiators.Add(typeof(UGraphLink<T>), Create);
			}
		}

		public void RegisterLink<T,D>(string machineID, string originState, string targetState, D data) where T : ULink<D>, new()
		{
			UGraphLink<T,D> link = AddLinkToGraph(new UGraphLink<T,D>(), machineID, originState, targetState);
			link.Data = data;
			if (!_instantiators.ContainsKey(typeof(UGraphLink<T,D>)))
			{
				Func<UGraphLink<T,D>, T> Create = l=>
				{
					T t = new T();
					t._target = l.Target;
					t.SetValue(l.Data);
					return t;
				};
				_instantiators.Add(typeof(UGraphLink<T,D>), Create);
			}
		}

		public void RegisterAggregateLinks(string machineID, string originState, params string[] targetStates)
		{

		}

		public UMachine InvokeMachine(string machineID)
		{
			UStateNode<UMachine, string> node = new UStateNode<UMachine, string>();
			node.Data = machineID;
			UMachine mac = BuildMachine(node);
			if (mac != null) mac.OnEnterState();
			return mac;
		}

		internal UMachine BuildMachine(UStateNode<UMachine, string> node)
		{
			UMachineGraph graph;
			if (!_machines.TryGetValue(node.Data, out graph))
				return null;
			UMachine mac = new UMachine();
			mac.SetData(node.Data);
			mac._graph = graph;
			mac._uflow = this;
			mac._node = node;
			_active.Add(mac);
			return mac;
		}

		internal void BuildController(UController controller)
		{
			if (controller._machine == null)
			{
				UStateNode<UMachine, string> node = new UStateNode<UMachine, string>();
				node.Data = controller.MachineID;
				controller._machine = BuildMachine(node);
				if (controller._machine != null)
					_controllers.Add(controller._machine, controller);
			}
		}

		internal void RemoveMachine(UMachine machine)
		{
			UController controller;
			if (_controllers.TryGetValue(machine, out controller))
			{
				controller._machine = null;
				_controllers.Remove(machine);
			}
			_active.Remove(machine);
		}

		override public string ToString()
		{
			List<string> machines = new List<string>();
			_active.ForEach(m=>{ machines.Add(m.ToString()); });
			return string.Join("\n", machines);
		}
	}
}
