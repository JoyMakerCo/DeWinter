using System;
using UFlow;
using UnityEngine;
using UnityEngine.Events;

namespace UFlow
{
    [Serializable]
    public class UControllerDelegate : UnityEvent {}
    [Serializable]
    public class UControllerDelegate<T> : UnityEvent<T> {}

    // TODO: Register arbitrary states with delegates, and nix this class completely
    public class UControllerState : UState
    {
        override public void OnEnterState() => Invoke();
        protected void Invoke() => _Machine?._UFlow?.GetController(_Machine)?.Invoke(ID);
    }

    public abstract class UControllerState<T> : UState
    {
        internal UControllerDelegate<T> _event = new UControllerDelegate<T>();
        protected void Invoke() => _Machine?._UFlow?.GetController(_Machine)?.Invoke(ID);
        override public void OnEnterState() => Invoke();
    }
}
