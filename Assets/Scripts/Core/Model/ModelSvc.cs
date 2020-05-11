using System;
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
        protected const string PATH = "SaveState";


        protected Dictionary<Type, Model> _models = new Dictionary<Type, Model>();
        protected Dictionary<Model, List<Delegate>> _observers = new Dictionary<Model, List<Delegate>>();

        protected SaveStateVO LoadSaveStateVO()
        {
            return Resources.Load<SaveStateVO>(PATH)

#if UNITY_EDITOR
                ?? Util.ScriptableObjectUtil.CreateScriptableObject<SaveStateVO>(PATH);
#endif
            ;
        }

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

        public DateTime Save(string saveID)
        {
            Dictionary<string, string> json = new Dictionary<string, string>();
            string str;
            DateTime date = DateTime.Now;
            MemberInfo info;
            SaveableAttribute attribute;
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
            str = JsonConvert.SerializeObject(json);
            SaveStateVO.SaveRecordVO record = new SaveStateVO.SaveRecordVO(saveID, date, str);
            LoadSaveStateVO().Records.Add(record);
            Resources.UnloadUnusedAssets();
            return date;
        }

        public bool Restore(string saveID)
        {
            SaveStateVO.SaveRecordVO vo = LoadSaveStateVO().Records.Find(s=>s.SaveID == saveID);
            if (string.IsNullOrWhiteSpace(vo.SaveData)) return false;

            string str;
            MemberInfo info;
            SaveableAttribute attribute;
            Dictionary<string, string> state = JsonConvert.DeserializeObject<Dictionary<string, string>>(vo.SaveData);
            List<Model> restored = new List<Model>();
            foreach (Model model in _models.Values)
            {
                info = model.GetType();
                attribute = info.GetCustomAttribute<SaveableAttribute>();
                if (attribute != null && state.TryGetValue(model.GetType().ToString(), out str))
                {
                    var restore = JsonConvert.DeserializeObject(str, info.ReflectedType);
                }
            }
            Resources.UnloadUnusedAssets();
            return true;
        }
    }
}
