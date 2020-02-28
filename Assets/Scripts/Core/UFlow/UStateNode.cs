using System;
using System.Collections.Generic;
using UnityEngine;

namespace UFlow
{
    // StateMap with instructions for initializing a simple UFlow State
    public class UStateNode
    {
        public string ID;
        public object[] Parameters;
        public UStateNode(string id, object[] parameters)
        {
            ID = id;
            Parameters = parameters;
        }
        virtual internal UState _Instantiate() => new UState();
    }

    public class UStateNode<S> : UStateNode where S:UState, new()
    {
        public UStateNode(string id, object[] parameters) : base(id, parameters) { }
        internal override UState _Instantiate() => new S();
    }

    // LinkMap with instructions for initializing a simple UFlow Link
    public class UGraphLink
    {
        internal virtual ULink _Instantiate() => new UDefaultLink();
    }
    public class UGraphLink<L> : UGraphLink where L:ULink, new()
    {
        internal override ULink _Instantiate() => new L();
    }
    public class UGraphLink<L,D> : UGraphLink where L:ULink<D>, new()
    {
        public D Data;
        internal override ULink _Instantiate()
        {
            L result = new L();
            result.SetValue(Data);
            return result;
        }
    }

    public class UDefaultLink : ULink
    {
        public override bool Validate() => true;
    }
}
