using UnityEngine;

using System;
using System.Collections.Generic;
using Dialog;
using UFlow;
using Util;
using Core;

namespace Ambition
{
    public static class AmbitionApp
    {
        public static T RegisterModel<T>() where T : Model, new()
        {
            return App.Service<ModelSvc>().Register<T>();
        }

        public static T GetService<T>() where T : IAppService => App.Service<T>();

        public static void UnregisterModel<T>() where T : Model
        {
            App.Service<ModelSvc>().Unregister<T>();
        }

        public static T GetModel<T>() where T : Model => App.Service<ModelSvc>().GetModel<T>();

        /// COMMANDS
        public static void RegisterCommand<C>(string messageID) where C : ICommand, new()
        {
            App.Service<CommandSvc>().Register<C>(messageID);
        }

        public static void RegisterCommand<C, T>() where C : ICommand<T>, new()
        {
            App.Service<CommandSvc>().Register<C, T>();
        }

        public static void RegisterCommand<C, T>(string messageID) where C : ICommand<T>, new()
        {
            App.Service<CommandSvc>().Register<C, T>(messageID);
        }

        public static void RegisterCommand<C, T>(string messageID, T data) where C : ICommand<T>, new()
        {
            App.Service<CommandSvc>().Register<C, T>(messageID, data);
        }

        public static void Execute<C>() where C : ICommand, new()
        {
            App.Service<CommandSvc>().Execute<C>();
        }

        public static void Execute<C, T>(T value) where C : ICommand<T>, new()
        {
            App.Service<CommandSvc>().Execute<C, T>(value);
        }

        public static void UnregisterCommand<C>(string messageID) where C : ICommand
        {
            App.Service<CommandSvc>().Unregister<C>(messageID);
        }

        public static void UnregisterCommand<C, T>(string messageID) where C : ICommand<T>
        {
            App.Service<CommandSvc>().Unregister<C, T>(messageID);
        }

        public static void UnregisterCommand<C, T>() where C : ICommand<T>
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

        public static GameObject OpenDialog(string phrase, Dictionary<string, string> substitutions = null)
        {
            GameObject dialog = OpenDialog<string>(DialogConsts.MESSAGE, phrase);
            MessageViewMediator mediator = dialog.GetComponent<MessageViewMediator>();
            if (mediator) mediator.SetPhrase(phrase, substitutions);
            return dialog;
        }

        public static GameObject OpenDialog(string phrase, Action OnConfirm, Dictionary<string, string> substitutions = null)
        {
            GameObject dialog = OpenDialog<string>(DialogConsts.CHOICE, phrase);
            MessageViewMediator mediator = dialog.GetComponent<MessageViewMediator>();
            if (mediator)
            {
                mediator.SetPhrase(phrase, substitutions);
                mediator.SetConfirmAction(OnConfirm);
            }
            return dialog;
        }


        public static void CloseDialog(string dialogID)
        {
            App.Service<DialogSvc>().Close(dialogID);
        }

        public static void CloseDialog(GameObject dialog)
        {
            App.Service<DialogSvc>().Close(dialog);
        }

        public static void CloseAllDialogs()
        {
            App.Service<DialogSvc>().CloseAll();
        }

        public static void RegisterState<C>(string machineID, string stateID) where C : UState, new()
        {
            App.Service<UFlowSvc>().RegisterState<C>(machineID, stateID);
        }

        public static void RegisterState(string machineID, string stateID)
        {
            App.Service<UFlowSvc>().RegisterState(machineID, stateID);
        }

        public static void RegisterState<C, T>(string machineID, string stateID, T arg) where C : UState, IInitializable<T>, new()
        {
            App.Service<UFlowSvc>().RegisterState<C, T>(machineID, stateID, arg);
        }

        // Binds an existing state to a new node type
        public static void BindState<T>(string machineID, string stateID) where T:UState, new()
        {
            App.Service<UFlowSvc>().BindState<T>(machineID, stateID);
        }

