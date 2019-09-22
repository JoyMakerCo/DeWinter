using System;
namespace UFlow
{
    public class UNode
    {
        public string ID { get; internal set; }
        internal UFlowSvc _UFlow;
        internal UMachine _Machine;
        public virtual void OnEnterState(string[] args) => OnEnterState();
        public virtual void OnEnterState() { }
        public virtual void Cleanup() {}
    }

    public abstract class UNode<T> : UNode
    {
        public abstract void SetData(T node);
    }
}
