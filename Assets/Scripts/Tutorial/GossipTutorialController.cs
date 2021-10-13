using System;
using UFlow;
namespace Ambition
{
    public class GossipTutorialController : UFlow.UFlowConfig
    {
        public override void Configure()
        {
            State("GossipTutorialStart");
            State("DisplayGossipTutorialInput");
            State("GossipTutorialSell");
            State("SellGossipTutorialInput");
            State("GossipTutorialConfirm");
            State("ConfirmGossipTutorialInput");
            State("GossipTutorialLeave");
            State("GossipTutorialLeaveInput");
            State("GossipTutorialExit");

            Bind<TutorialState>("GossipTutorialStart");
            Bind<TutorialState>("GossipTutorialSell");
            Bind<TutorialState>("GossipTutorialConfirm");
            Bind<TutorialState>("GossipTutorialLeave");
            Bind<EndTutorialState>("GossipTutorialExit");

            Bind<DisplayGossipInput>("DisplayGossipTutorialInput");
            Bind<SellGossipInput>("SellGossipTutorialInput");
            Bind<DialogCloseInput, string>("ConfirmGossipTutorialInput", DialogConsts.MESSAGE);
            Bind<MessageInput, string>("GossipTutorialLeaveInput", TutorialMessages.TUTORIAL_NEXT_STEP);
        }

        public class DisplayGossipInput : UInput, Util.IInitializable, IDisposable
        {
            public void Initialize() => AmbitionApp.Subscribe<GossipVO>(InventoryMessages.DISPLAY_GOSSIP, OnDisplayGossip);
            public void Dispose() => AmbitionApp.Unsubscribe<GossipVO>(InventoryMessages.DISPLAY_GOSSIP, OnDisplayGossip);
            private void OnDisplayGossip(GossipVO gossip) => Activate();
        }

        public class SellGossipInput : UInput, Util.IInitializable, IDisposable
        {
            public void Initialize() => AmbitionApp.Subscribe<GossipVO>(InventoryMessages.SELL_GOSSIP, OnSellGossip);
            public void Dispose() => AmbitionApp.Unsubscribe<GossipVO>(InventoryMessages.SELL_GOSSIP, OnSellGossip);
            private void OnSellGossip(GossipVO gossip) => Activate();
        }

        public class DialogCloseInput : UInput, Util.IInitializable<string>, IDisposable
        {
            private string _dialogID;
            public void Dispose() => AmbitionApp.Unsubscribe<string>(GameMessages.DIALOG_CLOSED, Handler);
            public void Initialize(string dialogID)
            {
                _dialogID = dialogID;
                AmbitionApp.Subscribe<string>(GameMessages.DIALOG_CLOSED, Handler);
            }
            private void Handler(string dialogID)
            {
                if (dialogID == _dialogID) Activate();
            }
        }
    }
}
