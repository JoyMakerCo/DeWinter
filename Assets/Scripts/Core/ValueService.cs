using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class ValueService : IAppService, IDisposable
    {
        protected interface ITrackableValue : IDisposable
        {
            void Track(Delegate d);
            void Untrack(Delegate d);
        }

        protected class TrackableValue<T> : ITrackableValue
        {
            public virtual T Value { get; set; }

            protected event Action<T> Dispatcher;

            public void Track(Delegate d)
            {
                if (d is Action<T>)
                    Dispatcher += (d as Action<T>);
            }

            public void Untrack(Delegate d)
            {
                if (d is Action<T>)
                    Dispatcher -= (d as Action<T>);
            }

            public virtual void Dispose()
            {
                Delegate[] delegates = Dispatcher.GetInvocationList();
                Array.ForEach(delegates, d => Dispatcher -= (Action<T>)d);
            }

            public void Broadcast()
            {
                Dispatcher.Invoke(Value);
            }
        }

        protected abstract class TrackableNumberValue<T> : TrackableValue<T>
        {
            T _baseValue;
            T _value;
            Dictionary<string, T> _modifiers = new Dictionary<string, T>();
            Dictionary<string, T> _multipliers = new Dictionary<string, T>();

            public override T Value
            {
                get { return _value; }
                set { Recalculate(value); }
            }

            public T AddModifier(string modifierID, T value)
            {
                _modifiers[modifierID] = value;
                return Recalculate(value);
            }

            public T AddMultiplier(string multiplierID, T value)
            {
                _modifiers[multiplierID] = value;
                return Recalculate(value);
            }

            protected abstract T Mult(IEnumerable<T> values);
            protected abstract T Add(IEnumerable<T> values);

            protected T Recalculate(T value)
            {
                _value = _baseValue = value;
                _value = Mult(_multipliers.Values.Append(_value));
                _value = Add(_modifiers.Values.Append(_value));
                Broadcast();
                return _value;
            }

            public override void Dispose()
            {
                _multipliers = _modifiers = null;
                base.Dispose();
            }
        }

        protected class TrackableInt : TrackableNumberValue<int>
        {
            protected override int Mult(IEnumerable<int> values)
            {
                return values.Aggregate(1, (x, y) => x * y);
            }

            protected override int Add(IEnumerable<int> values)
            {
                return values.Sum();
            }
        }

        protected class TrackableFloat : TrackableNumberValue<float>
        {
            protected override float Mult(IEnumerable<float> values)
            {
                return values.Aggregate(1f, (x, y) => x * y);
            }

            protected override float Add(IEnumerable<float> values)
            {
                return values.Sum();
            }
        }

        protected class TrackableDouble : TrackableNumberValue<double>
        {
            protected override double Mult(IEnumerable<double> values)
            {
                return values.Aggregate(1d, (x, y) => x * y);
            }

            protected override double Add(IEnumerable<double> values)
            {
                return values.Sum();
            }
        }

        protected class TrackableLong : TrackableNumberValue<long>
        {
            protected override long Mult(IEnumerable<long> values)
            {
                return values.Aggregate(1L, (x, y) => x * y);
            }

            protected override long Add(IEnumerable<long> values)
            {
                return values.Sum();
            }
        }

        private Dictionary<string, ITrackableValue> _values = new Dictionary<string, ITrackableValue>();

        public T GetValue<T>(string valueID)
        {
            ITrackableValue value;
            return (_values.TryGetValue(valueID, out value) && (value is TrackableValue<T>))
                ? ((TrackableValue<T>)value).Value : default(T);
        }

        public T SetValue<T>(string valueID, T value)
        {
            ITrackableValue v;
            if (!_values.TryGetValue(valueID, out v) || !(v is TrackableValue<T>))
            {
                if (value is int || value is float || value is long || value is double || value is decimal)
                {
                    //_values[valueID] = new TrackableNumberValue<T>();
                }
            }
            return ((TrackableValue<T>)v).Value;
        }

        public double Track(string valueID, Action<double> action)
        {
            /*            NumberValue v;
                        if (!_values.TryGetValue(valueID, out v))
                        {
                            this[valueID] = 0;
                            v = _values[valueID];
                        }
                        v.Dispatcher += action;
                        return this[valueID];
            */
            return 0d;
        }

        public void Untrack(string valueID, Action<double> action)
        {
            //NumberValue v;
            //if (_values.TryGetValue(valueID, out v))
            //{
            //    v.Dispatcher -= action;
            //}
        }

        public void Dispose()
        {
            foreach (IDisposable v in _values.Values)
                v.Dispose();
            _values = null;
        }
    }
}
