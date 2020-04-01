using System;
namespace UFlow
{
    public class UInputState : UState
    {
        // Input states must reference this function in their listeners
        protected void Activate() => _Machine.Activate(this);
    }
}
