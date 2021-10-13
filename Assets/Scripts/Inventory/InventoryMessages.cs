using System;

namespace Ambition
{
	public static class InventoryMessages
	{
		public const string SELL_ITEM = "SELL_ITEM";
		public const string BUY_ITEM = "BUY_ITEM";
		public const string DELETE_ITEM = "DELETE_ITEM";
        public const string SORT_INVENTORY = "SORT_INVENTORY"; //Used in conjunction with a string to sort whatever inventories are on screen at the moment
        public const string CREATE_GOSSIP = "CREATE_GOSSIP";
        public const string DISPLAY_GOSSIP = "DISPLAY_GOSSIP"; //Gossip items aren't part of the Item VO, and have some truly unique behaviors, so it feels like they need their own message set
        public const string SELL_GOSSIP = "SELL_GOSSIP";
        public const string PEDDLE_INFLUENCE = "PEDDLE_GOSSIP";
        public const string UPDATE_MERCHANT = "UPDATE_MERCHANT";
        public const string EQUIP = "EQUIP";
        public const string BROWSE = "BROWSE";
        public const string UNEQUIP = "UNEQUIP";
        public const string ITEM_DELETED = "ITEM_DELETED";
    }
}
