namespace Ambition
{
    public static class AudioMessages
    {
        public const string PLAY_MUSIC = "PLAY_MUSIC";
        public const string STOP_MUSIC = "STOP_MUSIC";
        public const string STOP_MUSIC_NOW = "STOP_MUSIC_NOW";
        public const string QUEUE_MUSIC = "QUEUE_MUSIC";
        public const string PLAY_AMBIENTSFX = "PLAY_AMBIENT";
        public const string STOP_AMBIENTSFX = "STOP_AMBIENT";
        public const string STOP_AMBIENTSFX_NOW = "STOP_AMBIENT_NOW";
        public const string PLAY_ONESHOTSFX = "PLAY_ONESHOTSFX"; //<- One shots don't need a stop command. They just play until they're finished, then disappear. 
    }
}
