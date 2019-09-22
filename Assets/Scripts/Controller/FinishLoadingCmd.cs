using System;
using Core;
namespace Ambition
{
    public class FinishLoadingCmd : ICommand
    {
        public void Execute()
        {
            AmbitionApp.UnregisterCommand<FinishLoadingCmd>(AudioMessages.ALL_SOUNDS_LOADED);
            App.Service<UFlow.UFlowSvc>().InvokeMachine("DayFlowController");
            AmbitionApp.SendMessage(GameMessages.START_GAME);
        }
    }
}
