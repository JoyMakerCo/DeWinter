using System;
namespace UFlow
{
    [Serializable]
    public class UDecisionState : UNode
    {
        public sealed override void OnEnterState()
        {
            //TODO: Build links that validate to true
        }
        public virtual bool Validate(string[] args=null) => true;
    }
}
