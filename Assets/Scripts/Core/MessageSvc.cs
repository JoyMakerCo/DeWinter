using System;
using System.Collections.Generic;

namespace Core
{
	public class MessageSvc : IAppService, IDisposable
	{
		protected Dictionary<string, List<Delegate>> _listeners;
		protected Dictionary<Type, List<Delegate>> _typeListeners;
		protected Dictionary<string, List<Delegate>> _markedForRemoval;
		protected Dictionary<Type, List<Delegate>> _typeMarkedForRemoval;
		protected bool _isNotifying;

		public MessageSvc ()
		{
			_listeners = new Dictionary<string, List<Delegate>>();
			_typeListeners = new Dictionary<Type, List<Delegate>>();
			_markedForRemoval = null;
			_isNotifying = false;
		}

		public void Subscribe(string messageID, Action callback)
		{
			AddDelegate(messageID, callback, _listeners);
		}

		public void Subscribe<T>(string messageID, Action<T> callback)
		{
			AddDelegate(messageID, callback, _listeners);
		}

		public void Subscribe<T>(Action<T> callback)
		{
			List<Delegate> result;
			Type t = typeof(T);
			if (!_typeListeners.TryGetValue(t, out result))
			{
				result = new List<Delegate>();
			}
			if (!result.Contains(callback))
			{
				result.Add(callback);
				_typeListeners[t] = result;
			}
		}

		public void Unsubscribe(string messageID, Delegate callback)
		{
			if (!_isNotifying)
			{
				RemoveDelegate(messageID, callback, _listeners);
				RemoveDelegate(messageID, callback, _markedForRemoval);
			}
			else
			{
				if (_markedForRemoval == null)
				{
					_markedForRemoval = new Dictionary<string, List<Delegate>>();
				}
				AddDelegate(messageID, callback, _markedForRemoval);
			}
		}

		public void Unsubscribe<T>(Delegate callback)
		{
			List<Delegate> delegates;
			Type t = typeof(T);
			if (_typeListeners.TryGetValue(t, out delegates) && delegates.Contains(callback))
			{
				if (!_isNotifying)
				{
					RemoveDelegate<T>(callback, _typeListeners);
					RemoveDelegate<T>(callback, _typeMarkedForRemoval);
				}
				else
				{
					if (_typeMarkedForRemoval == null)
					{
						_typeMarkedForRemoval = new Dictionary<Type, List<Delegate>>();
					}
					if (!_typeMarkedForRemoval.TryGetValue(t, out delegates))
					{
						_typeMarkedForRemoval[t] = new List<Delegate>();
					}
					_typeMarkedForRemoval[t].Add(callback);
				}
			}
		}

		public void Send(string messageID)
		{
			List<Delegate> callbacks;
			if (_listeners.TryGetValue(messageID, out callbacks))
			{
				_isNotifying = true;
				foreach(Delegate action in callbacks)
				{
					if (action is Action)
					{
						(action as Action).Invoke();
					}
				}
				RemoveMarkedDelegates();
			}
		}

		public void Send<T>(string messageID, T messageData)
		{
			List<Delegate> callbacks;
			if (_listeners.TryGetValue(messageID, out callbacks))
			{
				_isNotifying = true;
				foreach(Delegate action in callbacks)
				{
					if (action is Action<T>)
					{
						(action as Action<T>).Invoke(messageData);
					}
					else if (action is Action)
					{
						(action as Action).Invoke();
					}
				}
				RemoveMarkedDelegates();
			}
		}

		public void Send<T>(T messageData)
		{
			List<Delegate> callbacks;
			Type t = typeof(T);
			if (_typeListeners.TryGetValue(t, out callbacks))
			{
				_isNotifying = true;
				foreach(Delegate action in callbacks)
				{
					(action as Action<T>).Invoke(messageData);
				}
				RemoveMarkedDelegates();
			}
		}

		protected void AddDelegate(string type, Delegate d, Dictionary<string, List<Delegate>> dict)
		{
			List<Delegate> result;
			if (!dict.TryGetValue(type, out result))
			{
				result = new List<Delegate>();
				dict.Add(type, result);
			}
			if (!result.Contains(d))
			{
				dict[type].Add(d);
			}
		}

		protected void RemoveDelegate(string type, Delegate d, Dictionary<string, List<Delegate>> dict)
		{
			if (dict.ContainsKey(type))
			{
				dict[type].Remove(d);
			}
		}

		protected void RemoveDelegate<T>(Delegate d, Dictionary<Type, List<Delegate>> dict)
		{
			Type t = typeof(T);
			if (dict.ContainsKey(t))
			{
				dict[t].Remove(d);
			}
		}

		protected void RemoveMarkedDelegates()
		{
			_isNotifying = false;

			if (_markedForRemoval != null)
			{
				foreach(KeyValuePair<string, List<Delegate>> kvp in _markedForRemoval)
				{
					foreach(Delegate d in kvp.Value)
					{
						RemoveDelegate(kvp.Key, d, _listeners);
					}
				}
				_markedForRemoval.Clear();
				_markedForRemoval = null;
			}
		}

		public void Dispose()
		{
			_markedForRemoval.Clear();
			_markedForRemoval = null;
			_listeners.Clear();
			_listeners = null;
		}
	}
}