using System;

namespace Ambition
{
	public static class GameMessages
	{
        public static string LOAD_SCENE = "GotoSceneMsg";
		public static string SCENE_LOADED = "SCENE_LOADED";
        public static string FADE_OUT = "FADE_OUT";
		public static string FADE_IN = "FADE_IN";
		public static string FADE_OUT_COMPLETE = "FadeOutCompleteMsg";
		public static string FADE_IN_COMPLETE = "FadeInCompleteMsg";
        public static string INTERRUPT_FADE = "INTERRUPT_FADE";
        public static string QUIT_GAME = "QUIT_GAME";
        public static string NEW_GAME = "NEW_GAME";
        public static string EXIT_GAME = "EXIT_GAME";
        public static string START_GAME = "NEW_GAME";
        public static string LOAD_GAME = "RESTORE_GAME";
        public static string GAME_LOADED = "GAME_LOADED";
        public static string SAVE_GAME = "SAVE_GAME";
        public static string AUTOSAVE = "AUTOSAVE";
        public static string CONTINUE = "CONTINUE";
        public static string START_TUTORIAL = "START_TUTORIAL";
		public static string SKIP_TUTORIAL = "SKIP_TUTORIAL";
		public static string DIALOG_OPENED = "DIALOG_OPENED";
		public static string DIALOG_CLOSED = "DIALOG_CLOSED";
		public static string DIALOG_CONFIRM = "DIALOG_CONFIRM";
        public static string LOAD = "LOAD";

        public static string SHOW_HEADER = "SHOW_HEADER";
        public static string HIDE_HEADER = "HIDE_HEADER";
        public static string LOCK_UI = "LOCK_UI";
        public static string UNLOCK_UI = "UNLOCK_UI";

        public static string NEXT = "NEXT"; // All-Purpose "Next" event
        public static string COMPLETE = "COMPLETE"; // All-Purpose "Complete" event

        public static string TIMER_COMPLETE = "TIMER_COMPLETE";

        public static string TOGGLE_CONSOLE = "TOGGLE_CONSOLE";
        public const string UPDATE_SETTINGS = "UPDATE_SETTINGS";
        public const string UPDATE_LOCALIZATION = "UPDATE_LOCALIZATION";

        public const string ADD_EXHAUSTION = "ADD_EXHAUSTION";
        public const string EXHAUSTION_EFFECT = "EXHAUSTION_EFFECT";
        public const string OUTFIT_EFFECT = "OUTFIT_EFFECT";
        public const string EPILOGUE = "EPILOGUE";
        public const string OUT_OF_LIVRE = "OUT_OF_LIVRE";

        // WebRequest
        public const string REQUEST = "REQUEST";
        public const string RESPONSE = "RESPONSE";
        public const string ERROR = "ERROR";
    }
}
