using System;
using System.IO;
using System.Collections.Generic;
using Util;
using UnityEngine;
using System.Reflection;
using Newtonsoft.Json;

namespace Core
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class SaveableAttribute : Attribute {}

    public abstract class Model
    {
        internal ModelSvc _service;
        public void Broadcast() => _service?.Broadcast(this);
        public void Observe<T>(Action<T> action) where T : Model => _service?.Observe<T>((T)this, action);
        public void Unobserve<T>(Action<T> action) where T : Model => _service?.Unobserve<T>((T)this, action);
    }

    public interface IResettable { void Reset(); }

    public class ModelSvc : IAppService, IDisposable
	{
        protected Dictionary<Type, Model> _models = new Dictionary<Type, Model>();
        protected Dictionary<Model, List<Delegate>> _observers = new Dictionary<Model, List<Delegate>>();

        // Note this method tracks models that haven't been properly registered.
        public void Observe<T>(T model, Action<T> action) where T:Model
        {
            if (!_observers.TryGetValue(model, out List<Delegate> observers))
            {
                model._service = this;
                _observers[model] = observers = new List<Delegate>();
            }
            observers.Add(action);
            action.Invoke(model); // Courtesy Initialize. You better be ready. ;)
        }

        public bool Unobserve<T>(T model, Action<T> action) where T:Model
        {
            if (_observers.TryGetValue(model, out List<Delegate> observers))
            {
                return observers.Remove(action);
            }
            return false;
        }

        public void Broadcast<T>(T model) where T:Model
        {
            if (_observers.TryGetValue(model, out List<Delegate> observers))
            {
                observers.ForEach(a => (a as Action<T>)?.Invoke(model));
            }
        }

        // Retrieves a model of a particular type.
        public T GetModel<T>() where T : Model => _models.TryGetValue(typeof(T), out Model m) ? (T)m : null;

		// Register a unique model to the T service.
		public T Register<T>() where T : Model, new()
		{
			Type t = typeof(T);
			if (!_models.TryGetValue(t, out Model m))
			{
				_models[t] = m = new T();
                (m as IInitializable)?.Initialize();
			}
			return (T)m;
		}

		// Unregister a model.
		public void Unregister<T>() where T : Model
		{
			Model m;
			Type t = typeof(T);
			if (_models.TryGetValue(t, out m))
			{
				(m as IDisposable)?.Dispose();
				_models.Remove(t);
			}
		}

		public void Dispose()
		{
            foreach (KeyValuePair<Model, List<Delegate>> kvp in _observers)
            {
                kvp.Value.Clear();
            }
            foreach (KeyValuePair<Type, Model> kvp in _models)
            {
                (kvp.Value as IDisposable)?.Dispose();
            }
            _models.Clear();
            _observers.Clear();
			_models = null;
		}

        public void Reset()
        {
            foreach (Model model in _models.Values)
            {
                (model as IResettable)?.Reset();
            }
        }

        public string Save()
        {
            Dictionary<string, string> json = new Dictionary<string, string>();
            MemberInfo info;
            SaveableAttribute attribute; // TODO: kill the "saveable" attribute, just idenfity the JSON tags
            foreach (Model model in _models.Values)
            {
                info = model.GetType();
                attribute = info.GetCustomAttribute<SaveableAttribute>();
                if (attribute != null)
                {
                    try
                    { 
                        json.Add(model.GetType().ToString(), JsonConvert.SerializeObject(model));
                    }
                    catch(Exception e)
                    {
                        Debug.LogException(new Exception("Error serializing " + model.GetType().ToString() + ": " + e.Message));
                    }
                }
            }
            return JsonConvert.SerializeObject(json);
        }

        public bool Restore(string json)
        {
            Dictionary<string, string> data = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            foreach (Model model in _models.Values)
            {
                (model as IResettable)?.Reset();
                if (data.TryGetValue(model.GetType().ToString(), out string str))
                {
                    JsonConvert.PopulateObject(str, model);
                }
            }
            return true;
        }
    }
}
