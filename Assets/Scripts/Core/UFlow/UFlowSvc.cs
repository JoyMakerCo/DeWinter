<<<<<<< Updated upstream
using System;
=======
﻿using System;
>>>>>>> Stashed changes
using System.Collections.Generic;
using Core;
using Util;
using UGraph;

namespace UFlow
{
	public class UFlowSvc : IAppService, IConsoleEntity 
	{
        // Explicitly invoked machines.
        private List<UMachine> _machines = new List<UMachine>();

        // Machine definitions.
		private Dictionary<string, DirectedGraph<UStateNode, UGraphLink>> _graphs = new Dictionary<string, DirectedGraph<UStateNode, UGraphLink>>();

        public List<UMachine> GetMachines(string MachineID)
        {
            List<UMachine> result = new List<UMachine>();
            foreach(UMachine mac in _machines)
            {
                if (mac.MachineID == MachineID)
                {
                    result.Add(mac);
                }
                result.AddRange(mac.GetMachines(MachineID));
            }
            return result;
        }
        public List<UMachine> GetMachines()
        {
            List<UMachine> result = new List<UMachine>(_machines);
            foreach (UMachine mac in _machines)
            {
                result.AddRange(mac.GetMachines());
            }
            return result;
        }

        public List<string> GetMachineIDs()
        {
            List<UMachine> machines = GetMachines();
            List<string> result = new List<string>();
            machines.ForEach(mac => result.Add(mac.MachineID));
            return result;
        }

        public bool IsActiveMachine(string machineID) => GetMachineIDs().Contains(machineID);

        public void BindState<S>(string machineID, string stateID, params object[] parameters) where S:UState, new()
        {
            if (_graphs.TryGetValue(machineID, out DirectedGraph<UStateNode, UGraphLink> graph) && graph.Nodes != null)
            {
                int index = Array.FindIndex(graph.Nodes, n => n.ID == stateID);
                if (index >= 0)
                {
                    UStateNode n = graph.Nodes[index];
                    graph.Nodes[index] = new UStateNode<S>(n.ID, parameters.Length > 0 ? parameters : n.Parameters);
                };
            }
        }

        public void BindLink<L>(string machineID, string originState, string targetState, params object[] parameters) where L : ULink, new()
        {
            if (_graphs.TryGetValue(machineID, out DirectedGraph<UStateNode, UGraphLink> graph))
            {
                int from = Array.FindIndex(graph.Nodes, n => n.ID == originState);
                int to = Array.FindIndex(graph.Nodes, n => n.ID == targetState);
#if (DEBUG)
                if (from < 0) UnityEngine.Debug.LogError("UFLOW ERROR: No state called \"" + originState + "\" found in Machine \"" + machineID + "!");
                if (to < 0) UnityEngine.Debug.LogError("UFLOW ERROR: No state called \"" + targetState + "\" found in Machine \"" + machineID + "!");
#endif
                graph.Unlink(from, to);
                graph.Link(from, to, new UGraphLink<L>());
            }
#if (DEBUG)
            else UnityEngine.Debug.LogError("UFLOW ERROR: No Machine named \"" + machineID + "\" found!");
#endif
        }

        public void RegisterState(string machineID, string stateID, params object[] parameters)
		{
            UStateNode node = new UStateNode(stateID, parameters);
            if (!_graphs.TryGetValue(machineID, out DirectedGraph<UStateNode, UGraphLink> graph))
            {
                graph = new DirectedGraph<UStateNode, UGraphLink>();
                _graphs.Add(machineID, graph);
            }
            if (graph.Nodes == null)
            {
                graph.Nodes = new UStateNode[] { node };
            }
            else
            {
                List<UStateNode> ns = new List<UStateNode>(graph.Nodes);
                ns.Add(node);
                graph.Nodes = ns.ToArray();
            }
        }

        public void RegisterState<S>(string machineID, string stateID, params object[] parameters) where S:UState, new()
        {
            RegisterState(machineID, stateID, parameters);
            BindState<S>(machineID, stateID);
        }

        internal UState BuildNode(UStateNode node) => node._Instantiate();

        internal ULink BuildLink(UGraphLink ln) => ln._Instantiate();

        public void RegisterLink(string machineID, string originState, string targetState)
        {
            RegisterLinkUtil(machineID, originState, targetState, new UGraphLink());
        }

        public void RegisterLink<L>(string machineID, string originState, string targetState) where L : ULink, new()
		{
            RegisterLinkUtil(machineID, originState, targetState, new UGraphLink<L>());
        }

        private void RegisterLinkUtil(string machineID, string originState, string targetState, UGraphLink link)
        {
            if (_graphs.TryGetValue(machineID, out DirectedGraph<UStateNode, UGraphLink> graph))
            {
                int from = Array.FindIndex(graph.Nodes, n => n.ID == originState);
                int to = Array.FindIndex(graph.Nodes, n => n.ID == targetState);
                if (from < 0) UnityEngine.Debug.LogError("UFLOW ERROR: No state called \"" + originState + "\" found in Machine \"" + machineID + "!");
                if (to < 0) UnityEngine.Debug.LogError("UFLOW ERROR: No state called \"" + targetState + "\" found in Machine \"" + machineID + "!");
                graph.Link(from, to, link);
            }
            else UnityEngine.Debug.LogError("UFLOW ERROR: No Machine named \"" + machineID + "\" found!");
        }

        internal DirectedGraph<UStateNode, UGraphLink> GetMachineGraph(string machineID)
        {
            return _graphs.TryGetValue(machineID, out DirectedGraph<UStateNode, UGraphLink> graph)
                ? graph
                : null;
        }

        public UMachine InvokeMachine(string machineID)
		{
            DirectedGraph<UStateNode, UGraphLink> graph = GetMachineGraph(machineID);
            if (graph == null) return null;
            UMachine machine = new UMachine(machineID, graph);
            machine._UFlow = this;
            _machines.Add(machine);
            machine.OnEnterState();
			return machine;
		}

        // Remove a machine from the active machines list.
        internal bool Remove(UMachine machine)
        {
            if (_machines.Remove(machine))
            {
                machine.Dispose();
                return true;
            }
            return false;
        }

        override public string ToString() => string.Join("; ", GetMachines());

        public void Dispose()
        {
            Reset();
            foreach(DirectedGraph <UStateNode, UGraphLink> graph in _graphs.Values)
            {
                graph.Dispose();
            }
            _graphs.Clear();
            _graphs = null;
        }

        // Disposes of all currently active machines;
        // Does not clear graph definitions
        public void Reset()
        {
            GetMachines().ForEach(m => m.Dispose());
            _machines = new List<UMachine>();
        }

        // Combs over every machine to check for an activre state.
        // This could bee a potentially expnsive operation...
        public bool IsActiveState(string stateID)
        {
            foreach(UMachine machine in _machines)
            {
                if (machine.ID == stateID || FindActiveState(machine, stateID))
                {
                    return true;
                }
            }
            return false;
        }

        private bool FindActiveState(UMachine machine, string stateID)
        {
            if (machine.IsActiveState(stateID)) return true;
            List<UMachine> machines = machine.GetMachines();
            foreach(UMachine mac in machines)
            {
                if (FindActiveState(mac, stateID)) return true;
            }
            return false;
        }

        public string[] Dump()
        {
            var lines = new List<string>()
            {
                "UFlowSvc:",
            };

            foreach (var machine in _machines)
            {
                if (machine != null)
                {
                    foreach (var line in machine.Dump())
                    {
                        lines.Add("  " + line);
                    }
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
