using System;
namespace Ambition
{
    public class DebugState : UFlow.UState, Util.IInitializable<string>
    {
        public void Initialize(string debug) => UnityEngine.Debug.Log(" DEBUG STATE: " + debug);
    }
}
