using System;
using System.Collections.Generic;
using Core;
using Util;
using UGraph;

using UnityEngine;

namespace UFlow
{
    [Serializable]
	public sealed class UMachine : UInputState, IConsoleEntity, IDisposable
	{
		public string MachineID { get; private set; }

		private DirectedGraph<UStateNode, UGraphLink> _graph;

        private UState[] _nodes;
        private ULink[][] _links;
        private Queue<int> _queue = new Queue<int>();
        private Dictionary<string, Func<bool>> _decisions = new Dictionary<string, Func<bool>>();

        public UMachine() { }
        public UMachine(string machineID, DirectedGraph<UStateNode, UGraphLink> graph=null)
        {
            MachineID = machineID;
            _graph = graph ?? _UFlow.GetMachineGraph(MachineID);
            if (_graph != null)
            {
                int length = _graph.Nodes.Length;
                _nodes = new UState[length];
                _links = new ULink[length][];
            }
        }

        public override void Initialize(params object[] parameters)
        {
            MachineID = parameters[0] as string;
            _graph = _UFlow.GetMachineGraph(MachineID);
            if (_graph != null)
            {
                int length = _graph.Nodes.Length;
                _nodes = new UState[length];
                _links = new ULink[length][];
            }
        }

        public override void OnEnterState()
        {
            if (_graph != null && _graph.Nodes.Length > 0)
            {
                Activate(0);
            }
            else
            {
                Activate();
            }
        }

        // Current active states
        public string[] GetActiveStates()
        {
            List<string> result = new List<string>();
            foreach(UState n in _nodes)
            {
                if (n != null)
                {
                    result.Add(n.ID);
                }
            }
            return result.ToArray();
        }

        // Current Active Machines
        public List<UMachine> GetMachines()
        {
            List<UMachine> machines = new List<UMachine>();
            foreach (UState s in _nodes)
            {
                if (s is UMachine)
                {
                    machines.Add((UMachine)s);
                    machines.AddRange(GetMachines());
                }
            }
            return machines;
        }

        public List<UMachine> GetMachines(string machineID)
        {
            List<UMachine> machines = new List<UMachine>();
            foreach (UState s in _nodes)
            {
                if (s is UMachine)
                {
                    if (((UMachine)s).MachineID == machineID)
                    {
                        machines.Add((UMachine)s);
                        machines.AddRange(GetMachines(machineID));
                    }
                }
            }
            return machines;
        }

        public bool IsActiveState(string stateID)
        {
            foreach(UState state in _nodes)
            {
                if (state?.ID == stateID) return true;
            }
            return false;
        }

        // TODO: Links no longer activate, only states
        internal void Activate(ULink link)
		{
			ExitState(link._origin);
            Activate(link._target);
		}

        // Builds links for the state and follows the valid ones
        // Called immediately for UDecisions and UStates,
        // and upon an appropriate event for UMachineStates and UDecisions
        private void ActivateLinks(UState state)
        {
            int index = Array.IndexOf(_nodes, state);
            if (index < 0) return;

            if (_links[index]?.Length > 0)
            {
                List<UDefaultLink> defaults = new List<UDefaultLink>();
                foreach (ULink link in _links[index])
                {
                    if (link is UDefaultLink)
                    {
                        defaults?.Add((UDefaultLink)link);
                    }
                    else if (link.Validate())
                    {
                        Activate(link);
                        defaults?.Clear();
                        defaults = null;
                    }
                    defaults?.ForEach(Activate);
                }
            }
            else ExitState(index);
        }

        internal void ActivateInput(UInputState state) => ActivateLinks(state);

        private void Activate(int index)
        {
            if (_queue == null || _nodes == null || index < 0 || index >= _nodes.Length) return;
            if (_queue.Count > 1)
            {
                _queue.Enqueue(index);
                return;
            }
            UState node;
            do
            {
                node = BuildNode(index);
Debug.Log("Entering State " + node.ID);
                node.OnEnterState();
                if (!(node is UInputState))
                {
                    ActivateLinks(node);
                }
                index = _queue != null && _queue.Count > 0 ? _queue.Dequeue() : -1;
            }
            while (index >= 0);

            if (_nodes == null || _queue == null || Array.TrueForAll(_nodes, n => n == null))
            {
                Activate();
            }
        }

        private UState BuildNode(int index)
        {
            if (_links[index] == null)
            {
                UGraphLink[] links = _graph.GetLinks(index);
                _links[index] = new ULink[links.Length];
                for (int i=links.Length-1; i>=0; --i)
                {
                    _links[index][i] = BuildLink(links[i]);
                }
            }
            if (_nodes[index] == null)
            {
                UState node = _UFlow.BuildNode(_graph.Nodes[index]);
                node.ID = _graph.Nodes[index].ID;
                node._UFlow = _UFlow;
                node._Machine = this;
                _nodes[index] = node;
                node.Initialize(_graph.Nodes[index].Parameters);
            }
            return _nodes[index];
        }

        private void ExitState(int index)
		{
            ULink[] links = _links?[index];
            if (links != null)
            {
                Array.ForEach(links, l => l?.Dispose());
                _links[index] = null;
            }

            if (_nodes != null && _nodes[index] != null)
            {
                _nodes[index].OnExitState();
                _nodes[index].Dispose();
                _nodes[index] = null;
            }
		}

        // Deallocate memory and get ready for garbage collection.
        // May be called by the machine itself or by UFlowSvc.
        override public void Dispose()
		{
            if (_UFlow.Remove(this))
            {
                if (_links != null)
                {
                    foreach (ULink[] links in _links)
                    {
                        if (links != null)
                        {
                            Array.ForEach(links, l => l.Dispose());
                        }
                    }
                    _links = null;
                }

                if (_nodes != null)
                {
                    Array.ForEach(_nodes, s => s?.Dispose());
                    _nodes = null;
                }

                _queue?.Clear();
                _queue = null;
                _graph = null; // Don't destroy the graph! This is a reference from the Machine library
            }
        }

        // Build a link from the graph data.
		private ULink BuildLink(UGraphLink link)
		{
			ULink ln = _UFlow.BuildLink(link);
			int linkIndex = Array.IndexOf(_graph.LinkData, link);
			ln._origin = _graph.Links[linkIndex].x;
			ln._target = _graph.Links[linkIndex].y;
			ln._machine = this;
			ln.Initialize();
			return ln;
		}


        public string[] Dump()
        {
            string stateId = _Machine?.MachineID ?? "null";
            return new string[]
            {
                "UMachine "+MachineID+":",
                "state: "+stateId
            };
        }
        
        public void Invoke( string[] args )
        {
            ConsoleModel.warn("UMachine has no invocation.");
        }  
	}
}
