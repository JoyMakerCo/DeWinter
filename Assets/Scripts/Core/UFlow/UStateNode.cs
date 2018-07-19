using System;
using UnityEngine;

namespace UFlow
{
    public abstract class UComponent
    {

    }
    // StateMap with instructions for initializing a simple UFlow State
    public class UStateNode
    {
        public string ID;
    }

    public class UStateNode<S> : UStateNode where S:UState, new() {}
    public class UStateNode<S,D> : UStateNode<S> where S:UState<D>, new()
    {
        public D Data;
    }

    // LinkMap with instructions for initializing a simple UFlow Link
    public class UGraphLink {}
    public class UGraphLink<L> : UGraphLink where L:ULink, new() {}
    public class UGraphLink<L,D> : UGraphLink where L:ULink<D>, new()
    {
        public D Data;
    }
}
