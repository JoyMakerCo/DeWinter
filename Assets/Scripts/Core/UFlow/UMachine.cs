﻿using System;
using System.Collections.Generic;
using Core;
using Util;
using UGraph;

using UnityEngine;

namespace UFlow
{
    [Serializable]
    public sealed class UMachine : UState, IInitializable<string>, IConsoleEntity, IDisposable
    {
        public string MachineID { get; private set; }
        public bool IsActive => _graph != null;

        private UFlowSvc _UFlow;
        private DirectedGraph<UStateNode, UGraphLink> _graph = null;
        private Dictionary<int, UState> _states = new Dictionary<int, UState>();
        private Dictionary<int, ULink[]> _inputs = new Dictionary<int, ULink[]>();
        private Dictionary<ULink, int> _decisions = new Dictionary<ULink, int>();
        private Queue<int> _queue = new Queue<int>();

        public UMachine() { }
        public UMachine(UFlowSvc uflow, DirectedGraph<UStateNode, UGraphLink> graph)
        {
            _UFlow = uflow;
            _graph = graph;
        }

        public void Initialize(string machineID) => MachineID = machineID;
        public override void OnEnterState() => Activate(0);

        // Deallocate memory and get ready for garbage collection.
        // May be called by the machine itself or by UFlowSvc.
        public void Dispose()
        {
            _UFlow.DectivateMachine(this);
            _queue = null;
            _graph = null; // Don't destroy the graph! This is a reference from the Machine library
            _decisions = null;
            if (_states != null)
            {
                foreach (UState node in _states.Values)
                {
                    (node as IDisposable)?.Dispose();
                }
                _states.Clear();
                _states = null;
            }
            if (_inputs != null)
            {
                foreach (ULink[] inputs in _inputs.Values)
                {
                    Array.ForEach(inputs, i => (i as IDisposable)?.Dispose());
                }
                _inputs.Clear();
                _inputs = null;
            }
        }

        // Current active states
        public string[] GetActiveStates()
        {
            List<string> result = new List<string>();
            foreach (UState n in _states.Values)
                if (n != null) result.Add(n.ID);
            return result.ToArray();
        }

        public bool RestoreState(IEnumerable<int> states, Dictionary<int, UMachine> controllers=null)
        {
            UMachine controller;
            UState state;
            int[] neighbors;
            ULink link;
            List<ULink> links;

            foreach (ushort index in states)
            {
                if (controllers != null && controllers.TryGetValue(index, out controller))
                {
                    state = controller;
                }
                else state = _UFlow.Instantiate(_graph.Nodes[index]);
                if (state == null) return false;
                state.ID = _graph.Nodes[index].ID;
                state._Machine = this;
                _states[index] = state;

                neighbors = _graph?.GetNeighbors(index);
                if (neighbors?.Length > 0)
                {
                    links = new List<ULink>();
                    foreach (int neighbor in neighbors)
                    {
                        link = _UFlow.Instantiate(_graph.GetLinkData(index, neighbor));
                        if (link != null && !link.Validate())
                        {
                            link._origin = index;
                            link._target = neighbor;
                            link._machine = this;
                            links.Add(link);
                        }
                        _inputs[index] = links.ToArray();
                    }
                }
            }
            return true;
        }

        public int[] SaveState(out Dictionary<int, UMachine> dependencies)
        {
            List<int> result = new List<int>();
            dependencies = new Dictionary<int, UMachine>();
            foreach (KeyValuePair<int, UState> state in _states)
            {
                result.Add((ushort)(state.Key));
                if (state.Value is UMachine)
                {
                    dependencies.Add(state.Key, (UMachine)(state.Value));
                }
            }
            return result.ToArray();
        }

        public string[] Dump()
        {
            return new string[]
            {
                "UFlow "+MachineID+": ",
                "stateID: "+ID,
                "active: " + string.Join(", ", GetActiveStates())
            };
        }

        public void Invoke(string[] args)
        {
            ConsoleModel.warn("UFlow has no invocation.");
        }


        // INTERNAL METHODS //////////////

        internal void Activate(ULink link)
        {
            if (link == null) return;
            int origin = link._origin;
            int target = link._target;
            ExitState(origin);
            if (_inputs.TryGetValue(origin, out ULink[] links))
            {
                foreach(ULink input in links)
                {
                    _decisions.Remove(input);
                    (input as IDisposable)?.Dispose();
                }
                _inputs.Remove(origin);
            }
            Activate(target);
        }

