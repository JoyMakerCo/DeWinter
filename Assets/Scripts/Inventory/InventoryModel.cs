using System;
using System.Collections.Generic;
using Core;

namespace DeWinter
{
	public class InventoryModel : DocumentModel
	{
		private int _nextStyleSwitch;

		public InventoryModel() : base ("InventoryData") {}

		[JsonProperty("noveltyDamage")]
		public int NoveltyDamage;

		[JsonProperty("outOfStyleMultiplier")]
		public float OutOfStyleMultiplier;

		[JsonProperty("sellbackMultiplier")]
		public float SellbackMultiplier;

		[JsonProperty("styles")]
		public string [] Styles;

		[JsonProperty("slots")]
		public int NumSlots;

		[JsonProperty("maxSlots")]
		public int MaxSlots;

		[JsonProperty("marketCapacity")]
		public int NumMarketSlots;

		[JsonProperty("startingStyle")]
		public string CurrentStyle;

		public string NextStyle;

// TODO: These definitely need to be in the view, not the model
public ItemVO SelectedItem;
public ItemVO SelectedMarketItem;

// TODO: Use ItemVO to represent gossip
		public List<Gossip> GossipItems = new List<Gossip>();

		public Dictionary<string, ItemVO> Equipped = new Dictionary<string, ItemVO>();

		public Dictionary<string, List<ItemVO>> Inventory = new Dictionary<string, List<ItemVO>>();

		public List<ItemVO> Market=new List<ItemVO>();

// TODO: Reflect this in the Equipped dictionary.
		//Outfit Stuff. The Values start at -1 because I can't use null and I need an ID number that will never appear in the list.
	    public int partyOutfitID=-1;
	    public int partyAccessoryID=-1;

	    //Used for seeing if the same Outfit was used twice in a row
	    public int lastPartyOutfitID=-1;
	    public bool woreSameOutfitTwice=false;
	}
}