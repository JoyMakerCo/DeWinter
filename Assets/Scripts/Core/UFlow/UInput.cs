namespace UFlow
{
    public class UInput : UState
    {
        public virtual void OnActivate() { }
        public void Activate()
        {
            OnActivate();
            if (_Machine != null) _Machine.Activate(this);
            else (this as System.IDisposable)?.Dispose();
        }
    }
}
