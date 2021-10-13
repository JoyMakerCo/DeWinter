namespace UFlow
{
    public class UInput : UState
    {
        public void Activate()
        {
            if (_Flow != null) _Flow.Activate(this);
            else (this as System.IDisposable)?.Dispose();
        }
    }
}
