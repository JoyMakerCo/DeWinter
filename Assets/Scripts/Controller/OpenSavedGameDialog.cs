using System;
using System.Collections.Generic;
using Core;
using UFlow;
namespace Ambition
{
    public class OpenSavedGameDialog : ICommand
    {
        public void Execute() => AmbitionApp.OpenDialog(DialogConsts.RESTORE_GAME);
    }

    public class LoadSavedGameCmd : ICommand<string>
    {
        private string _record;
        public void Execute(string record)
        {
            UFlowSvc uflow = AmbitionApp.GetService<UFlowSvc>();
            AmbitionApp.CloseAllDialogs();
            _record = record;
           
            AmbitionApp.SendMessage(AudioMessages.STOP_AMBIENT);
            AmbitionApp.SendMessage(AudioMessages.STOP_MUSIC);

            AmbitionApp.Subscribe(GameMessages.FADE_OUT_COMPLETE, HandleFadeOut);
            AmbitionApp.SendMessage(GameMessages.FADE_OUT);
        }

        private void HandleFadeOut()
        {
            AmbitionApp.Unsubscribe(GameMessages.FADE_OUT_COMPLETE, HandleFadeOut);
            AmbitionApp.Execute<LoadGameCmd, string>(_record);
        }
    }
}
