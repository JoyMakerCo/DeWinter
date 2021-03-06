﻿using System;
using UFlow;
using Core;
namespace Ambition
{
    public class ResetGameCmd : Core.ICommand
    {
        public void Execute()
        {
            UFlowSvc uflow = AmbitionApp.GetService<UFlowSvc>();
            AmbitionApp.CloseAllDialogs();
            AmbitionApp.GetService<ModelSvc>().Reset();

            string[] machines = uflow.GetActiveMachines();
            Array.ForEach(machines, m => uflow.GetMachine(m)?.Dispose());
            AmbitionApp.RegisterCommand<FadeToMenuCmd>(GameMessages.FADE_OUT_COMPLETE);
            AmbitionApp.SendMessage(AudioMessages.STOP_AMBIENT);
            AmbitionApp.SendMessage(AudioMessages.STOP_MUSIC);

            AmbitionApp.SendMessage(GameMessages.FADE_OUT);
        }
    }

    public class FadeToMenuCmd : Core.ICommand
    {
        public void Execute()
        {
            AmbitionApp.UnregisterCommand<FadeToMenuCmd>(GameMessages.FADE_OUT_COMPLETE);
            AmbitionApp.SendMessage(GameMessages.LOAD_SCENE, SceneConsts.START_SCENE);
            AmbitionApp.SendMessage(GameMessages.HIDE_HEADER);
            AmbitionApp.SendMessage(GameMessages.FADE_IN);
        }
    }
}
