using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ambition
{
    public class GameSettingsVO
    {
        public static int[] RESOLUTIONS = new int[] { 1920, 1080, 1600, 900, 1366, 768, 1280, 720, 1024, 576 };

        [JsonProperty("volume")]
        public Dictionary<AudioChannel, int> Volume = new Dictionary<AudioChannel, int>()
        {
            {AudioChannel.Master,100},
            {AudioChannel.Ambient,100},
            {AudioChannel.Sfx,100},
            {AudioChannel.Music,100}
        };

        [JsonProperty("language")]
        public UnityEngine.SystemLanguage Language = UnityEngine.Application.systemLanguage;

        public UnityEngine.Resolution Resolution
        {
            get => new UnityEngine.Resolution()
            {
                width = _width,
                height = _height,
            };
            set
            {
                _width = value.width;
                _height = value.height;
            }
        }

        [JsonProperty("fullscreen")]
        public bool Fullscreen = true;

        [JsonProperty("res_w")]
        private int _width;

        [JsonProperty("res_h")]
        private int _height;
    }
}
