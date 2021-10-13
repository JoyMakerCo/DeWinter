using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Core;
using Newtonsoft.Json;

namespace Ambition
{
    public class StartupInitBehavior
    {
        [RuntimeInitializeOnLoadMethod]
        static void Initialize()
        {
            LocalizationSvc loc = App.Register<LocalizationSvc>();
            App.Register<MessageSvc>();
            App.Register<CommandSvc>();
            App.Register<ModelSvc>().Register<LocalizationModel>();
            AudioSvc audio = App.Register<AudioSvc>();

            AmbitionApp.RegisterCommand<LoadGameCmd, string>(GameMessages.LOAD_GAME);
            AmbitionApp.RegisterCommand<QuitGameCmd>(GameMessages.QUIT_GAME);
            AmbitionApp.RegisterCommand<AmbitionErrorCmd, ErrorEventArgs>(GameMessages.ERROR);

            // Load game settings
            Application.targetFrameRate = 60;
            string filepath = Path.Combine(UnityEngine.Application.persistentDataPath, Filepath.SETTINGS);
            GameSettingsVO settings;
            if (File.Exists(filepath))
            {
                string json = File.ReadAllText(filepath);
                settings = JsonConvert.DeserializeObject<GameSettingsVO>(json);
            }
            else
            {
                settings = new GameSettingsVO() { Fullscreen = true };
                for (int i=0; i< GameSettingsVO.RESOLUTIONS.Length; i+=2)
                {
                    if (Screen.currentResolution.width >= GameSettingsVO.RESOLUTIONS[i])
                    {
                        settings.Resolution = new Resolution()
                        {
                            width = GameSettingsVO.RESOLUTIONS[i],
                            height = GameSettingsVO.RESOLUTIONS[i + 1],
                            refreshRate = Screen.currentResolution.refreshRate
                        };
                        break;
                    }
                }
            }

            foreach (KeyValuePair<AudioChannel, int> volume in settings.Volume)
            {
                audio.SetVolume(volume.Key, volume.Value);
            }

            // TODO: Finish locs
            TextAsset txt = Resources.Load<TextAsset>(Filepath.LOCALIZATIONS + SystemLanguage.English.ToString());
            //TextAsset txt = Resources.Load<TextAsset>(Filepath.LOCALIZATIONS + settings.Language.ToString());
            if (txt != null)
            {
                // TODO: Load loc-relevant fonts in assetbundle
                FontSet fonts = Resources.Load<FontSet>("LocFonts/" + SystemLanguage.English.ToString() + "/Fonts");
                loc.DefaultLanguage = settings.Language;
                loc.LoadLocFile(settings.Language, txt.text);
                loc.ClearProxyFonts();
                loc.DefaultFont = fonts?.Default;
                if (fonts?.Fonts != null)
                {
                    foreach (FontSet.FontSubstitution proxy in fonts.Fonts)
                    {
                        loc.SetProxyFont(proxy.ProxyFont, proxy.SubstitutionFont);
                    }
                }
            }
            Screen.SetResolution(settings.Resolution.width, settings.Resolution.height, settings.Fullscreen, settings.Resolution.refreshRate);
            AmbitionApp.SendMessage(GameMessages.UPDATE_LOCALIZATION, settings.Language);
        }
    }
}
