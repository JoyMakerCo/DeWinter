namespace Ambition
{
    public static class AudioMessages
    {
        public const string PLAY_MUSIC = "PLAY_MUSIC";
        public const string STOP_MUSIC = "STOP_MUSIC";
        public const string STOP_MUSIC_NOW = "STOP_MUSIC_NOW";
        public const string PLAY_AMBIENT = "PLAY_AMBIENT";
        public const string STOP_AMBIENT = "STOP_AMBIENT";
        public const string STOP_AMBIENT_NOW = "STOP_AMBIENT_NOW";
        public const string PLAY = "PLAY_ONESHOTSFX"; //<- One shots don't need a stop command. They just play until they're finished, then disappear. 
    }
}
