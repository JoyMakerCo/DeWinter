using System;
namespace Ambition
{
    public class PlaySoundCmd : Core.ICommand<FMODEvent>
    {
        public void Execute(FMODEvent sound)
        {
            FMODUnity.RuntimeManager.PlayOneShot(sound.Name);
        }
    }
}
