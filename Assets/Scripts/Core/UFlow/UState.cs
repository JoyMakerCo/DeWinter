namespace UFlow
{
    public class UState
    {
        public string ID { get; internal set; }
        internal UMachine _Machine;

        public virtual void OnEnterState() { }
    }
}
