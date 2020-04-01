using System;
using System.Collections.Generic;
using UnityEngine;

namespace UFlow
{
    // StateMap with instructions for initializing a simple UFlow State
    public class UStateNode
    {
        public string ID;
        public string[] Tags;
    }

    public class UStateNode<S> : UStateNode where S:UNode, new() {}
    public class UStateNode<S,D> : UStateNode<S> where S:UState<D>, new()
    {
        public D Data;
    }

    public class UMachineStateNode : UStateNode
    {
        public string MachineID { get; private set; }
        public UMachineStateNode(string machineID) => MachineID = machineID;
    }

    // LinkMap with instructions for initializing a simple UFlow Link
    public class UGraphLink {}
    public class UGraphLink<L> : UGraphLink where L:ULink, new() {}
    public class UGraphLink<L,D> : UGraphLink where L:ULink<D>, new()
    {
        public D Data;
    }

    public class UDefaultLink : ULink
    {
        public override bool Validate() => true;
    }

    public class UDelegateNode : UStateNode
    {
        public Delegate OnEnterState;
    }

    public class UDecisionNode : UStateNode
    {
        public Func<bool> Check;
    }
}
