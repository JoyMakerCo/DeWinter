using UnityEngine;

using System;
using System.Collections.Generic;
using Dialog;
using UFlow;
using Core;

namespace Ambition
{
	public static class AmbitionApp
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

		public static void Execute<C>() where C:ICommand, new()
		{
			App.Service<CommandSvc>().Execute<C>();
		}

		public static void Execute<C, T>(T value) where C:ICommand<T>, new()
		{
			App.Service<CommandSvc>().Execute<C, T>(value);
		}

		public static void UnregisterCommand<C>(string messageID) where C:ICommand
		{
			App.Service<CommandSvc>().Unregister<C>(messageID);
		}

		public static void UnregisterCommand<C, T>(string messageID) where C:ICommand<T>
		{
			App.Service<CommandSvc>().Unregister<C, T>(messageID);
		}

		public static void UnregisterCommand<C, T>() where C:ICommand<T>
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
			App.Service<MessageSvc>().Unsubscribe<T>(message, callback);
		}

		public static void Unsubscribe<T>(Action<T> callback)
		{
			App.Service<MessageSvc>().Unsubscribe<T>(callback);
		}

		public static GameObject OpenDialog(string DialogID)
		{
			return App.Service<DialogSvc>().Open(DialogID);
		}

		public static GameObject OpenDialog<T>(string DialogID, T Data)
		{
			return App.Service<DialogSvc>().Open<T>(DialogID, Data);
		}

		public static GameObject OpenMessageDialog(string phrase, Dictionary<string, string> substitutions)
		{
			GameObject dialog = OpenDialog<string>(DialogConsts.MESSAGE, phrase);
			MessageViewMediator mediator = dialog.GetComponent<MessageViewMediator>();
			if (mediator) mediator.SetPhrase(phrase, substitutions);
			return dialog;
		}

		public static GameObject OpenMessageDialog(string phrase)
		{
			return OpenDialog<string>(DialogConsts.MESSAGE, phrase);
		}

		public static void CloseDialog(string dialogID)
		{
			App.Service<DialogSvc>().Close(dialogID);
		}

		public static void CloseDialog(GameObject dialog)
		{
			App.Service<DialogSvc>().Close(dialog);
		}

		public static void RegisterState<C>(string machineID, string stateID) where C : UState, new()
		{
			App.Service<UFlowSvc>().RegisterState<C>(machineID, stateID);
		}

		public static void RegisterState(string machineID, string stateID)
		{
			App.Service<UFlowSvc>().RegisterState(machineID, stateID);
		}

		public static void RegisterState<C, T>(string machineID, string stateID, T arg) where C : UState<T>, new()
		{
			App.Service<UFlowSvc>().RegisterState<C, T>(machineID, stateID, arg);
		}

		public static void RegisterLink(string machineID, string originState, string targetState)
		{
			App.Service<UFlowSvc>().RegisterLink(machineID, originState, targetState);
		}

		public static void RegisterLink<T>(string machineID, string originState, string targetState) where T : ULink, new()
		{
			App.Service<UFlowSvc>().RegisterLink<T>(machineID, originState, targetState);
		}

		public static void RegisterLink<T, U>(string machineID, string originState, string targetState, U data) where T : ULink<U>, new()
		{
			App.Service<UFlowSvc>().RegisterLink<T,U>(machineID, originState, targetState, data);
		}

		public static void InvokeMachine(string MachineID)
		{
			App.Service<UFlowSvc>().InvokeMachine(MachineID);
		}

		public static bool IsActiveState(string stateID)
		{
			return App.Service<UFlowSvc>().IsActiveState(stateID);
		}

		public static string GetString(string key)
		{
			return App.Service<ModelSvc>().GetModel<LocalizationModel>().GetString(key);
		}

		public static string GetString(string key, Dictionary<string, string> substitutions)
		{
			return App.Service<ModelSvc>().GetModel<LocalizationModel>().GetString(key, substitutions);
		}

		public static string[] GetPhrases(string key)
		{
			return App.Service<ModelSvc>().GetModel<LocalizationModel>().GetList(key);
		}
	}
}
