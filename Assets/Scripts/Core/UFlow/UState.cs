using System;
namespace UFlow
{
	public class UState : UNode
	{
        // Overload for instructions
        public virtual void OnExitState() {}
	}

    public abstract class UState<T> : UState
    {
        public abstract void SetData(T data);
    }
}
