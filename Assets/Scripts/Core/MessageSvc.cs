using System;
using System.Collections.Generic;

namespace Core
{
	public class MessageSvc : IAppService, IDisposable
	{
		protected Dictionary<string, List<Delegate>> _listeners;
		protected Dictionary<Type, List<Delegate>> _typeListeners;
		protected Dictionary<string, List<Delegate>> _markedForRemoval;
		protected bool _isNotifying;

		public MessageSvc ()
		{
			_listeners = new Dictionary<string, List<Delegate>>();
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
			Type type = typeof(T);
			if (!_typeListeners.TryGetValue(type, out result))
			{
				result = new List<Delegate>();
				_typeListeners[type] = result;
			}
			if (!result.Contains(callback))
			{
				result.Add(callback);
				_typeListeners[type] = result;
			}		
		}

		public void Unsubscribe(string messageID, Delegate callback)
		{
			if (!_isNotifying)
			{
				RemoveDelegate(messageID, callback, _listeners);
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
			if (_typeListeners.TryGetValue(typeof(T), out callbacks))
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
			}
			if (!result.Contains(d))
			{
				result.Add(d);
				dict[type] = result;
			}
		}

		protected void RemoveDelegate(string type, Delegate d, Dictionary<string, List<Delegate>> dict)
		{
			List<Delegate> result;
			if (dict.TryGetValue(type, out result))
			{
				result.Remove(d);
				dict[type] = result;
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