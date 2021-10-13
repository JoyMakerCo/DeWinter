using System.Threading;
using Steamworks;
namespace Ambition
{
    public class SteamworksSvc : Core.IAppService
    {
        private Thread _steamThread;

        public SteamworksSvc()
        {
            SteamAPIWarningMessageHook_t debug = new SteamAPIWarningMessageHook_t(SteamAPIDebugTextHook);
            _steamThread = new Thread(SteamThread);
            _steamThread.Start();
            SteamClient.SetWarningMessageHook(debug);
        }

        public void Dispose()
        {
            _steamThread.Abort();
            SteamAPI.Shutdown();
        }

        private void SteamThread()
        {
            while(SteamAPI.IsSteamRunning())
            {
                SteamAPI.RunCallbacks();
                Thread.Sleep(500);
            }
        }

        private void SteamAPIDebugTextHook(int nSeverity, System.Text.StringBuilder pchDebugText)
        {
            UnityEngine.Debug.LogWarning(pchDebugText);
        }
    }
}
