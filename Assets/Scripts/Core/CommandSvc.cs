using System;
using System.Collections.Generic;
using Util;

namespace Core
{
// TODO: Test callbacks within commands
// Possible approach: Make an interface or abstract class for commands with callbacks
// Find out if that's necessary

	public interface ICommand<T>
	{
		void Execute(T msg);
	}

	public interface ICommand
	{
		void Execute();
	}

	public class CommandSvc : IAppService
	{
		private Dictionary<string, List<Delegate>> _messageAssociations;
		private Dictionary<Type, List<Delegate>> _typeAssociations;

		private delegate void MessageDelegate<T, C>(T msg);
		private delegate void CommandDelegate<C>();

		public CommandSvc()
		{
			_messageAssociations = new Dictionary<string, List<Delegate>>();
			_typeAssociations = new Dictionary<Type, List<Delegate>>();
		}

		public void Register<C, T>() where C : ICommand<T>, new()
		{
			Type t = typeof(T);
			if (!_typeAssociations.ContainsKey(t))
			{
				_typeAssociations.Add(t, new List<Delegate>());
				App.Service<MessageSvc>().Subscribe<T>(HandleMessage<T>);
			}
			Action<T> handleMsg = msg => new C().Execute(msg);
			_typeAssociations[t].Add(handleMsg);
		}

		public void Register<C>(string messageID) where C : ICommand, new()
		{
			if (!_messageAssociations.ContainsKey(messageID))
			{
				_messageAssociations.Add(messageID, new List<Delegate>());
			}
			Action handleMsg = delegate() { new C().Execute(); };
			_messageAssociations[messageID].Add(handleMsg);
			App.Service<MessageSvc>().Subscribe(messageID, handleMsg);
		}

		public void Register<C, T>(string messageID) where C : ICommand<T>, new()
		{
			if (!_messageAssociations.ContainsKey(messageID))
			{
				_messageAssociations.Add(messageID, new List<Delegate>());
			}
			Action<T> handleMsg = msg => new C().Execute(msg);
			_messageAssociations[messageID].Add(handleMsg);
			App.Service<MessageSvc>().Subscribe<T>(messageID, handleMsg);
		}

// TODO: Implement unregistering commands
		public void Unregister<C, T>() where C : ICommand
		{
//			App.Service<MessageSvc>().Unsubscribe
		}

		public void Unregister<C, T>(string messageID) where C : ICommand
		{
//			App.Service<MessageSvc>().Unsubscribe
		}

		public void Unregister<C>(string messageID) where C : ICommand
		{
//			App.Service<MessageSvc>().Unsubscribe
		}

		private void HandleMessage(string messageID)
		{
			List<Delegate> delegates;
			if (_messageAssociations.TryGetValue(messageID, out delegates))
			{
				foreach(Delegate d in delegates)
				{
					if (d is Action)
						(d as Action).Invoke();
				}
			}
		}

		private void HandleMessage<T>(string messageID, T msg)
		{
			List<Delegate> delegates;
			if (_messageAssociations.TryGetValue(messageID, out delegates))
			{
				foreach(Delegate d in delegates)
				{
					if (d is Action<T>)
						(d as Action<T>).Invoke(msg);
				}
			}
		}

		private void HandleMessage<T>(T msg)
		{
			List<Delegate> delegates;
			if (_typeAssociations.TryGetValue(typeof(T), out delegates))
			{
				foreach(Delegate d in delegates)
				{
					if (d is Action<T>)
						(d as Action<T>).Invoke(msg);
				}
			}
		}
	}
}