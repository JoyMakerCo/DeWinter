using System;
using Core;
using UFlow;
namespace Ambition
{
    public class QuitGameCmd : ICommand
    {
        public void Execute()
        {
            AmbitionApp.OpenDialog("quit.dialog", OnQuit);
        }

        private void OnQuit()
        {
            UnityEngine.Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
