using UnityEngine;
using System.Collections;
using Steamworks;

namespace Ambition
{
    public class SteamComponent : MonoBehaviour
    {
        private const int STEAM_ID = 949200;
//        private const int STEAM_ID = 1313980;

        private void Initialize()
        {
            Core.App.Register<RewardFactorySvc>();
            Core.App.Register<RequirementsSvc>();
            AmbitionApp.RegisterReward<SteamAchievementReward>(CommodityType.Achievement);
            AmbitionApp.RegisterRequirement(CommodityType.Achievement, SteamAchievementReq.Check);
        }

        private void Awake()
        {
#if !UNITY_ANDROID && !UNITY_IOS && !UNITY_TIZEN && !UNITY_TVOS && !UNITY_WEBGL && !UNITY_WSA && !UNITY_PS4 && !UNITY_WII && !UNITY_XBOXONE && !UNITY_SWITCH
            if (Core.App.Service<SteamworksSvc>() == null)
            {
                if (!Packsize.Test())
                {
                    Debug.LogError("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", this);
                }

                if (!DllCheck.Test())
                {
                    Debug.LogError("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.", this);
                }

#if !UNITY_EDITOR
                try {
                    // If Steam is not running or the game wasn't started through Steam, SteamAPI_RestartAppIfNecessary starts the
                    // Steam client and also launches this game again if the User owns it. This can act as a rudimentary form of DRM.

                    // Once you get a Steam AppID assigned by Valve, you need to replace AppId_t.Invalid with it and
                    // remove steam_appid.txt from the game depot. eg: "(AppId_t)480" or "new AppId_t(480)".
                    // See the Valve documentation for more information: https://partner.steamgames.com/doc/sdk/api#initialization_and_shutdown
                    if (SteamAPI.RestartAppIfNecessary((AppId_t)STEAM_ID))
                    {
                        Application.Quit();
                        return;
                    }
                }
                catch (System.DllNotFoundException e)
                { // We catch this exception here, as it will be the first occurrence of it.
                    Debug.LogError("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + e, this);

                    Application.Quit();
                    return;
                }
#endif
                // Initializes the Steamworks API.
                // If this returns false then this indicates one of the following conditions:
                // [*] The Steam client isn't running. A running Steam client is required to provide implementations of the various Steamworks interfaces.
                // [*] The Steam client couldn't determine the App ID of game. If you're running your application from the executable or debugger directly then you must have a [code-inline]steam_appid.txt[/code-inline] in your game directory next to the executable, with your app ID in it and nothing else. Steam will look for this file in the current working directory. If you are running your executable from a different directory you may need to relocate the [code-inline]steam_appid.txt[/code-inline] file.
                // [*] Your application is not running under the same OS user context as the Steam client, such as a different user or administration access level.
                // [*] Ensure that you own a license for the App ID on the currently active Steam account. Your game must show up in your Steam library.
                // [*] Your App ID is not completely set up, i.e. in Release State: Unavailable, or it's missing default packages.
                // Valve's documentation for this is located here:
                // https://partner.steamgames.com/doc/sdk/api#initialization_and_shutdown
                if (!SteamAPI.Init())
                {
                    Debug.LogError("[Steamworks.NET] SteamAPI_Init() failed. Steam is likely not running. Refer to Valve's documentation or the comment above this line for more information.", this);
                }
                else
                {
                    Core.App.Register<SteamworksSvc>();
                    Initialize();
                }
            }
#endif
            Destroy(gameObject);
        }
    }
}
