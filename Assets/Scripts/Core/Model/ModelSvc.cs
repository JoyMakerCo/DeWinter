using System;
using System.Collections.Generic;
using Util;

namespace Core
{
	public interface IModel {};
	public class ModelSvc : IAppService, IDisposable
	{
		protected Dictionary<Type, IModel> _models;

		public ModelSvc()
		{
			_models = new Dictionary<Type, IModel>();
		}

		// Retrieves a model of a particular type.
		public T GetModel<T>() where T : IModel
		{
			IModel m;
			return _models.TryGetValue(typeof(T), out m) ? (T)m : default(T);
		}

		// Register a unique model to the T service.
		public T Register<T>() where T : IModel, new()
		{
			Type t = typeof(T);
			IModel m = default(T);
			if (!_models.TryGetValue(t, out m))
			{
				_models[t] = m = new T();
				if (m is IInitializable)
					(m as IInitializable).Initialize();
			}
			return (T)m;
		}

		// Unregister a model.
		public void Unregister<T>() where T : IModel
		{
			IModel m;
			Type t = typeof(T);
			if (_models.TryGetValue(t, out m))
			{
				Dispose(m as IDisposable);
				_models.Remove(t);
			}
		}

		// Use this for disposal
		public void Clear()
		{
			foreach (KeyValuePair<Type, IModel> kvp in _models)
			{
				Dispose(kvp.Value as IDisposable);
			}
			_models.Clear();
		}

		public void Dispose()
		{
			Clear();
			_models = null;
		}

		protected void Dispose(IDisposable d)
		{
			if (d != null)
				d.Dispose();
		}
	}
}