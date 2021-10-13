using System;
using System.Collections.Generic;
using Core;
using Util;
using UGraph;

namespace UFlow
{
    public class UFlowSvc : IAppService
    {
        // Active flows.
        private List<UMachine> _active = new List<UMachine>();

        // Flow definitions.
        private Dictionary<string, DirectedGraph<UStateNode>> _flows = new Dictionary<string, DirectedGraph<UStateNode>>();

        public void Register<D>(string flowID) where D : UFlowConfig, new()
        {
            _flows[flowID] = (new D()).OnRegister(this, flowID);
        }

        public DirectedGraph<UStateNode> GetGraph(string flowID)
        {
            _flows.TryGetValue(flowID, out DirectedGraph<UStateNode> graph);
            return graph;
        }

        public bool FlowExists(string flowID) => _flows.ContainsKey(flowID);

        public string Save()
        {
            Dictionary<string, List<UMachine>> machines = new Dictionary<string, List<UMachine>>();
            List<string> output = new List<string>();
            List<string> line;
            string flowID;
            UMachine umachine;
            int[] state;
            Dictionary<int, UMachine> dependencies;
            foreach (UMachine machine in _active)
            {
                flowID = GetAddFlowID(machines, machine);
                line = new List<string>() { flowID };
                state = machine.SaveState(out dependencies);
                foreach (int i in state)
                {
                    // Subflows 
                    if (dependencies.TryGetValue(i, out umachine))
                    {
                        flowID = GetAddFlowID(machines, umachine);
                        line.Add(i.ToString() + ":" + flowID);
                    }
                    else
                    {
                        line.Add(i.ToString());
                    }
                    if (line.Count > 1) // Don't bother saving if the flow has no active states
                    {
                        output.Add(string.Join(",", line));
                    }
                }
            }
            return string.Join("\n", output);
        }

        public void Reset()
        {
            if (_active == null) _active = new List<UMachine>();
            else while (_active.Count > 0) _active[0].Dispose();
        }

        public bool Restore(string uflowState)
        {
            bool complete;
            string[] machineState = uflowState.Split('\n');
            string[][] state = new string[machineState.Length][];
            Dictionary<string, UMachine> machines = new Dictionary<string, UMachine>();
            Dictionary<int, UMachine> subflow = new Dictionary<int, UMachine>();
            UMachine serializedMachine;

            for (int i = machineState.Length - 1; i >= 0; --i)
            {
                state[i] = machineState[i].Split(',');
            }

            for (complete = true; !complete; )
            {
                foreach (string[] mstate in state)
                {
                    if (!machines.ContainsKey(mstate[0]))
                    {
                        serializedMachine = RestoreFlow(mstate, machines);
                        if (serializedMachine != null)
                        {
                            machines.Add(mstate[0], serializedMachine);
                        }
                        else complete = false;
                    }
                }
            }
            Reset();
            _active = new List<UMachine>(machines.Values);
            Execute();
            return true;
        }

        public void Execute()
        {
            for (int i = 0; i < _active.Count; ++i)
                _active[i].Execute();
        }

        public UMachine GetFlow(string MachineID, ushort instance = 0)
        {
            List<UMachine> result = _active.FindAll(m => m.FlowID == MachineID);
            return instance < result.Count ? result[instance] : null;
        }

        public UMachine[] GetFlows(string MachineID)
        {
            return _active.FindAll(m => m.FlowID == MachineID).ToArray();
        }

        public string[] GetActiveFlows()
        {
            string[] result = new string[_active.Count];
            for (int i = _active.Count - 1; i >= 0; --i)
                result[i] = _active[i]?.FlowID;
            return result;
        }

        public UMachine[] GetAllFlows() => _active.ToArray();
        public bool IsActiveState(string stateID) => _active.Exists(m => Array.Exists(m.GetActiveStates(), s => s == stateID));
        public bool IsActiveFlow(string flowID) => _active.Exists(m => m.FlowID == flowID);
        public bool Exit(string flowID) => Exit(_active.Find(m => m.FlowID == flowID));
        public bool Exit(UMachine flow)
        {
            flow?.Exit();
            return flow != null;
        }

