using System;
namespace UFlow
{
    public class UInputState : UState
    {
        public void Activate()
        {
            if (_Machine != null)
            {
                _Machine.ActivateInput(this);
            }
            else
            {
                Dispose();
            }
        }
    }
}