        internal void Activate(UInput input)
        {
            input?.OnActivate();
            ActivateLinks(_FindIndex(input));
        }

        /////////// Private/Protected

        private void ExitState(int index)
        {
            if (_states != null && _states.TryGetValue(index, out UState state))
            {
                (state as IDisposable)?.Dispose();
                _states.Remove(index);
            }
        }

        private UState InitState(int index)
        {
            if (!_states.TryGetValue(index, out UState state) || state == null)
            {
                _states[index] = state = _UFlow.Instantiate(_graph.Nodes[index]);
                if (state != null)
                {
                    state.ID = _graph.Nodes[index].ID;
                    state._Machine = this;
                    _states[index] = state;
                }
            }
#if DEBUG
            if (state == null) Debug.Log("UFLOW ERROR: Attempted to instantiate State at index " + index.ToString() + " in Flow " + MachineID);
#endif
            return state;
        }


        // Activates all links that aren't waiting for inputs for node at given index.
        private void ActivateLinks(int index)
        {
            int[] neighbors = _graph?.GetNeighbors(index);
            if (neighbors?.Length == 0)
            {
                ExitState(index);
                return;
            }
            List<int> defaults = new List<int>();
            List<ULink> inputs = new List<ULink>();
            ULink link;

            foreach (int i in neighbors)
            {
                if (IsActive)
                {
                    link = _UFlow.Instantiate(_graph.GetLinkData(index, i));
                    if (link == null) defaults?.Add(i);
                    else
                    {
                        link._origin = index;
                        link._target = i;
                        link._machine = this;
                        if (link.Validate())
                        {
                            defaults = null;
                            inputs = ClearLinks(inputs);
                            (link as IDisposable)?.Dispose();
                            ExitState(index);
                            Activate(i);
                        }
                        else inputs?.Add(link);
                    }
                }

                // Machine might become inactive during link evaluation;
                // Bow out if that happens
                if (!IsActive || neighbors == null)
                {
                    //bail out if the Machine is inactive
                    ClearLinks(inputs);
                    return;
                }
            }
            if (defaults?.Count > 0)
            {
                ExitState(index);
                inputs = ClearLinks(inputs);
                defaults.ForEach(Activate);
                defaults.Clear();
            }
            if (inputs?.Count > 0)
            {
                if (_inputs != null) _inputs[index] = inputs.ToArray();
                inputs.Clear();
            }
        }
        
        private List<ULink> ClearLinks(List<ULink> links)
        {
            if (links != null)
            {
                links.ForEach(l => (l as IDisposable)?.Dispose());
                links.Clear();
            }
            return null;
        }

        private void Activate(int index)
        {
            // Early out; handles cases when the machine has exited.
            if (index < 0 || index > _graph?.Nodes?.Length) return;

            _queue.Enqueue(index);
            if (_queue.Count == 1) 
            {
                UState state;
                do
                {
#if DEBUG
                    if (_graph?.Nodes == null)
                    {
                        Debug.Log("ERR: UFlow: Flow continued operation after disposal; MachineID: " + MachineID);
                        return;
                    }
#endif
                    index = _queue.Dequeue();
                    state = InitState(index);

                    if (state is UMachine)
                    {
                        ((UMachine)state)._UFlow = _UFlow;
                        state.OnEnterState();
                    }
                    else
                    {
                        state?.OnEnterState();
                        ActivateLinks(index);
                    }
                } while (_queue?.Count > 0);
                if (_states?.Count == 0)
                {
                    Exit();
                }
            }
        }

        private int _FindIndex(UState state)
        {
            if (state != null && _states != null)
            {
                foreach(KeyValuePair<int, UState> kvp in _states)
                {
                    if (kvp.Value == state) return kvp.Key;
                }
            }
            return -1;
        }

        private int _FindIndex(string stateID)
        {
            if (_graph?.Nodes != null)
            {
                for (int i=_graph.Nodes.Length-1; i>=0; --i)
                {
                    if (_graph.Nodes[i]?.ID == stateID) return i;
                }
            }
            return -1;
        }

        private void Exit()
        {
            if (_Machine != null)
            {
                foreach (KeyValuePair<int, UState> kvp in _Machine._states)
                {
                    if (kvp.Value == this)
                    {
                        _Machine.ExitState(kvp.Key);
                        _Machine.ActivateLinks(kvp.Key);
                        return;
                    }
                }

            }
            Dispose();
        }
    }
}
