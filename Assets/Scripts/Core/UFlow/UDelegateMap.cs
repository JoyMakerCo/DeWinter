using System;
using System.Collections.Generic;
namespace UFlow
{
    public abstract class UDelegateMap
    {
        internal string _machine;
        internal Dictionary<string, Func<bool>> _decisions = new Dictionary<string, Func<bool>>();
        internal Dictionary<string, Delegate> _states = new Dictionary<string, Delegate>();
        protected UDelegateMap(string UMachineID) => _machine = UMachineID;
        
        public Delegate State(string StateID)
        {
            if (!_states.TryGetValue(StateID, out Delegate result))
            { } // instantiate here... however one does that

            return _states[StateID];
        }
    }
}
