using System;
using System.Collections.Generic;
using Core;
using Util;
using UGraph;


using UnityEngine;

namespace UFlow
{
    [Serializable]
	public sealed class UMachine : IDisposable, IConsoleEntity
	{
		public string MachineID { get; private set; }
        internal UMachineState _State;

		internal UFlowSvc _UFlow;	// Active UFlow Service
		internal DirectedGraph<UStateNode, UGraphLink> _graph;
        internal uint _exitStates = 0;

        private UNode[] _nodes;
        private ULink[][] _links;
        private Queue<int> _queue = new Queue<int>();
        private bool _activeQueue = false;
        private Dictionary<string, Func<bool>> _decisions = new Dictionary<string, Func<bool>>();

        public UMachine(string machineID=null, DirectedGraph<UStateNode, UGraphLink> graph=null)
        {
            int length = graph.Nodes.Length;

            MachineID = machineID;
            _graph = graph;

            _nodes = new UNode[length];
            _links = new ULink[length][];

            // Temporarily inferring Exit States from links
            // In the future, the UI will automatically make states with no links
            //  appear as exit states, and provide the option to toggle this condition off
            _exitStates = ~(uint)0;
            Array.ForEach(_graph.Links, l => _exitStates &= ~(uint)(1 << l.x));
        }

        internal void Start()
        {
            if (_UFlow.Activate(this))
                Activate(0);
        }

        // Current active states
        public string[] GetActiveStates()
        {
            List<string> result = new List<string>();
            foreach(UNode n in _nodes)
            {
                if (n != null)
                {
                    result.Add(n.ID);
                }
            }
            return result.ToArray();
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
        internal void ActivateLinks(UNode state)
        {
            int index = Array.IndexOf(_nodes, state);
            if (index >= 0 && _links?[index] != null)
            {
                // Find all links that are valid
                ULink[] links = Array.FindAll(_links[index], l => l?.Validate() ?? false);
                if (Array.TrueForAll(links, l => l is UDefaultLink))
                {
                    Array.ForEach(links, Activate);
                }
                else
                {
                    foreach (ULink link in links)
                    {
                        if (!(link is UDefaultLink))
                        {
                            Activate(link);
                        }
                    }
                }
            }
        }

        private bool isExitState(int index) => (_exitStates & (1 << index)) > 0;

        internal void Activate(UInputState state) => ActivateLinks(state);
        internal void Activate(UMachineState state) => ActivateLinks(state);

        private void Activate(int index)
        {
            if (_activeQueue) _queue.Enqueue(index);
            else
            {
                UNode node;
                _activeQueue = true;
                do
                {
                    node = BuildNode(index);
                    node.OnEnterState();
Debug.Log(node._Machine?.MachineID + " " + node.ID);
                    // Exit the machine if the current node is marked as an exit state.
                    if (isExitState(index))
                    {
                        ExitState(index);
                        ExitMachine();
                    }
                    // TODO
                    //switch(_graph.Nodes[index].Type)
                    //{
                        //case UNodeType.Exit:
                            //ExitState(index);
                            //ExitMachine();
                            //break;
                        //case UNodeType.State:
                        //case UNodeType.Decision:
                    else if (node is UState || node is UDecisionState)
                    {
                        ActivateLinks(node);
                    }
                    _activeQueue = _queue?.Count > 0;
                    if (_activeQueue) index = _queue.Dequeue();
                } while (_activeQueue);
                // TODO:
                // - In the UI, outgoing links are shown in a reorderable list where list order = execution order

                // Exit the machine if nothing else is happening.
                if (_nodes != null && Array.TrueForAll(_nodes, n => n == null))
                {
                    ExitMachine();
                }
            }
        }

        private UNode BuildNode(int index)
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
                UNode node = _UFlow.BuildNode(_graph.Nodes[index]);
                node.ID = _graph.Nodes[index].ID;
                node._UFlow = _UFlow;
                node._Machine = this;
                _nodes[index] = node;
            }
            return _nodes[index];
        }

        private void ExitMachine()
        {
            if (_State?._Machine == null) Dispose();
            else _State._Machine.Activate(_State);
        }

        private void ExitState(int index)
		{
            ULink[] links = _links?[index];
            if (links != null)
            {
                Array.ForEach(links, l => l?.Dispose());
                _links[index] = null;
            }

            if (_nodes != null)
            {
                (_nodes[index] as UState)?.OnExitState();
                _nodes[index]?.Dispose();
                _nodes[index] = null;
            }
		}

        // Deallocate memory and get ready for garbage collection.
        // May be called by the machine itself or by UFlowSvc.
		public void Dispose()
		{
            if (_UFlow.Remove(this))
            {
                if (_links != null)
                    foreach (ULink[] links in _links)
                        if (links != null)
                            Array.ForEach(links, l => l.Dispose());
                _links = null;

                if (_nodes != null)
                    Array.ForEach(_nodes, s => s?.Dispose());
                _nodes = null;

                _queue?.Clear();
                _queue = null;

                _graph = null; // Don't destroy the graph! This is a reference from the Machine library

                _activeQueue = false;
                _State = null;
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
            var stateId = "null";
            if (_State != null)
            {
                stateId = _State.ID;
            }

            var lines = new List<string>()
            {
                "UMachine "+MachineID+":",
                "state: "+stateId
            };

            return lines.ToArray();
        }

        
        public void Invoke( string[] args )
        {
            ConsoleModel.warn("UMachine has no invocation.");
        }  
	}
}
