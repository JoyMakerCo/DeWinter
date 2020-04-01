using System;
using System.Collections.Generic;
namespace UFlow
{
    public abstract class UFlowBindings
    {
        internal Dictionary<string, Action> _states = new Dictionary<string, Action>();
        internal Dictionary<string, Action> _cleanup = new Dictionary<string, Action>();
        internal Dictionary<string, Func<bool>> _decisions = new Dictionary<string, Func<bool>>();
        internal Dictionary<string, Func<string>> _switches = new Dictionary<string, Func<string>>();
        public string MachineID;

        protected UFlowBindings(string machineID) => MachineID = machineID;

        protected void State(string StateID, Action action)
        {
            if (!_states.ContainsKey(StateID))
                _states[StateID] = action;
            else _states[StateID] += action;
         }

        protected void Decision(string StateID, Func<bool> check)
        {
            if (!_states.ContainsKey(StateID))
                _decisions[StateID] = check;
            else _decisions[StateID] += check;
        }

        protected void Switch(string StateID, Func<string> check)
        {
            if (!_states.ContainsKey(StateID))
                _switches[StateID] = check;
            else _switches[StateID] += check;
        }
    }
}
