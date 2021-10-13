using System;
using System.Collections.Generic;
using UnityEngine;

namespace UFlow
{
    // StateMap with instructions for initializing a simple UFlow State
    public class UStateNode
    {
        public string ID;
        internal virtual UState Instantiate(UMachine flow) => new UState() { ID = this.ID, _Flow = flow };
    }

    public class UStateNode<S> : UStateNode where S:UState, new()
    {
        internal override UState Instantiate(UMachine flow)
        {
            S s = new S() { ID = this.ID, _Flow = flow };
            (s as Util.IInitializable)?.Initialize();
            return s;
        }
    }

    public class UStateNode<S,D> : UStateNode<S> where S:UState, Util.IInitializable<D>, new()
    {
        public D Data;
        internal override UState Instantiate(UMachine flow)
        {
            S s = new S() { ID = this.ID, _Flow = flow };
            (s as Util.IInitializable<D>)?.Initialize(Data);
            return s;
        }
    }

    public class UFlowNode : UStateNode<UMachine, string>
    {
        internal override UState Instantiate(UMachine flow)
        {
            UMachine result = flow._UFlow.Instantiate(Data, flow);
            result.ID = this.ID;
            return result;
        }
    }
}
