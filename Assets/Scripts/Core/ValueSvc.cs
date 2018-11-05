using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public interface IModifier<T>
    {
        string ID { get; }
        T Value { set; get; }
        T Apply(CompositeValue<T> composite);
    }

    public interface ICompositeValue
    {
        string ID { set; get; }
    }

    public class CompositeValue<T>
    {
        private Action<T> _publishHandler;
        private event Action<T> _publisher;
        private List<IModifier<T>> _modifiers = new List<IModifier<T>>();
        protected T _baseValue;

        public T Set(T value)
        {
            _baseValue = value;
            T result = Get();
            //_publish(result);
            return result;
        }
    
        public T Get()
        {
            _modifiers.ForEach(t => t.Apply(this));
            return _baseValue; // TODO: Nope. Figure this out
        }

        public static implicit operator T(CompositeValue<T> t)
        {
            return t.Get();
        }

        public T Modify(IModifier<T> mod)
        {
            _modifiers.Add(mod);
            return Get();
        }

        public T Unmodify(string id)
        {
            _modifiers.RemoveAll(m => m.ID == id);
            return Get();
        }

        public T Unmodify(IModifier<T> mod)
        {
            _modifiers.Remove(mod);
            return Get();
        }

        public T Subscribe(Action<T> action)
        {
            T result = Get();
            //_publish += action;
            action(result);
            return result;
        }

        public T Unsubscribe(Action<T> action)
        {
            //_publish -= action;
            return Get();
        }
    }

    public class ValueSvc : IAppService
    {
        private Dictionary<string, ICompositeValue> _valueHandlers = new Dictionary<string, ICompositeValue>();
    }
}