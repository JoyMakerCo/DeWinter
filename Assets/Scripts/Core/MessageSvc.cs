﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
	public interface IMessageEvent {}
	public class TypeEvent<T> : IMessageEvent
	{
		protected event Action<T> EventHandler;

		public void Add(Action<T> action)
		{
			if (EventHandler == null || !EventHandler.GetInvocationList().Contains(action))
				EventHandler += action;
		}

		public void Remove(Action<T> action)
		{
			EventHandler -= action;
		}

		public void Send(T data)
		{
			EventHandler(data);
		}
	}

	public class MessageEvent : IMessageEvent
	{
		protected event Action EventHandler;

		public void Add(Action action)
		{
			if (EventHandler == null || !EventHandler.GetInvocationList().Contains(action))
				EventHandler += action;
		}

		public void Remove(Action action)
		{
			EventHandler -= action;
		}

		public void Send()
		{
			EventHandler();
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
			if (!_messageHandlers.ContainsKey(messageID))
			{
				_messageHandlers.Add(messageID, new MessageEvent());
			}
			_messageHandlers[messageID].Add(callback);
		}

		public void Subscribe<T>(string messageID, Action<T> callback)
		{
			if (!_messageTypeHandlers.ContainsKey(messageID))
			{
				_messageTypeHandlers.Add(messageID, new Dictionary<Type, IMessageEvent>());
			}
			Type t = typeof(T);
			if (!_messageTypeHandlers[messageID].ContainsKey(t))
			{
				_messageTypeHandlers[messageID].Add(t, new TypeEvent<T>());
			}
			(_messageTypeHandlers[messageID][t] as TypeEvent<T>).Add(callback);
		}

		public void Subscribe<T>(Action<T> callback)
		{
			Type t = typeof(T);
			if (!_typeHandlers.ContainsKey(t))
			{
				_typeHandlers.Add(t, new TypeEvent<T>());
			}
			(_typeHandlers[t] as TypeEvent<T>).Add(callback);
		}

		public void Unsubscribe(string messageID, Action callback)
		{
			if (_messageHandlers.ContainsKey(messageID))
			{
				_messageHandlers[messageID].Remove(callback);
			}
		}

		public void Unsubscribe<T>(Action<T> callback)
		{
			Type t = typeof(T);
			if (_typeHandlers.ContainsKey(t))
			{
				(_typeHandlers[t] as TypeEvent<T>).Remove(callback);
			}
		}

		public void Unsubscribe<T>(string messageID, Action<T> callback)
		{
			Type t = typeof(T);
			if (_messageTypeHandlers.ContainsKey(messageID) && _messageTypeHandlers[messageID].ContainsKey(t))
			{
				(_messageTypeHandlers[messageID][t] as TypeEvent<T>).Remove(callback);
			}
		}

		public void Send(string messageID)
		{
			MessageEvent e;
			if (_messageHandlers.TryGetValue(messageID, out e))
				e.Send();
		}

		public void Send<T>(string messageID, T messageData)
		{
			IMessageEvent e;
			Dictionary<Type, IMessageEvent> d;
			Type t = typeof(T);
			if (_messageTypeHandlers.TryGetValue(messageID, out d) && d.TryGetValue(t, out e))
				(e as TypeEvent<T>).Send(messageData);
		}

		public void Send<T>(T messageData)
		{
			IMessageEvent e;
			Type t = typeof(T);
			if (_typeHandlers.TryGetValue(t, out e))
				(e as TypeEvent<T>).Send(messageData);
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