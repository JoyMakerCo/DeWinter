using System;
namespace UFlow
{
    public class UDecision : UState, Util.IInitializable<Func<bool>>
    {
        public Func<bool> Validate { get; private set; } = null;
        public void Initialize(Func<bool> delgate) => Validate = delgate;
    }
}
