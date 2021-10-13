using System;

namespace Ambition
{
	public static class DialogConsts
	{
		public const string TITLE = ".title";
		public const string BODY = ".body";
		public const string OK = ".ok";
		public const string CANCEL = ".cancel";
        public const string CONFIRM = ".confirm";
		public const string DEFAULT_CONFIRM = "btn_confirm";
		public const string DEFAULT_CANCEL = "btn_cancel";
        public const string DEFAULT_CLOSE = "btn_close";

        // GENERIC MESSAGE DIALOG
        public const string MESSAGE = "MESSAGE";

        // LOADING 
        public const string EXIT_GAME = "EXIT_GAME";
		public const string GAME_MENU = "GAME_MENU";
		public const string GAME_OPTIONS = "GAME_OPTIONS";
        public const string LOAD_GAME = "LOAD_GAME";
		public const string PLAYER_SELECT = "PLAYER_SELECT";
		public const string SAVE_GAME = "SAVE_GAME";
		public const string SAVE_NEW = "SAVE_NEW";

		// MODALS & DIALOGS
		public const string RSVP = "RSVP";
		public const string RSVP_CHOICE = "RSVP_CHOICE";
        public const string RENDEZVOUS_RSVP = "RENDEZVOUS_RSVP";
        public const string CHOOSE_PARTY = "CHOOSE_PARTY";
		public const string CANCEL_PARTY = "CANCEL_PARTY";
		public const string NO_OUTFIT = "NO_OUTFIT";
		public const string MAKE_OUTFIT = "MAKE_OUTFIT";
		public const string INCIDENT = "INCIDENT";
		public const string ALTER_OUTFIT = "ALTER_OUTFIT";
		public const string BUY_OR_SELL = "BUY_OR_SELL";
		public const string CANT_AFFORD = "CANT_AFFORD";
		public const string TRADE_GOSSIP = "TRADE_GOSSIP";
        public const string PEDDLE_INFLUENCE = "PEDDLE_INFLUENCE";
        public const string QUEST = "QUEST";
		public const string ERROR = "ERROR";

		// PARIS MODAL
		public const string PARIS_LOCATION = "PARIS_LOCATION";

        // PARTY MODALS & DIALOGS
        public const string AMBUSH = "AMBUSH";
		public const string HOST = "HOST";
		public const string CHOOSE_ROOM = "CHOOSE_ROOM";
		public const string HOST_ENCOUNTER = "HOST_ENCOUNTER";
		public const string ROOM = "ROOM";
		public const string ROOM_TUTORIAL = "ROOM_TUTORIAL";

		public const string PAY_DAY_DIALOG = "pay_day_dialog";
		public const string FIRE_CAMILLE_DIALOG = "fire_camille_dialog";
		public const string CANT_BUY_DIALOG = "cant_buy_dialog";
		public const string CANT_MAKE_DIALOG = "cant_make_dialog";
		public const string REPUTATION_WINE_DIALOG = "reputation_wine_dialog";
		public const string MOVED_THROUGH_DIALOG = "moved_through_dialog";
		public const string CONVERSATION_OVER_DIALOG = "conversation_over_dialog";
		public const string CHARMED_HOST_DIALOG = "charmed_host_dialog";
		public const string FAILED_HOST_DIALOG = "failed_host_dialog";
		public const string MISSED_RSVP_DIALOG = "missed_party_rsvp_dialog";
        public const string MISSED_RENDEZVOUS_DIALOG = "missed_rendezvous_rsvp_dialog";
        public const string CAUGHT_GOSSIPING_DIALOG = "caught_gossiping_dialog";
		public const string CAUGHT_GOSSIPING_THIRD_ESTATE_DIALOG = "caught_gossiping_third_estate_dialog";
		public const string REDEEM_QUEST_DIALOG = "redeem_quest_dialog";
		public const string CREATE_OUTFIT_DIALOG = "create_outfit_dialog";
		public const string ALTER_OUTFIT_DIALOG = "alter_outfit_dialog";

		public const string CANCEL_RSVP_DIALOG = "rsvp_cancel_dialog";//CancellationModal";
		public const string RSVP_DIALOG = "rsvp_dialog";//RSVPPopUpModal";
		public const string RSVP_CHOICE_DIALOG = "rsvp_choice_dialog";//TwoPartyRSVPdPopUpModal";

        public const string CREATE_RENDEZVOUS = "CREATE_RENDEZVOUS";
        public const string SHOW_INVITATION = "SHOW_INVITATION";
    }
}