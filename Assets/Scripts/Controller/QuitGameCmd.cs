using System;
using Core;
using UFlow;
namespace Ambition
{
    public class QuitGameCmd : ICommand
    {
        public void Execute()
        {
            UFlowSvc uflow = AmbitionApp.GetService<UFlowSvc>();
            AmbitionApp.CloseAllDialogs();

            string[] machines = uflow.GetActiveMachines();
            Array.ForEach(machines, m => uflow.GetMachine(m)?.Dispose());
            AmbitionApp.Subscribe(GameMessages.FADE_OUT_COMPLETE, HandleFade);
            AmbitionApp.SendMessage(AudioMessages.STOP_AMBIENT);
            AmbitionApp.SendMessage(AudioMessages.STOP_MUSIC);
            AmbitionApp.SendMessage(GameMessages.FADE_OUT);
        }

        private void HandleFade()
        {
            UnityEngine.Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
