using System;
using System.Collections.Generic;
using Core;
using Newtonsoft.Json;

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

		[JsonProperty("luxury")]
		public Dictionary<int, string> Luxury;

		[JsonProperty("modesty")]
		public Dictionary<int, string> Modesty;

		public string NextStyle;

// TODO: These definitely need to be in the view, not the model
public ItemVO SelectedItem;
public ItemVO SelectedMarketItem;

// TODO: Use ItemVO to represent gossip
		public List<Gossip> GossipItems = new List<Gossip>();

		public Dictionary<string, ItemVO> Equipped = new Dictionary<string, ItemVO>();
		public Dictionary<string, ItemVO> LastEquipped = new Dictionary<string, ItemVO>();

		// Inventories stored by ItemVO.Type (eg, Accessory, Gossip, Outfit)
		public List<ItemVO> Inventory = new List<ItemVO>();

		[JsonProperty("items")]
		public ItemVO[] ItemDefinitions;

		public List<ItemVO> Market=new List<ItemVO>();
	}
}