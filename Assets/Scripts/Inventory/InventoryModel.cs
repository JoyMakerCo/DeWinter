using System;
using System.Collections.Generic;
using Core;

namespace DeWinter
{
	public class InventoryModel : DocumentModel
	{
		public InventoryModel() : base ("InventoryData") {}

		[JsonProperty("noveltyDamage")]
		public int NoveltyDamage;

		[JsonProperty("outOfStyleMultiplier")]
		public float OutOfStyleMultiplier;

		[JsonProperty("personalInventoryMultiplier")]
		public float PersonalInventoryMultiplier;

		[JsonProperty("styles")]
		public string [] Styles;

		[JsonProperty("accessories")]
		public AccessoryVO [] Accessories;

		public List<Gossip> GossipItems = new List<Gossip>();

		public Dictionary<string, ItemVO> Equipped;

		public Dictionary<string, List<ItemVO>> Inventory;

// TODO: Reflect this in the Equipped dictionary.
		//Outfit Stuff. The Values start at -1 because I can't use null and I need an ID number that will never appear in the list.
	    public int partyOutfitID;
	    public int partyAccessoryID;
	    //Used for seeing if the same Outfit was used twice in a row
	    public int lastPartyOutfitID;
	    public bool woreSameOutfitTwice;

// TODO: Commandify style switching
		public string currentStyle;
	    public string nextStyle;
	}
}