        public bool Unregister(string flowID)
        {
            _active.FindAll(m => m.FlowID == flowID).ForEach(m=>m.Exit());
            if (_flows.TryGetValue(flowID, out DirectedGraph<UStateNode> graph))
                graph?.Dispose();
            return _flows.Remove(flowID);
        }

        public UMachine Instantiate(string flowID, UMachine flow = null)
        {
            if (!_flows.ContainsKey(flowID)) return null;
            UMachine result = new UMachine(this) { _Flow = flow };
            result.Initialize(flowID);
            _active.Add(result);
            return result;
        }

        public UMachine Invoke(string flowID, UMachine flow = null)
        {
            UMachine result = Instantiate(flowID, flow);
            result?.OnEnter();
            return result;
        }

        // PRIVATE / PROTECTED METHODS //////////////////////

        // When saving a UFlow state, number flows of the same type.
        private string GetAddFlowID(Dictionary<string, List<UMachine>> flows, UMachine flow)
        {
            string flowID = flow?.FlowID;
            if (string.IsNullOrEmpty(flowID) || !_flows.ContainsKey(flowID)) return null;
            if (!flows.ContainsKey(flowID))
            {
                flows.Add(flowID, new List<UMachine>() { flow });
                return flowID + "#0";
            }
            int index = flows[flowID].IndexOf(flow);
            if (index < 0)
            {
                index = flows[flowID].Count;
                flows[flowID].Add(flow);
            }
            return flowID + "#" + index;
        }

        private UMachine RestoreFlow(string[] state, Dictionary<string, UMachine> machines)
        {
            // Corrupted state; early out
            int len = state?.Length ?? 0;
            if (len < 1) return null;
            DirectedGraph<UStateNode> graph;
            string[] stateStr = state[0].Split('#');

            // Couldn't find graph; early out
            if (stateStr.Length < 1 || !_flows.TryGetValue(stateStr[0], out graph))
            {
                return null;
            }

            string machineID = stateStr[0];
            int[] init = new int[len - 1];
            Dictionary<int, UMachine> subflows = new Dictionary<int, UMachine>();
            UMachine machine;
            UMachine result = Instantiate(machineID);

            for (int i = 0; i < len - 1; ++i)
            {
                stateStr = state[i + 1].Split(':');
                init[i] = int.Parse(stateStr[0]);
                if (stateStr.Length > 1)
                {
                    // subflows are states that contain other machines
                    // subflows need to be rebuilt before flows that depend on them
                    if (machines.TryGetValue(stateStr[1], out machine))
                    {
                        machine._Flow = result;
                        machine.ID = graph.Nodes[init[i]].ID;
                        subflows[init[i]] = machine;
                    }
                    else return null; // Not all machines instantiated; early out
                }
            }

            if (result.RestoreStates(state, subflows))
            {
                return result;
            }
            result.Dispose();
            return null;
        }

        // Remove a machine from the active machines list.
        // Note that this does not fully dispose of the removed machine.
        internal void DectivateMachine(UMachine machine) => _active.Remove(machine);

        public void Dispose()
        {
            foreach(DirectedGraph <UStateNode> graph in _flows.Values)
            {
                graph.Dispose();
            }
            while (_active.Count > 0)
            {
                // UMachines remove themselves from UFlow's Active list
                _active[0].Dispose();
            }
            foreach(DirectedGraph<UStateNode> graph in _flows.Values)
            {
                graph.Dispose();
            }
            _flows.Clear();
        }

        public override string ToString()
        {
            List<string> result = new List<string>();
            foreach (UMachine machine in _active)
                result.Add(machine.ToString());
            return "UFlowSvc:\n" +  string.Join("\n", result.ToArray());
        }
    }
}
