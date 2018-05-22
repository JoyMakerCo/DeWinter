using System;

namespace UFlow
{
    // StateMap with instructions for initializing a simple UFlow State
    public class UStateMap
    {
       public string Name;
        public UStateMap() {}
        public UStateMap(string name) { Name = name; }
        public UStateMap(UStateMap map) { Name = map.Name; }
        internal virtual UState _instantiate()
        {
            return new UState(Name);
        }
    }

    // StateMap for UState derived classes
    public class UStateMap<T> : UStateMap where T:UState, new()
    {
        public UStateMap() {}
        public UStateMap(string name):base(name) {}
        public UStateMap(UStateMap map):base(map) {}
        public UStateMap(UStateMap<T> map):base(map) {}
        override internal UState _instantiate()
        {
            T state = new T();
            state.Name = Name;
            return state;
        }
    }

    // StateMap for derived classes with initialization data
    public class UStateMap<T,U> : UStateMap<T> where T:UState<U>, new()
    {
        public U Data;
        public UStateMap() : base() {}
        public UStateMap(string name) : base(name) {}
        public UStateMap(UStateMap map) : base(map) {}
        public UStateMap(UStateMap<T> map) : base(map) {}
        public UStateMap(UStateMap<T,U> map) : base(map) { Data = map.Data; }
        override internal UState _instantiate()
        {
            T state = new T();
            state.Name = Name;
            state.Data = Data;
            return state;
        }
    }

    // LinkMap with instructions for initializing a simple UFlow Link    
    public class ULinkMap : Util.DirectedGraphLink
    {
        public string Input;
        public ULinkMap() {}
        public ULinkMap(string input) : base() { Input = input; }
        public ULinkMap(ULinkMap map) : base((Util.DirectedGraphLink)map)
        {
            Input = map.Input;
        }

        internal virtual ULink _instantiate(UMachineState machine, UState state)
        {
            ULink l = new ULink();
            l.State = state;
            l.Input = Input;
            l._targetState = Target;
            l._machine = machine;
            l.Initialize();
            return l;
        }
    }

    // LinkMap with instructions for initializing a derived Link Class
    public class ULinkMap<T> : ULinkMap where T:ULink, new()
    {
        public ULinkMap() {}
        public ULinkMap(string input) : base(input) {}
        public ULinkMap(ULinkMap map) : base(map) {}
        override internal ULink _instantiate(UMachineState machine, UState state)
        {
            T l = new T();
            l.Input = Input;
            l.State = state;
            l._targetState = Target;
            l._machine = machine;
            l.Initialize();
            return l;
        }
    }

    public class ULinkMap<T,U> : ULinkMap where T:ULink<U>, new()
    {
        public U Data;
        public ULinkMap() {}
        public ULinkMap(string input) : base(input) {}
        public ULinkMap(ULinkMap map) : base(map) {}
        public ULinkMap(ULinkMap<T,U> map) : base(map)
        {
            Data = map.Data;
        }

        override internal ULink _instantiate(UMachineState machine, UState state)
        {
            T l = new T();
            l.Input = Input;
            l.State = state;
            l._targetState = Target;
            l._machine = machine;
            l.Data = Data;
            l.Initialize();
            return l;
        }
    }
}
