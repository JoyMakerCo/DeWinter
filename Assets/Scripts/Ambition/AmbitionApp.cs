using UnityEngine;

using System;
using System.Linq;
using System.Collections.Generic;
using Dialog;
using UFlow;
using Core;

namespace Ambition
{
    public static class AmbitionApp
    {
        public static T RegisterModel<T>() where T : Model, new()
        {
            T model = App.Service<ModelSvc>().Register<T>();
            App.Service<ModelTrackingSvc>().Track(model as IResettable);
            return model;
        }

        public static T GetService<T>() where T : IAppService => App.Service<T>();

        public static void Save()
        {
            GameModel model = GetModel<GameModel>();
            string saveID = model.PlayerName + " " + GetModel<CalendarModel>().Today.ToLongDateString();
            App.Service<ModelSvc>().Save(saveID);
        }

        public static bool Restore(string savedState) => App.Service<ModelSvc>().Restore(savedState);

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

        public static void CloseAllDialogs()
        {
            App.Service<DialogSvc>().CloseAll();
        }

        public static void RegisterState<C>(string machineID, string stateID, params object[] parameters) where C : UState, new()
        {
            App.Service<UFlowSvc>().RegisterState<C>(machineID, stateID, parameters);
        }

        public static void RegisterState(string machineID, string stateID, params object[] parameters)
        {
            App.Service<UFlowSvc>().RegisterState(machineID, stateID, parameters);
        }

        // Binds an existing state to a new node type
        public static void BindState<T>(string machineID, string stateID, params object[] parameters) where T:UState, new()
        {
            App.Service<UFlowSvc>().BindState<T>(machineID, stateID, parameters);
        }

        public static void BindLink<T>(string machineID, string originState, string targetState, params object[] parameters) where T : ULink, new()
        {
            App.Service<UFlowSvc>().BindLink<T>(machineID, originState, targetState, parameters);
        }

        public static void RegisterLink(string machineID, string originState, string targetState)
		{
            App.Service<UFlowSvc>().RegisterLink(machineID, originState, targetState);
		}

		public static void RegisterLink<T>(string machineID, string originState, string targetState) where T : ULink, new()
		{
            App.Service<UFlowSvc>().RegisterLink<T>(machineID, originState, targetState);
		}

        public static bool IsActiveState(string stateID)
		{
            return App.Service<UFlowSvc>().IsActiveState(stateID);
		}

		public static bool IsActiveMachine(string machineID)
		{
            return App.Service<UFlowSvc>().IsActiveMachine(machineID);
		}

        public static string GetString(string key)
		{
            LocalizationModel model = AmbitionApp.GetModel<LocalizationModel>();
			return App.Service<LocalizationSvc>().GetString(key, model.Substitutions);
		}

		public static string GetString(string key, Dictionary<string, string> substitutions)
		{
            LocalizationModel model = AmbitionApp.GetModel<LocalizationModel>();
            return App.Service<LocalizationSvc>().GetString(key, substitutions.Concat(model.Substitutions).GroupBy(k=>k.Key).ToDictionary(k=>k.Key, k=>k.First().Value));
		}

		public static string[] GetPhrases(string key)
		{
			return App.Service<LocalizationSvc>().GetList(key);
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
