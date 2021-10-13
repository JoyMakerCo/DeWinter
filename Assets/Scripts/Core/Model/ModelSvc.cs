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
    }

    public abstract class ObservableModel<M> : Model, IDisposable where M:class
    {
        private List<Action<M>> _observers = new List<Action<M>>();
        public void Broadcast() => _observers.ForEach(o=>o.Invoke(this as M));
        public void Observe(Action<M> action)
        {
            _observers.Add(action);
            action.Invoke(this as M);
        }
        public void Unobserve(Action<M> action) => _observers.Remove(action);
        public virtual void Dispose() => _observers.Clear();
    }

    public interface IResettable { void Reset(); }

    public class ModelSvc : IAppService, IDisposable
    {
        protected Dictionary<Type, Model> _models = new Dictionary<Type, Model>();
        protected Dictionary<Type, JsonConverter> _converters = new Dictionary<Type, JsonConverter>();

        // Retrieves a model of a particular type.
        public T GetModel<T>() where T : Model => _models.TryGetValue(typeof(T), out Model m) ? (T)m : null;

        // Register a unique model to the T service.
        public T Register<T>() where T : Model, new()
        {
            Type t = typeof(T);
            if (!_models.TryGetValue(t, out Model m))
            {
                _models[t] = m = new T();
                m._service = this;
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
            foreach (KeyValuePair<Type, Model> kvp in _models)
            {
                (kvp.Value as IDisposable)?.Dispose();
            }
            _models.Clear();
            _models = null;
        }

        public void Reset()
        {
            foreach (Model model in _models.Values)
            {
                (model as IResettable)?.Reset();
            }
        }

        public void RegisterConverter<TClass, TAbstract>() where TClass : TAbstract
        {
            Type t = typeof(AbstractConverter<TClass, TAbstract>);
            if (!_converters.ContainsKey(t))
            {
                _converters.Add(t, new AbstractConverter<TClass, TAbstract>());
            }
        }


        public string Save()
        {
            Dictionary<string, string> json = new Dictionary<string, string>();
            MemberInfo info;
            SaveableAttribute attribute; // TODO: kill the "saveable" attribute, just idenfity the JSON tags
            JsonSerializerSettings settings = new JsonSerializerSettings() { Converters = new List<JsonConverter>(_converters.Values) };
            foreach (Model model in _models.Values)
            {
                info = model.GetType();
                attribute = info.GetCustomAttribute<SaveableAttribute>();
                if (attribute != null)
                {
                    try
                    {
                        json.Add(model.GetType().ToString(), JsonConvert.SerializeObject(model,settings));
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(new Exception("Error serializing " + model.GetType().ToString() + ": " + e.Message));
                    }
                }
            }
            return JsonConvert.SerializeObject(json);
        }

        public bool Restore(string json)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings() { Converters = new List<JsonConverter>(_converters.Values) };
            Dictionary<string, string> data = JsonConvert.DeserializeObject<Dictionary<string, string>>(json, settings);
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
