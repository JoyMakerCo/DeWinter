namespace UFlow
{
    public class UState : Core.IState
    {
        public string ID { get; internal set; }
        internal UMachine _Flow;

        public virtual void OnEnter() { }
    }
}
