using System;
namespace Core
{
    public class StateMachine : IDisposable
    {
        protected IState _state = null;
        public IState state
        {
            get => _state;
            set
            {
                (_state as IExitState)?.OnExit();
                (_state as IDisposable)?.Dispose();
                _state = value;
                _state?.OnEnter();
            }
        }
        public void Dispose() => (_state as IDisposable)?.Dispose();
    }

    public interface IState { void OnEnter(); }
    public interface IExitState { void OnExit(); }
}
