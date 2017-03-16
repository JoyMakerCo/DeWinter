using System;
using System.Collections.Generic;

namespace Core
{
// TODO: Implement C# Events instead of reinventing the wheel
	public class MessageSvc : IAppService, IDisposable
	{
		protected Dictionary<string, List<Delegate>> _listeners;
		protected Dictionary<Type, List<Delegate>> _typeListeners;
		protected List<KeyValuePair<string, Delegate>> _markedForRemoval;
		protected List<KeyValuePair<Type, Delegate>> _typeMarkedForRemoval;
		protected List<KeyValuePair<string, Delegate>> _markedForSubscription;
		protected List<KeyValuePair<Type, Delegate>> _typeMarkedForSubscription;
		protected bool _isNotifying;

		public MessageSvc ()
		{
			_listeners = new Dictionary<string, List<Delegate>>();
			_typeListeners = new Dictionary<Type, List<Delegate>>();
			_markedForRemoval = null;
			_typeMarkedForRemoval = null;
			_markedForSubscription = null;
			_typeMarkedForSubscription = null;
			_isNotifying = false;
		}

		public void Subscribe(string messageID, Action callback)
		{
			if (!_isNotifying)
			{
				if (!_listeners.ContainsKey(messageID))
				{
					_listeners.Add(messageID, new List<Delegate>());
				}
				if (!_listeners[messageID].Contains(callback))
				{
					_listeners[messageID].Add(callback);
				}
			}
			else if (!_listeners.ContainsKey(messageID) || !_listeners[messageID].Contains(callback))
			{
				if (_markedForSubscription == null)
				{
					_markedForSubscription = new List<KeyValuePair<string, Delegate>>();
				}
				_markedForSubscription.Add(new KeyValuePair<string, Delegate>(messageID, callback));
			}
		}

		public void Subscribe<T>(string messageID, Action<T> callback)
		{
			if (!_isNotifying)
			{
				if (!_listeners.ContainsKey(messageID))
				{
					_listeners.Add(messageID, new List<Delegate>());
				}
				if (!_listeners[messageID].Contains(callback))
				{
					_listeners[messageID].Add(callback);
				}
			}
			else if (!_listeners.ContainsKey(messageID) || !_listeners[messageID].Contains(callback))
			{
				if (_markedForSubscription == null)
				{
					_markedForSubscription = new List<KeyValuePair<string, Delegate>>();
				}
				_markedForSubscription.Add(new KeyValuePair<string, Delegate>(messageID, callback));
			}
		}

		public void Subscribe<T>(Action<T> callback)
		{
			Type t = typeof(T);
			if (!_isNotifying)
			{
				if (!_typeListeners.ContainsKey(t))
				{
					_typeListeners.Add(t, new List<Delegate>());
				}
				if (!_typeListeners[t].Contains(callback))
				{
					_typeListeners[t].Add(callback);
				}
			}
			else if (!_typeListeners.ContainsKey(t) || !_typeListeners[t].Contains(callback))
			{
				if (_typeMarkedForSubscription == null)
				{
					_typeMarkedForSubscription = new List<KeyValuePair<Type, Delegate>>();
				}
				_typeMarkedForSubscription.Add(new KeyValuePair<Type, Delegate>(t, callback));
			}
		}

		public void Unsubscribe(string messageID, Delegate callback)
		{
			if (!_isNotifying)
			{
				if (_listeners.ContainsKey(messageID))
				{
					_listeners[messageID].Remove(callback);
				}
			}
			else
			{
				if (_markedForRemoval == null)
				{
					_markedForRemoval = new List<KeyValuePair<string, Delegate>>();
				}
				_markedForRemoval.Add(new KeyValuePair<string, Delegate>(messageID, callback));
			}
		}

		public void Unsubscribe<T>(Action<T> callback)
		{
			Type t = typeof(T);
			if (!_isNotifying)
			{
				if (_typeListeners.ContainsKey(t))
				{
					_typeListeners[t].Remove(callback);
				}
			}
			else
			{
				if (_typeMarkedForRemoval == null)
				{
					_typeMarkedForRemoval = new List<KeyValuePair<Type, Delegate>>();
				}
				_typeMarkedForRemoval.Add(new KeyValuePair<Type, Delegate>(t, callback));
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
				ResolveMarkedDelegates();
			}
		}

		public void Send<T>(string messageID, T msg)
		{
			List<Delegate> callbacks;
			if (_listeners.TryGetValue(messageID, out callbacks))
			{
				_isNotifying = true;
				foreach(Delegate d in callbacks)
				{
					if (d is Action<T>)
						(d as Action<T>).Invoke(msg);
				}
				ResolveMarkedDelegates();
			}
		}

		public void Send<T>(T messageData)
		{
			List<Delegate> callbacks;
			Type t = typeof(T);
			if (_typeListeners.TryGetValue(t, out callbacks))
			{
				_isNotifying = true;
				foreach(Action<T> action in callbacks)
				{
					(action as Action<T>).Invoke(messageData);
				}
				ResolveMarkedDelegates();
			}
		}

		protected void ResolveMarkedDelegates()
		{
			_isNotifying = false;
			if (_markedForSubscription != null)
			{
				foreach(KeyValuePair<string, Delegate> kvp in _markedForSubscription)
				{
					if (!_listeners.ContainsKey(kvp.Key))
					{
						_listeners.Add(kvp.Key, new List<Delegate>());
					}
					if (!_listeners[kvp.Key].Contains(kvp.Value))
					{
						_listeners[kvp.Key].Add(kvp.Value);
					}
				}
				_markedForSubscription.Clear();
				_markedForSubscription = null;
			}
			if (_typeMarkedForSubscription != null)
			{
				foreach(KeyValuePair<Type, Delegate> kvp in _typeMarkedForSubscription)
				{
					if (!_typeListeners.ContainsKey(kvp.Key))
					{
						_typeListeners.Add(kvp.Key, new List<Delegate>());
					}
					if (!_typeListeners[kvp.Key].Contains(kvp.Value))
					{
						_typeListeners[kvp.Key].Add(kvp.Value);
					}
				}
				_typeMarkedForSubscription.Clear();
				_typeMarkedForSubscription = null;
			}
			if (_markedForRemoval != null)
			{
				foreach(KeyValuePair<string, Delegate> kvp in _markedForRemoval)
				{
					if (_listeners.ContainsKey(kvp.Key))
					{
						_listeners[kvp.Key].Remove(kvp.Value);
					}
				}
				_markedForRemoval.Clear();
				_markedForRemoval = null;
			}
			if (_typeMarkedForRemoval != null)
			{
				foreach(KeyValuePair<Type, Delegate> kvp in _typeMarkedForRemoval)
				{
					if (_typeListeners.ContainsKey(kvp.Key))
					{
						_typeListeners[kvp.Key].Remove(kvp.Value);
					}
				}
				_typeMarkedForRemoval.Clear();
				_typeMarkedForRemoval = null;
			}
		}

		public void Dispose()
		{
			ResolveMarkedDelegates();
			_listeners.Clear();
			_listeners = null;
			_typeListeners.Clear();
			_typeListeners = null;
		}
	}
}