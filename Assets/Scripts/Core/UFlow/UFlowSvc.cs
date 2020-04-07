using System;
using System.Linq;
using System.Collections.Generic;
using Core;
using Util;
using UGraph;

namespace UFlow
{
	public class UFlowSvc : IAppService, IConsoleEntity 
	{
        // Active machines.
        private List<UMachine> _active = new List<UMachine>();

        // Machine definitions.
        private List<UController> _controllers = new List<UController>();
		private Dictionary<string, DirectedGraph<UStateNode, UGraphLink>> _machines = new Dictionary<string, DirectedGraph<UStateNode, UGraphLink>>();
		private Dictionary<Type, Delegate> _instantiators = new Dictionary<Type, Delegate>();
        private Dictionary<string, string[]> _decisions = new Dictionary<string, string[]>();

        public UFlowSvc()
        {
            Func<UMachineStateNode, UMachineState> del = n => new UMachineState(n.ID, n.MachineID);
            _instantiators.Add(typeof(UMachineStateNode), del);
        }

        public UMachine GetMachine(string MachineID)
		{
			return _active.Find(m => m.MachineID == MachineID);
		}

        public UMachine[] GetMachines(string MachineID)
        {
            return _active.FindAll(m => m.MachineID == MachineID).ToArray();
        }

        internal UController GetController(UMachine machine) => _controllers.Find(c => c._Machine == machine);

		public string[] GetActiveMachines()
		{
			return _active.Select(m=>m.MachineID).ToArray();
		}

        public UMachine[] GetAllMachines()
        {
            return _active.ToArray();
        }

		public bool IsActiveState(string stateID)
		{
			return _active.Exists(m=>m.GetActiveStates().Contains(stateID));
		}

		public bool IsActiveMachine(string machineID)
		{
			return _active.Exists(m=>m.MachineID == machineID);
		}

        public bool ExitMachine(string MachineID)
        {
            UMachine machine = _active.Find(m => m.MachineID == MachineID);
            machine?.Dispose();
            return _active.Remove(machine);
        }

        private N AddNodeToGraph<N>(N node, string nodeID, string machineID) where N:UStateNode
		{
			if (!_machines.TryGetValue(machineID, out DirectedGraph<UStateNode, UGraphLink> graph))
			{
				graph = new DirectedGraph<UStateNode, UGraphLink>();
				_machines.Add(machineID, graph);
			}
			node.ID = nodeID;
            graph.Nodes = graph.Nodes == null
                ? new UStateNode[] { node }
                : graph.Nodes.Append(node).ToArray();
			return node;
		}

        public void BindState<T>(string machineID, string stateID) where T:UNode, new()
        {
            // Activator uses reflection. This does not.
            Type t = typeof(UStateNode<T>);
            if (!_instantiators.TryGetValue(t, out Delegate del))
            {
                Func<UStateNode<T>, T> Create = node => new T { ID = stateID };
                _instantiators[t] = Create;
            }
            BindNode(machineID, stateID, new UStateNode<T>());
        }

        public void BindMachineState(string machineID, string stateID, string subMachineID)
        {
            BindNode(machineID, stateID, new UMachineStateNode(subMachineID));
        }

        private void BindNode(string machineID, string stateID, UStateNode node)
        {
            if (!_machines.TryGetValue(machineID, out DirectedGraph<UStateNode, UGraphLink> graph)) return;
            if (graph.Nodes == null) return;
            int index = Array.FindIndex(graph.Nodes, n=>n.ID == stateID);
            if (index >= 0) 
            {
                node.ID = stateID;
                node.Tags = graph.Nodes[index].Tags;
                graph.Nodes[index] = node;
            };

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
				Func<UStateNode<S>, S> Create = node => new S { ID = node.ID };
                _instantiators.Add(typeof(UStateNode<S>), Create);
			}
		}

		public void RegisterState<S, T>(string machineID, string stateID, T arg) where S : UState<T>, new()
		{
			UStateNode<S,T> n = AddNodeToGraph(new UStateNode<S,T>(), stateID, machineID);
			n.Data = arg;
			if (!_instantiators.ContainsKey(typeof(UStateNode<S,T>)))
			{
                Func<UStateNode<S, T>, S> Create = node =>
                 {
                    S s = new S { ID = node.ID };
                    s.SetData(node.Data);
					return s;
				};
				_instantiators.Add(typeof(UStateNode<S,T>), Create);
			}
		}

        public void RegisterMachineState(string machineID, string stateID, string newMachineID)
        {
            AddNodeToGraph(new UMachineStateNode(newMachineID), stateID, machineID);
        }

        public void RegisterDecision<D>(string machineID, string stateID, string yesState, string noState)
        {

        }

