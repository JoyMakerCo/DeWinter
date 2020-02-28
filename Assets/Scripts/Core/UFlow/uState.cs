using System;
namespace UFlow
{
	public class UState : IDisposable
	{
        public string ID { get; internal set; }
        internal UFlowSvc _UFlow;
        internal UMachine _Machine;

    // Overload for instructions
        public virtual void Initialize(object[] parameters) {}
        public virtual void OnEnterState() {}
        public virtual void OnExitState() {}
        public virtual void Dispose() {}
    }
}
