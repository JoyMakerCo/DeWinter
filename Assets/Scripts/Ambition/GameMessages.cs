﻿using System;

namespace Ambition
{
	public static class GameMessages
	{
        public static string LOAD_SCENE = "GotoSceneMsg";
		public static string SCENE_LOADED = "SCENE_LOADED";
        public static string EXIT_SCENE = "EXIT_SCENE";
        public static string FADE_OUT = "FADE_OUT";
		public static string FADE_IN = "FADE_IN";
		public static string FADE_OUT_COMPLETE = "FadeOutCompleteMsg";
		public static string FADE_IN_COMPLETE = "FadeInCompleteMsg";
        public static string INTERRUPT_FADE = "INTERRUPT_FADE";
        public static string QUIT_GAME = "QUIT_GAME";
        public static string SAVE_GAME = "SAVE_GAME";
        public static string RESTORE_GAME = "RESTORE_GAME";
        public static string NEW_GAME = "NEW_GAME";
        public static string EXIT_MENU = "EXIT_MENU";
        public static string START_GAME = "NEW_GAME";
        public static string END_GAME = "END_GAME";
        public static string START_TUTORIAL = "START_TUTORIAL";
		public static string SKIP_TUTORIAL = "SKIP_TUTORIAL";
		public static string DIALOG_OPENED = "DIALOG_OPENED";
		public static string DIALOG_CLOSED = "DIALOG_CLOSED";
		public static string DIALOG_CONFIRM = "DIALOG_CONFIRM";

        public static string SHOW_HEADER = "SHOW_HEADER";
        public static string HIDE_HEADER = "HIDE_HEADER";
        public static string LOCK_UI = "LOCK_UI";
        public static string UNLOCK_UI = "UNLOCK_UI";

        public static string SET_TITLE = "SET_TITLE";
        public static string COMPLETE = "COMPLETE"; // All-Purpose "Complete" event

        public static string INIT_CHARACTER = "INIT_CHARACTER";
        public static string TIMER_COMPLETE = "TIMER_COMPLETE";
        public static string INHIBIT_MENU = "INHIBIT_MENU"; // Disable the game menu until the next fade in
    }
}
