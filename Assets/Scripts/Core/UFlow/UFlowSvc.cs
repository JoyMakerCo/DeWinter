﻿using System;
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
        private Dictionary<string, DirectedGraph<UStateNode, UGraphLink>> _machines = new Dictionary<string, DirectedGraph<UStateNode, UGraphLink>>();
        private Dictionary<Type, Func<UStateNode, UState>> _states = new Dictionary<Type, Func<UStateNode, UState>>();
        private Dictionary<Type, Func<UGraphLink, ULink>> _links = new Dictionary<Type, Func<UGraphLink, ULink>>();
        private Dictionary<string, UFlowConfig> _defs = new Dictionary<string, UFlowConfig>();

        public UFlowSvc()
        {
            _states[typeof(UStateNode<UMachine, string>)] = node => (UState)BuildMachine((node as UStateNode<UMachine, string>)?.Data);
        }

        public void Register<D>(string flowID) where D : UFlowConfig, new()
        {
            _defs[flowID] = new D();
            _defs[flowID]._flowID = flowID;
            _defs[flowID]._uFlow = this;
        }

        public string Save()
        {
            Dictionary<string, List<UMachine>> machines = new Dictionary<string, List<UMachine>>();
            List<string> output = new List<string>();
            List<string> line;
            string machineID;
            UMachine umachine;
            int[] state;
            Dictionary<int, UMachine> dependencies;
            foreach (UMachine machine in _active)
            {
                machineID = GetAddMachineID(machines, machine);
                line = new List<string>() { machineID };
                state = machine.SaveState(out dependencies);
                foreach (int i in state)
                {
                    if (dependencies.TryGetValue(i, out umachine))
                    {
                        machineID = GetAddMachineID(machines, umachine);
                        line.Add(i.ToString() + ":" + machineID);
                    }
                    else
                    {
                        line.Add(i.ToString());
                    }
                    if (line.Count > 1) output.Add(string.Join(",", line));
                }
            }
            return string.Join("@", output);
        }

        public void Reset()
        {
            if (_active == null) _active = new List<UMachine>();
            else while (_active.Count > 0) _active[0].Dispose();
        }

        public bool Restore(string uflowState)
        {
            bool complete;
            string[] machineState = uflowState.Split('@');
            string[][] state = new string[machineState.Length][];
            Dictionary<string, UMachine> machines = new Dictionary<string, UMachine>();
            Dictionary<int, UMachine> hash = new Dictionary<int, UMachine>();
            UMachine serializedMachine;

            for (int i = machineState.Length - 1; i >= 0; --i)
            {
                state[i] = machineState[i].Split(',');
            }

            do
            {
                complete = true;
                foreach (string[] mstate in state)
                {
                    if (!machines.ContainsKey(mstate[0]))
                    {
                        serializedMachine = RestoreMachine(mstate, machines);
                        if (serializedMachine != null)
                        {
                            machines.Add(mstate[0], serializedMachine);
                        }
                        else complete = false;
                    }
                }
            } while (!complete);
            Reset();
            _active = new List<UMachine>(machines.Values);
            return true;
        }

        public UMachine GetMachine(string MachineID, ushort instance = 0)
        {
            List<UMachine> result = _active.FindAll(m => m.MachineID == MachineID);
            return instance < result.Count ? result[instance] : null;
        }

        public UMachine[] GetMachines(string MachineID)
        {
            return _active.FindAll(m => m.MachineID == MachineID).ToArray();
        }

        public string[] GetActiveMachines()
        {
            string[] result = new string[_active.Count];
            for (int i = _active.Count - 1; i >= 0; i--)
                result[i] = _active[i]?.MachineID;
            return result;
        }

        public UMachine[] GetAllMachines()
        {
            return _active.ToArray();
        }

        public bool IsActiveState(string stateID)
        {
            foreach (UMachine m in _active)
            {
                if (m.ID == stateID || Array.IndexOf(m.GetActiveStates(), stateID) >= 0)
                    return true;
            }
            return false;
        }

        public bool IsActiveMachine(string machineID)
        {
            return _active.Exists(m => m.MachineID == machineID);
        }

        public bool ExitMachine(string MachineID)
        {
            UMachine machine = _active.Find(m => m.MachineID == MachineID);
            machine?.Dispose();
            return _active.Remove(machine);
        }

        private N AddNodeToGraph<N>(N node, string nodeID, string machineID) where N : UStateNode
        {
            if (!_machines.TryGetValue(machineID, out DirectedGraph<UStateNode, UGraphLink> graph))
            {
                graph = new DirectedGraph<UStateNode, UGraphLink>();
                _machines.Add(machineID, graph);
            }
            node.ID = nodeID;
            if (graph.Nodes == null) graph.Nodes = new UStateNode[] { node };
            else
            {
                List<UStateNode> nodes = new List<UStateNode>(graph.Nodes);
                nodes.Add(node);
                graph.Nodes = nodes.ToArray();
            }
            return node;
        }

        public void BindState<S>(string machineID, string stateID) where S : UState, new()
        {
            // Activator uses reflection. This does not.
            Type t = typeof(UStateNode<S>);
            if (!_states.ContainsKey(t))
            {
                _states[t] = node => new S { ID = stateID };
            }
            BindNode(machineID, stateID, new UStateNode<S>());
        }

        public void BindState<S, D>(string machineID, string stateID, D data) where S : UState, IInitializable<D>, new()
        {
            // Activator uses reflection. This does not.
            Type t = typeof(UStateNode<S, D>);
            if (!_states.ContainsKey(t))
            {
                _states[t] = node =>
                {
                    S state = new S { ID = stateID };
                    state.Initialize(((UStateNode<S, D>)node).Data);
                    return state;
                };
            }
            BindNode(machineID, stateID, new UStateNode<S, D>() { Data = data });
        }

        public void BindLink<L>(string machineID, string fromState, string toState) where L : ULink, new()
        {
            // Activator uses reflection. This does not.
            Type t = typeof(UGraphLink<L>);
            if (!_links.ContainsKey(t))
            {
                _links[t] = l =>
                {
                    L link = new L();
                    (link as IInitializable)?.Initialize();
                    return link;
                };
            }
            BindLink(machineID, fromState, toState, new UGraphLink<L>());
        }

        public void BindLink<L, D>(string machineID, string fromState, string toState, D data) where L : ULink, IInitializable<D>, new()
        {
            // Activator uses reflection. This does not.
            Type t = typeof(UGraphLink<L, D>);
            if (!_links.ContainsKey(t))
            {
                _links[t] = l =>
                {
                    L link = new L();
                    link.Initialize(((UGraphLink<L, D>)l).Data);
                    return link;
                };
            }
            BindLink(machineID, fromState, toState, new UGraphLink<L, D>() { Data = data });
        }

        // PRIVATE / PROTECTED METHODS //////////////////////

        private string GetAddMachineID(Dictionary<string, List<UMachine>> machines, UMachine machine)
        {
            string machineID = machine?.MachineID;
            if (string.IsNullOrEmpty(machineID) || !_machines.ContainsKey(machineID)) return null;
            if (!machines.ContainsKey(machineID))
            {
                machines.Add(machineID, new List<UMachine>() { machine });
                return machineID + "#0";
            }
            int index = machines[machineID].IndexOf(machine);
            if (index < 0)
            {
                index = machines[machineID].Count;
                machines[machineID].Add(machine);
            }
            return machineID + "#" + index;
        }

        private UMachine RestoreMachine(string[] state, Dictionary<string, UMachine> machines)
        {
            // Corrupted state; early out
            int len = state?.Length ?? 0;
            if (len < 1) return null;
            DirectedGraph<UStateNode, UGraphLink> graph;
            string[] stateStr = state[0].Split('#');

            // Couldn't find graph; early out
            if (stateStr.Length < 1 || !_machines.TryGetValue(stateStr[0], out graph))
            {
                return null;
            }

            string machineID = stateStr[0];
            int[] init = new int[len - 1];
            Dictionary<int, UMachine> dependencies = new Dictionary<int, UMachine>();
            UMachine machine;
            UMachine result = new UMachine(this, graph);
            result.Initialize(machineID);

            for (int i = 0; i < len - 1; ++i)
            {
                stateStr = state[i + 1].Split(':');
                init[i] = int.Parse(stateStr[0]);
                if (stateStr.Length > 1)
                {
                    if (machines.TryGetValue(stateStr[1], out machine))
                    {
                        machine._Machine = result;
                        machine.ID = graph.Nodes[init[i]].ID;
                        dependencies[init[i]] = machine;
                    }
                    else return null; // Not all machines instantiated; early out
                }
            }

            if (result.RestoreState(init, dependencies))
            {
                return result;
            }
            result.Dispose();
            return null;
        }

        private void BindNode(string machineID, string stateID, UStateNode node)
        {
            if (!_machines.TryGetValue(machineID, out DirectedGraph<UStateNode, UGraphLink> graph)) return;
            if (graph.Nodes == null) return;
            int index = Array.FindIndex(graph.Nodes, n => n.ID == stateID);
            if (index >= 0)
            {
                node.ID = stateID;
                graph.Nodes[index] = node;
            };
        }

        private void BindLink(string machineID, string fromState, string toState, UGraphLink link)
        {
            if (!_machines.TryGetValue(machineID, out DirectedGraph<UStateNode, UGraphLink> graph)) return;
            if (graph?.Nodes == null || graph.Links == null) return;
            if (graph.LinkData == null) graph.LinkData = new UGraphLink[graph.Links.Length];
            int x = Array.FindIndex(graph.Nodes, n => n.ID == fromState);
            int y = Array.FindIndex(graph.Nodes, n => n.ID == toState);
            if (x >= 0 && y >= 0)
            {
                x = Array.FindIndex(graph.Links, l => l.x == x && l.y == y);
                if (x >= 0) graph.LinkData[x] = link;
            };
        }

        public void RegisterState(string machineID, string stateID)
        {
            AddNodeToGraph(new UStateNode(), stateID, machineID);
        }

        public void RegisterState<S>(string machineID, string stateID) where S : UState, new()
        {
            Type t = typeof(UStateNode<S>);
            if (!_states.ContainsKey(t))
            {
                _states[t] = node => new S { ID = node.ID };
            }
            AddNodeToGraph(new UStateNode<S>(), stateID, machineID);
        }

        public void RegisterState<S, T>(string machineID, string stateID, T arg) where S : UState, IInitializable<T>, new()
        {
            UStateNode<S, T> n = AddNodeToGraph(new UStateNode<S, T>(), stateID, machineID);
            n.Data = arg;
            if (!_states.ContainsKey(typeof(UStateNode<S, T>)))
            {
                Func<UStateNode, UState> Create = node =>
                 {
                     S s = new S { ID = node.ID };
                     s.Initialize((node as UStateNode<S, T>).Data);
                     return s;
                 };
                _states.Add(typeof(UStateNode<S, T>), Create);
            }
        }

        public UMachine Instantiate(string machineID, UMachine flow = null)
        {
            UMachine result = BuildMachine(machineID);
            if (result != null) result._Machine = flow;
            return result;
        }

        internal UState Instantiate(UStateNode node)
        {
            return node != null && _states.TryGetValue(node.GetType(), out Func<UStateNode, UState> del) && del != null
                ? del(node)
                : new UState();
        }           

        internal ULink Instantiate(UGraphLink ln)
        {
            return ln != null && _links.TryGetValue(ln.GetType(), out Func<UGraphLink, ULink> del) && del != null
                ? del(ln)
                : null;
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
            AddLinkToGraph(null, machineID, originState, targetState);
		}

		public void RegisterLink<T>(string machineID, string originState, string targetState) where T : ULink, new()
		{
			AddLinkToGraph(new UGraphLink<T>(), machineID, originState, targetState);
			if (!_links.ContainsKey(typeof(UGraphLink<T>)))
			{
				_links.Add(typeof(UGraphLink<T>), l=>new T());
			}
		}

		public void RegisterLink<L,D>(string machineID, string originState, string targetState, D data) where L : ULink, IInitializable<D>, new()
		{
			UGraphLink<L,D> link = ((UGraphLink<L,D>)AddLinkToGraph(new UGraphLink<L,D>(), machineID, originState, targetState));
			link.Data = data;
			if (!_links.ContainsKey(typeof(UGraphLink<L,D>)))
			{
				Func<UGraphLink, ULink> Create = l=>
				{
					L ln = new L();
					ln.Initialize(((UGraphLink<L,D>)l).Data);
					return ln;
				};
				_links.Add(typeof(UGraphLink<L,D>), Create);
			}
		}

        public UMachine InvokeMachine(string machineID)
        {
            UMachine machine = BuildMachine(machineID);
            machine?.OnEnterState();
            return machine;
        }

        private UMachine BuildMachine(string machineID)
        {
            if (_machines.TryGetValue(machineID, out DirectedGraph<UStateNode, UGraphLink> graph))
            {
                UMachine machine = new UMachine(this, graph);
                machine.Initialize(machineID);
                _active.Add(machine);
                return machine;
            }
            return null;
        }

        // Remove a machine from the active machines list.
        // Note that this does not fully dispose of the removed machine.
        internal void DectivateMachine(UMachine machine) => _active.Remove(machine);

        override public string ToString() => string.Join("; ", GetActiveMachines());

        public void Dispose()
        {
            foreach(DirectedGraph <UStateNode, UGraphLink> graph in _machines.Values)
            {
                graph.Dispose();
            }
            while (_active.Count > 0)
            {
                // UMachines remove themselves from UFlow's Active list
                _active[0].Dispose();
            }
            foreach(DirectedGraph<UStateNode, UGraphLink> graph in _machines.Values)
            {
                graph.Dispose();
            }
            _machines.Clear();
            _states.Clear();
        }

        public string[] Dump()
        {
            List<string> dump = new List<string>() { "UFlowSvc:" };
            foreach (UMachine machine in _active)
            {
                foreach (string line in machine.Dump())
                {
                    dump.Add( "  "+line );
                }
            }
            return dump.ToArray();
        }

        
        public void Invoke( string[] args )
        {
            ConsoleModel.warn("UFlowSvc has no invocation.");
        }    
    }
}
