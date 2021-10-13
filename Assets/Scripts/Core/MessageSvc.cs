using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core
{
	public interface IMessageEvent { }
	public class TypeEvent<T> : IMessageEvent
	{
		protected Action<T> _typeMessageHandler;
        protected readonly object _messageLock = new object();
        protected List<Action<T>> _queue = null;
        protected event Action<T> MessageHandler
		{
			add
			{
                if (!_typeMessageHandler?.GetInvocationList().Contains(value) ?? true)
				{
                    lock(_messageLock) { _typeMessageHandler += value; }
				}
			}
			remove
			{
                if (_typeMessageHandler != null)
                {
                    lock(_messageLock) { _typeMessageHandler -= value; }
                }
			}
		}

		public void Add(Action<T> action)
		{
            if (_queue == null) MessageHandler += action;
            else _queue.Add(action);
		}

		public void Remove(Action<T> action)
		{
			MessageHandler -= action;
		}

		public void Send(T data)
		{
            if (_queue == null)
            {
                _queue = new List<Action<T>>();
                _typeMessageHandler?.Invoke(data);
                _queue.ForEach(a => MessageHandler += a);
                _queue.Clear();
                _queue = null;
            }
        }
	}

	public class MessageEvent : IMessageEvent
	{
        protected Action _messageHander;
        protected readonly object _messageLock = new object();
        protected List<Action> _queue = null;
        protected event Action EventHandler
        {
            add
            {
                if (!_messageHander?.GetInvocationList().Contains(value) ?? true)
                {
                    lock (_messageLock) { _messageHander += value; }
                }
            }
            remove
            {
                if (_messageHander != null)
                {
                    lock (_messageLock) { _messageHander -= value; }
                }
            }

        }

        public void Add(Action action)
		{
            if (_queue == null) EventHandler += action;
            else _queue.Add(action);
		}

		public void Remove(Action action)
		{
            EventHandler -= action;
		}

		public void Send()
		{
            if (_queue == null)
            {
                _queue = new List<Action>();
                _messageHander?.Invoke();
                _queue.ForEach(a => EventHandler += a);
                _queue.Clear();
                _queue = null;
            }
		}
	}

	public class MessageSvc : IAppService, IDisposable
	{
		protected Dictionary<Type, IMessageEvent> _typeHandlers;
		protected Dictionary<string, Dictionary<Type, IMessageEvent>> _messageTypeHandlers;
		protected Dictionary<string, MessageEvent> _messageHandlers;

        public MessageSvc ()
		{
			_typeHandlers = new Dictionary<Type, IMessageEvent>();
			_messageHandlers = new Dictionary<string, MessageEvent>();
			_messageTypeHandlers = new Dictionary<string, Dictionary<Type, IMessageEvent>>();
		}

		public void Subscribe(string messageID, Action callback)
		{
			if (messageID == null) return;
            _messageHandlers.TryGetValue(messageID, out MessageEvent e);
            if (e == null) _messageHandlers[messageID] = e = new MessageEvent();
            e.Add(callback);
        }

		public void Subscribe<T>(string messageID, Action<T> callback)
		{
			if (messageID == null) return;
            Type t = typeof(T);
            _messageTypeHandlers.TryGetValue(messageID, out Dictionary<Type, IMessageEvent> handlers);
            if (handlers == null) _messageTypeHandlers[messageID] = handlers = new Dictionary<Type, IMessageEvent>();
            handlers.TryGetValue(t, out IMessageEvent e);
            if (!(e is TypeEvent<T>)) handlers[t] = new TypeEvent<T>();
            (handlers[t] as TypeEvent<T>).Add(callback);
		}

		public void Subscribe<T>(Action<T> callback)
		{
            Type t = typeof(T);
            _typeHandlers.TryGetValue(t, out IMessageEvent e);
            if (!(e is TypeEvent<T>)) _typeHandlers[t] = e = new TypeEvent<T>();
            (e as TypeEvent<T>).Add(callback);
		}

        public void Unsubscribe(string messageID, Action callback)
        {
            _messageHandlers.TryGetValue(messageID, out MessageEvent e);
            e?.Remove(callback);
        }

		public void Unsubscribe<T>(Action<T> callback)
		{
			Type t = typeof(T);
            _typeHandlers.TryGetValue(t, out IMessageEvent e);
            (e as TypeEvent<T>)?.Remove(callback);
		}

		public void Unsubscribe<T>(string messageID, Action<T> callback)
		{
			if (messageID == null) return;
			Type t = typeof(T);
            IMessageEvent e = null;
            _messageTypeHandlers.TryGetValue(messageID, out Dictionary<Type, IMessageEvent> dic);
            dic?.TryGetValue(t, out e);
            (e as TypeEvent<T>)?.Remove(callback);
		}

		public void Send(string messageID)
		{
			Debug.LogFormat( "MessageSvc.Send {0}", messageID);
            if (messageID != null && _messageHandlers.TryGetValue(messageID, out MessageEvent e))
            {
                e?.Send();
            }
        }

        public void Send<T>(string messageID, T messageData)
		{
            Debug.LogFormat( "MessageSvc.Send {0} <{1}: {2}>", messageID, typeof(T).ToString(), messageData?.ToString() );

			IMessageEvent e;
			Dictionary<Type, IMessageEvent> d;
			Type t = typeof(T);
			if (messageID != null && _messageTypeHandlers.TryGetValue(messageID, out d) && d.TryGetValue(t, out e))
				(e as TypeEvent<T>).Send(messageData);
        }

        public void Send<T>(T messageData)
		{
			Debug.LogFormat( "MessageSvc.Send <{0}: {1}>", typeof(T).ToString(), messageData.ToString() );

			IMessageEvent e;
			Type t = typeof(T);
			if (_typeHandlers.TryGetValue(t, out e))
			{
				(e as TypeEvent<T>).Send(messageData);
			}
		}

		public void Dispose()
		{
			_typeHandlers.Clear();
			_typeHandlers = null;
			_messageHandlers.Clear();
			_messageHandlers = null;
			_messageTypeHandlers.Clear();
			_messageTypeHandlers = null;
		}
    }
}