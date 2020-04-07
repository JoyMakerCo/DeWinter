using System;
namespace UFlow
{
    public class UNode : IDisposable
    {
        public string ID { get; internal set; }
        internal UFlowSvc _UFlow;
        internal UMachine _Machine;
        public virtual void OnEnterState() { }
        public virtual void Dispose() {}
    }

    public abstract class UNode<T> : UNode
    {
        public abstract void SetData(T node);
    }
}
