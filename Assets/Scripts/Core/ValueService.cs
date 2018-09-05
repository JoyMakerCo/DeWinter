using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class ValueService : IAppService
    {
        public interface ITrackableValue : IDisposable
        {
            void Track(Delegate d);
            void Untrack(Delegate d);
        }

        public class TrackableValue<T>
        {
            public T Value;
           
            protected event Action<T> _dispatcher;

            public static implicit operator TrackableValue<T>(T value)
            {
                TrackableValue<T> t = new TrackableValue<T>();
                t.Value = value;
                return t;
            }

            public static implicit operator T(TrackableValue<T> value)
            {
                return value.Value;
            }

            public void Track(Delegate d)
            {
                if (d is Action<T>)
                    _dispatcher += d as Action<T>;
            }

            public void Untrack(Delegate d)
            {
                if (d is Action<T>)
                    _dispatcher -= (d as Action<T>);
            }

            public virtual void Dispose()
            {
                Delegate[] delegates = _dispatcher.GetInvocationList();
                Array.ForEach(delegates, d => _dispatcher -= (Action<T>)d);
            }

            public void Broadcast()
            {
                _dispatcher.Invoke(Value);
            }
        }

        public class ObservableNumber<T> : TrackableValue<T>
        {
            // TODO: Modifiers and multipliers
        }
    }
}
