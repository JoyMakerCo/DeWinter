using System;
using System.Collections.Generic;
using Core;

namespace DeWinter
{
	public static class DeWinterApp
	{
		public static T RegisterModel<T>() where T:IModel, new()
		{
			return App.Service<ModelSvc>().Register<T>();
		}

		public static void UnregisterModel<T>() where T:IModel
		{
			App.Service<ModelSvc>().Unregister<T>();
		}

		public static T GetModel<T>() where T:IModel
		{
			return App.Service<ModelSvc>().GetModel<T>();
		}

		/// COMMANDS
		public static void RegisterCommand<C>(string messageID) where C:ICommand, new()
		{
			App.Service<CommandSvc>().Register<C>(messageID);
		}

		public static void RegisterCommand<C, T>() where C:ICommand<T>, new()
		{
			App.Service<CommandSvc>().Register<C, T>();
		}

		public static void RegisterCommand<C, T>(string messageID) where C:ICommand<T>, new()
		{
			App.Service<CommandSvc>().Register<C, T>(messageID);
		}

		public static void UnregisterCommand<C>(string messageID) where C:ICommand
		{
			App.Service<CommandSvc>().Unregister<C>(messageID);
		}

		public static void UnregisterCommand<C, T>(string messageID) where C:ICommand
		{
			App.Service<CommandSvc>().Unregister<C, T>(messageID);
		}

		public static void UnregisterCommand<C, T>() where C:ICommand
		{
			App.Service<CommandSvc>().Unregister<C, T>();
		}

		/// MESSAGES
		public static void SendMessage(string messageID)
		{
			App.Service<MessageSvc>().Send(messageID);
		}

		public static void SendMessage<T>(string messageID, T msg)
		{
			App.Service<MessageSvc>().Send<T>(messageID, msg);
		}

		public static void SendMessage<T>(T msg)
		{
			App.Service<MessageSvc>().Send<T>(msg);
		}

		public static void Subscribe<T>(Action<T> callback)
		{
			App.Service<MessageSvc>().Subscribe<T>(callback);
		}

		public static void Subscribe(string messageID, Action callback)
		{
			App.Service<MessageSvc>().Subscribe(messageID, callback);
		}

		public static void Subscribe<T>(string messageID, Action<T> callback)
		{
			App.Service<MessageSvc>().Subscribe(messageID, callback);
		}

		public static void Unsubscribe(string message, Action callback)
		{
			App.Service<MessageSvc>().Unsubscribe(message, callback);
		}

		public static void Unsubscribe<T>(string message, Action<T> callback)
		{
			App.Service<MessageSvc>().Unsubscribe<T>(callback);
		}

		public static void Unsubscribe<T>(Action<T> callback)
		{
			App.Service<MessageSvc>().Unsubscribe<T>(callback);
		}
	}
}