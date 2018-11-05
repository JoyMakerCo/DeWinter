using System;

namespace Ambition
{
	public static class InventoryMessages
	{
		public const string SELL_ITEM = "SELL_ITEM";
		public const string BUY_ITEM = "BUY_ITEM";
        public const string DISPLAY_ITEM = "DISPLAY_ITEM"; // This is used in the store, wardrobe and party loadout screens to show whatever is being chosen
		public const string DEGRADE_OUTFIT = "DEGRADE_OUTFIT";
		public const string DELETE_ITEM = "DELETE_ITEM";
		public const string INVENTORY = "INVENTORY";
        public const string SORT_INVENTORY = "SORT_INVENTORY"; //Used in conjunction with a string to sort whatever inventories are on screen at the moment
        public const string DISPLAY_GOSSIP = "DISPLAY_GOSSIP"; //Gossip items aren't part of the Item VO, and have some truly unique behaviors, so it feels like they need their own message set
        public const string SELL_GOSSIP = "SELL_GOSSIP";
        public const string PEDDLE_GOSSIP = "PEDDLE_GOSSIP";

		public const string EQUIP = "EQUIP";
		public const string UNEQUIP = "UNEQUIP";
		public const string UNEQUIPPED = "UNEQUIPPED";
		public const string EQUIPPED = "EQUIPPED";
        public const string ITEM_DELETED = "ITEM_DELETED";
    }
}
