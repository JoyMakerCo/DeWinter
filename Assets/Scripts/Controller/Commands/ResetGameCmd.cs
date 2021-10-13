using System;
using UFlow;
using Core;
namespace Ambition
{
    public class ResetGameCmd : Core.ICommand
    {
        public void Execute()
        {
            AmbitionApp.SendMessage(GameMessages.AUTOSAVE);
            AmbitionApp.GetService<UFlowSvc>().Reset();
            AmbitionApp.GetService<ModelSvc>().Reset();
            AmbitionApp.CloseAllDialogs();
            AmbitionApp.SendMessage(AudioMessages.STOP_MUSIC);
            AmbitionApp.SendMessage(AudioMessages.STOP_AMBIENT);
            AmbitionApp.SendMessage(GameMessages.LOAD_SCENE, SceneConsts.START_SCENE);
        }
    }
}
