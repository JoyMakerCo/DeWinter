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

	public interface IPersistentCommand
	{
		void OnCompleteCommand();
	}

	public class CommandSvc : IAppService
	{
		private Dictionary<string, Dictionary<Type, Delegate>> _messageAssociations;
		private Dictionary<Type, Dictionary<Type, Delegate>> _typeAssociations;
		private List<IPersistentCommand> _commands;

		public CommandSvc()
		{
			_messageAssociations = new Dictionary<string, Dictionary<Type, Delegate>>();
			_typeAssociations = new Dictionary<Type, Dictionary<Type, Delegate>>();
		}

		public void Register<C, T>() where C : ICommand<T>, new()
		{
			Type t = typeof(T);
			Type c = typeof(C);
			if (!_typeAssociations.ContainsKey(t))
			{
				_typeAssociations.Add(t, new Dictionary<Type, Delegate>());
			}
			if (!_typeAssociations[t].ContainsKey(c))
			{
				Action<T> handleMsg = msg => new C().Execute(msg);
				_typeAssociations[t].Add(c, handleMsg);
				App.Service<MessageSvc>().Subscribe<T>(handleMsg);
			}
		}

		public void Register<C>(string messageID) where C : ICommand, new()
		{
			Type c = typeof(C);
			if (!_messageAssociations.ContainsKey(messageID))
			{
				_messageAssociations.Add(messageID, new Dictionary<Type, Delegate>());
			}
			if (!_messageAssociations[messageID].ContainsKey(c))
			{
				Action handleMsg = () => new C().Execute();
				_messageAssociations[messageID].Add(c, handleMsg);
				App.Service<MessageSvc>().Subscribe(messageID, handleMsg);
			}
		}

		public void Register<C, T>(string messageID) where C : ICommand<T>, new()
		{
			Type c = typeof(C);
			if (!_messageAssociations.ContainsKey(messageID))
			{
				_messageAssociations.Add(messageID, new Dictionary<Type, Delegate>());
			}
			if (!_messageAssociations[messageID].ContainsKey(c))
			{
				Action<T> handleMsg = msg => new C().Execute(msg);
				_messageAssociations[messageID].Add(c, handleMsg);
				App.Service<MessageSvc>().Subscribe(messageID, handleMsg);
			}
		}

		public void Execute<C>() where C : ICommand, new()
		{
			new C().Execute();
		}

		public void Execute<C, T>(T value) where C: ICommand<T>, new() 
		{
			new C().Execute(value);
		}

		public void Unregister<C, T>() where C : ICommand<T>
		{
			Type t = typeof(T);
			if (_typeAssociations.ContainsKey(t))
			{
				Type c = typeof(C);
				if (_typeAssociations[t].ContainsKey(c))
					App.Service<MessageSvc>().Unsubscribe<T>(_typeAssociations[t][c] as Action<T>);
				_typeAssociations[t].Remove(typeof(C));
			}
		}

		public void Unregister<C, T>(string messageID) where C : ICommand<T>
		{
			if (_messageAssociations.ContainsKey(messageID))
			{
				Type c = typeof(C);
				if (_messageAssociations[messageID].ContainsKey(c))
					App.Service<MessageSvc>().Unsubscribe<T>(messageID, _messageAssociations[messageID][c] as Action<T>);
				_messageAssociations[messageID].Remove(c);
			}
		}

		public void Unregister<C>(string messageID) where C : ICommand
		{
			if (_messageAssociations.ContainsKey(messageID))
			{
				Type c = typeof(C);
				if (_messageAssociations[messageID].ContainsKey(c))
					App.Service<MessageSvc>().Unsubscribe(messageID, _messageAssociations[messageID][c] as Action);
				_messageAssociations[messageID].Remove(c);
			}
		}
	}
}
