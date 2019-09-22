using System;
namespace UFlow
{
    public class UInputState : UState
    {
        public sealed override void OnEnterState(string [] args) => InitListeners(args);
        public virtual void InitListeners(string[] args) { }
        // Input states must reference this function in their listeners
        protected void Activate() => _Machine.Activate(this);
    }
}
