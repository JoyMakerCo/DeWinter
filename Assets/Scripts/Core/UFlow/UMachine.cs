using System;
using System.Collections.Generic;
using Core;
using Util;
using UGraph;

using UnityEngine;

namespace UFlow
{
    [Serializable]
    public sealed class UMachine : UInput, IInitializable<string>, IDisposable
    {
        public string FlowID { get; private set; }

        private DirectedGraph<UStateNode> _graph = null;
        private Dictionary<int, UState> _states = new Dictionary<int, UState>();
        private Queue<int> _queue = new Queue<int>();
        private bool _active = false;

        internal UFlowSvc _UFlow;

        public UMachine() { }
        public UMachine(UFlowSvc uflow) => _UFlow = uflow;

        public override void OnEnter()
        {
            _queue.Enqueue(0);
            Execute();
        }

        public void Initialize(string flowID) => _graph = _UFlow.GetGraph(FlowID = flowID);

        // Deallocate memory and get ready for garbage collection.
        // May be called by the machine itself or by UFlowSvc.
        public void Dispose()
        {
            _UFlow?.DectivateMachine(this);
            _queue?.Clear(); _queue = null;
            _graph = null; // Don't destroy the graph! This is a reference from the Machine library
            ClearStates();
            _states = null;
            _Flow = null;
            _UFlow = null;
        }

        // Current active states
        public string[] GetActiveStates()
        {
            List<string> result = new List<string>();
            foreach (UState n in _states.Values)
                if (n != null) result.Add(n.ID);
            return result.ToArray();
        }

        /// <summary>Manually restore a Flow to a specified state.</summary>
        /// <param name="states">The IDs of the states to restore.</param>
        /// <param name="dependencies">Flows that have already been built.</param>
        /// <exception cref="MissingReferenceException">Specified state ID doesn't exist in the named Flow.</exception>
        public bool RestoreStates(string[] states, Dictionary<int, UMachine> dependencies)
        {
            int index;
            Reset();
            UState dependency;
            foreach(KeyValuePair<int, UMachine> kvp in dependencies)
            {
                _states[kvp.Key] = kvp.Value;
            }
            foreach (string stateID in states)
            {
                index = Array.FindIndex(_graph.Nodes, n => n.ID == stateID);
                if (index < 0)
                {
                    throw new MissingReferenceException("UFlow Exception: State \'" + stateID + "\' not found in Flow \'" + FlowID + "\'");
                }
                dependency = InitState(index);
                if (!(dependency is UInput))
                {
                    QueueLinks(index);
                }
                else if (dependency is UMachine)
                {
                    dependencies[index] = dependency as UMachine;
                }
            }
            return true;
        }

        public UState GetState(string stateID)
        {
            if (_states == null) return null;
            foreach(UState state in _states.Values)
            {
                if (state.ID == stateID)
                    return state;
            }
            return null;
        }

        public override string ToString()
        {
            return "UFlow " + FlowID +
                   "\n StateID " + ID +
                   "\n Active States: " + string.Join(", ", GetActiveStates());
        }

        // INTERNAL METHODS //////////////

        // The main UFlow execution loop
        internal void Execute()
        {
            if (_active || _queue?.Count == 0) return;
            UState state;
            int index;
            for (_active = true; _queue?.Count > 0; _active = _queue?.Count > 0)
            {
                index = _queue.Dequeue();
                state = InitState(index);
                Debug.Log("UFLOW>> " + FlowID + "::" + state?.ID);
                state?.OnEnter();

                // Handle Decisions
                if (state is UDecision)
                {
                    int[] decision = _graph?.GetNeighbors(index);
                    if (decision?.Length > 0)
                    {
                        int next = (decision.Length == 1 || (state as UDecision).Validate())
                            ? decision[0]
                            : decision[1];
                        _queue?.Enqueue(next);
                        ExitState(index);
                    }
                }
                // Only activate links for non-input states
                // Do not exit input states
                else if (!(state is UInput))
                {
                    ExitState(index);
                    QueueLinks(index);
                    Execute();
                }
            }
            // If no more states are queued and no inputs are awaited, exit the Flow
            if (_states?.Count == 0) Exit();
        }

        internal void Exit()
        {
            Debug.Log("Exiting: " + FlowID);
            if (_Flow != null) _Flow.Activate(this);
            else Dispose();
        }

        internal void Activate(UInput input)
        {
Debug.Log("UFlow: Input>> " + input?.ID);
            (input as IExitState)?.OnExit();
            (input as IDisposable)?.Dispose();
            if (_states == null || _graph == null) return;
            int index = -1;
            foreach (KeyValuePair<int, UState> kvp in _states)
            {
                if (kvp.Value == input)
                {
                    index = kvp.Key;
                    _states.Remove(index);
                     break;
                }
            }
            input = null;
            if (index >= 0)
            {
                // when exiting an input, dispose of all sibling inputs as well.
                // This means finding all states that direct to the current input
                // and removing all active inputs that are directed from those states.
                Vector2Int[] links = Array.FindAll(_graph.Links, l => l.y == index);
                UState state;
                int[] neighbors;
                // The x coordinate of these links are the sources of the input.
                for (int i = links.Length - 1; i >= 0; --i)
                {
                    // Find all neighbors of the current source.
                    neighbors = _graph.GetNeighbors(links[i].x);
                    foreach (int neighbor in neighbors)
                    {
                        // ONLY dispose of input states.
                        _states.TryGetValue(neighbor, out state);
                        if (state is UInput)
                        {
Debug.Log("Removing parallel input: " + state.ID);
                            (state as IDisposable)?.Dispose();
                            _states.Remove(neighbor);
                        }
                    }
                }
                // Queue links from the input and continue execution.
                QueueLinks(index);
                Execute();
            }
        }

        internal int[] SaveState(out Dictionary<int, UMachine> dependencies)
        {
            dependencies = new Dictionary<int, UMachine>();
            foreach (KeyValuePair<int, UState> input in _states)
            {
                if (input.Value is UMachine)
                {
                    dependencies.Add(input.Key, (UMachine)(input.Value));
                }
            }
            return new List<int>(_states.Keys).ToArray();
        }

        /////////// Private/Protected

        private void ExitState(int index)
        {
            UState state = null;
            _states?.TryGetValue(index, out state);
            (state as IExitState)?.OnExit();
            (state as IDisposable)?.Dispose();
            _states?.Remove(index);
        }

        private void QueueLinks(int index)
        {
            if (_graph != null && _queue != null)
            {
                int[] neighbors = _graph.GetNeighbors(index);
                Array.ForEach(neighbors, _queue.Enqueue);
            }
        }

        private UState InitState(int index)
        {
            if (_states.TryGetValue(index, out UState state) && state != null)
                return state;
            state = _graph.Nodes[index].Instantiate(this);
            _states[index] = state;
            return state;
        }

        private void Reset()
        {
            if (_queue != null) _queue.Clear();
            else _queue = new Queue<int>();
            if (_states == null) _states = new Dictionary<int, UState>();
            else ClearStates();
            _active = false;
        }

        private void ClearStates()
        {
            if (_states != null)
            {
                foreach (UState state in _states.Values)
                {
                    (state as IDisposable)?.Dispose();
                }
                _states.Clear();
            }
        }
    }
}
