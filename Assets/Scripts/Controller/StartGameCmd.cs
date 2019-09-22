using System;
namespace Ambition
{
    public class StartGameCmd<T> : Core.ICommand<T>
    {
        public void Execute(T config)
        {
            AmbitionApp.Execute<InitGameCmd>();
            AmbitionApp.Execute<InitPlayerCmd, PlayerConfig>(config as PlayerConfig);
//            AmbitionApp.Execute<RestoreGameCmd, string>(config as string);
            if (!FMODUnity.RuntimeManager.AnyBankLoading()) AmbitionApp.Execute<FinishLoadingCmd>();
            else AmbitionApp.RegisterCommand<FinishLoadingCmd>(AudioMessages.ALL_SOUNDS_LOADED);
        }
    }
}