        // Binds an existing state to a new node type
        public static void BindState<S, D>(string machineID, string stateID, D data) where S : UState, IInitializable<D>, new()
        {
            App.Service<UFlowSvc>().BindState<S,D>(machineID, stateID, data);
        }

        public static void RegisterLink(string machineID, string originState, string targetState)
		{
            App.Service<UFlowSvc>().RegisterLink(machineID, originState, targetState);
		}

		public static void RegisterLink<T>(string machineID, string originState, string targetState) where T : ULink, new()
		{
            App.Service<UFlowSvc>().RegisterLink<T>(machineID, originState, targetState);
		}

		public static void RegisterLink<T, U>(string machineID, string originState, string targetState, U data) where T : ULink, IInitializable<U>, new()
		{
            App.Service<UFlowSvc>().RegisterLink<T, U>(machineID, originState, targetState, data);
		}

        public static bool IsActiveState(string stateID)
		{
            return App.Service<UFlowSvc>().IsActiveState(stateID);
		}

		public static bool IsActiveMachine(string machineID)
		{
            return App.Service<UFlowSvc>().IsActiveMachine(machineID);
		}

        public static string Localize(string key)
		{
            LocalizationModel model = AmbitionApp.GetModel<LocalizationModel>();
			string result = App.Service<LocalizationSvc>().GetString(key, model.Substitutions);
#if DEBUG
            if (string.IsNullOrEmpty(result))
            {
                Debug.LogWarning("Warning: No localizations found for key \"" + key + "\"");
            }
#endif
            return result;
        }

        public static string GetString(string key)
        {
            return App.Service<LocalizationSvc>().GetString(key);
        }

        public static string GetString(string key, Dictionary<string, string> substitutions)
		{
            LocalizationModel model = AmbitionApp.GetModel<LocalizationModel>();
            if (substitutions == null)
            {
                return App.Service<LocalizationSvc>().GetString(key, model.Substitutions);
            }

            foreach (KeyValuePair<string, string> kvp in model.Substitutions)
            {
                if (!substitutions.ContainsKey(kvp.Key))
                {
                    substitutions[kvp.Key] = kvp.Value;
                }
            }
            return App.Service<LocalizationSvc>().GetString(key, substitutions);
		}

		public static Dictionary<string, string> GetPhrases(string key)
		{
            return App.Service<LocalizationSvc>().GetPhrases(key);
		}

		public static void RegisterFactory<Key, Product>(IFactory<Key, Product> factory)
		{
			App.Service<FactorySvc>().Register<Key, Product>(factory);
		}
		public static IFactory<Key, Product> GetFactory<Key, Product>()
		{
			return App.Service<FactorySvc>().GetFactory<Key, Product>();
		}

		public static Product Create<Key, Product>(Key key)
		{
			return App.Service<FactorySvc>().Create<Key, Product>(key);
		}

        public static void RegisterReward<T>(CommodityType type) where T : ICommand<CommodityVO>, new()
        {
            App.Service<RewardFactorySvc>().RegisterReward<T>(type);
        }

        public static void Reward(CommodityVO commodity)
        {
            App.Service<RewardFactorySvc>().Reward(commodity);
        }

        public static void Reward(CommodityType type, int value=0)
        {
            App.Service<RewardFactorySvc>().Reward(new CommodityVO(type, value));
        }

        public static void Reward(CommodityType type, string id, int value = 0)
        {
            App.Service<RewardFactorySvc>().Reward(new CommodityVO(type, id, value));
        }

        public static void LoadAssetBundle(string bundleID, Action<AssetBundle> onLoad)
        {
            App.Service<AssetBundleSvc>().Load(bundleID, onLoad);
        }

        public static void RegisterRequirement(CommodityType type, Func<RequirementVO, bool> check) => App.Service<RequirementsSvc>().Register(type, check);
        public static bool CheckRequirement(RequirementVO req) => App.Service<RequirementsSvc>().Check(req);
        public static bool CheckRequirements(RequirementVO[] reqs) => App.Service<RequirementsSvc>().Check(reqs);
    }
}