        internal UNode BuildNode(UStateNode node)
        {
            _instantiators.TryGetValue(node.GetType(), out Delegate del);
            return (del?.DynamicInvoke(node) as UNode) ?? new UState();
        }           

        internal ULink BuildLink(UGraphLink ln)
        {
            _instantiators.TryGetValue(ln.GetType(), out Delegate del);
            return (del?.DynamicInvoke(ln) as ULink) ?? new UDefaultLink();
        }

        private UGraphLink AddLinkToGraph(UGraphLink link, string machineID, string originState, string targetState)
		{
			DirectedGraph<UStateNode, UGraphLink> graph;
			if (!_machines.TryGetValue(machineID, out graph)) return null;

			int from = Array.FindIndex(graph.Nodes, n=>n.ID == originState);
            int to = Array.FindIndex(graph.Nodes, n=>n.ID == targetState);
            graph.Link(from, to, link);
			return link;
		}

		public void RegisterLink(string machineID, string originState, string targetState)
		{
#if (DEBUG)
            DirectedGraph<UStateNode, UGraphLink> graph;
            if (!_machines.TryGetValue(machineID, out graph)) UnityEngine.Debug.LogError("UFLOW ERROR: No Machine named \"" + machineID + "\" found!");
            else if (!Array.Exists(graph.Nodes, n => n.ID == originState)) UnityEngine.Debug.LogError("UFLOW ERROR: No state called \"" + originState + "\" found in Machine \"" + machineID + "!");
            else if (!Array.Exists(graph.Nodes, n => n.ID == targetState)) UnityEngine.Debug.LogError("UFLOW ERROR: No state called \"" + targetState + "\" found in Machine \"" + machineID + "!");
#endif
            AddLinkToGraph(new UGraphLink(), machineID, originState, targetState);
		}

		public void RegisterLink<T>(string machineID, string originState, string targetState) where T : ULink, new()
		{
			AddLinkToGraph(new UGraphLink<T>(), machineID, originState, targetState);
			if (!_instantiators.ContainsKey(typeof(UGraphLink<T>)))
			{
				Func<UGraphLink<T>, T> Create = l=> new T();
				_instantiators.Add(typeof(UGraphLink<T>), Create);
			}
		}

		public void RegisterLink<T,D>(string machineID, string originState, string targetState, D data) where T : ULink<D>, new()
		{
			UGraphLink<T,D> link = ((UGraphLink<T,D>)AddLinkToGraph(new UGraphLink<T,D>(), machineID, originState, targetState));
			link.Data = data;
			if (!_instantiators.ContainsKey(typeof(UGraphLink<T,D>)))
			{
				Func<UGraphLink<T,D>, T> Create = l=>
				{
					T t = new T();
					t.SetValue(l.Data);
					return t;
				};
				_instantiators.Add(typeof(UGraphLink<T,D>), Create);
			}
		}

        // Build a machine based on machine ID;
        internal UMachine BuildMachine(string machineID)
        {
            DirectedGraph<UStateNode, UGraphLink> graph;
            if (!_machines.TryGetValue(machineID, out graph)) return null;
            UMachine machine = new UMachine(machineID, graph);
            machine._UFlow = this;
            return machine;
        }

        public UMachine InvokeMachine(string machineID)
		{
            UMachine machine = BuildMachine(machineID);
            machine?.Start();
			return machine;
		}

        // Remove a machine from the active machines list.
        // Note that this does not fully dispose of the removed machine.
        internal bool Remove(UMachine machine) => _active.Remove(machine);
        internal bool Remove(UController controller)
        {
            controller?._Machine?.Dispose();
            return _controllers.Remove(controller);
        }
        internal bool Activate(UMachine machine)
        {
            if (machine == null || _active.Contains(machine)) return false;
            _active.Add(machine);
            return true;
        }
        internal bool Activate(UController controller)
        {
            if (controller == null || _controllers.Contains(controller)) return false;
            _controllers.Add(controller);
            return true;
        }
        override public string ToString() => string.Join("; ", _active.Select(m => m.MachineID));

        public void Dispose()
        {
            foreach(DirectedGraph <UStateNode, UGraphLink> graph in _machines.Values)
            {
                graph.Dispose();
            }
            while (_active.Count > 0) _active[0].Dispose();
            _machines.Clear();
            _controllers.Clear();
            _instantiators.Clear();
            _decisions.Clear();
        }

        public string[] Dump()
        {
            var lines = new List<string>()
            {
                "UFlowSvc:",
                
            };

            foreach (var machine in _active)
            {
                if (machine == null)
                    continue;

                foreach (var line in machine.Dump())
                {
                    lines.Add( "  "+line );
                }
            }

            return lines.ToArray();
        }

        
        public void Invoke( string[] args )
        {
            ConsoleModel.warn("UFlowSvc has no invocation.");
        }    
    }
}
