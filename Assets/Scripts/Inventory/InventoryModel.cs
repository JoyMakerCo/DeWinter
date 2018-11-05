using System;
using System.Collections.Generic;
using Core;
using Newtonsoft.Json;

namespace Ambition
{
	public class InventoryModel : DocumentModel
	{
		private int _nextStyleSwitch;
		private string _currentStyle;

		public InventoryModel() : base ("InventoryData") {}

		[JsonProperty("noveltyDamage")]
		public int NoveltyDamage;

		[JsonProperty("outOfStyleMultiplier")]
		public float OutOfStyleMultiplier;

		[JsonProperty("sellbackMultiplier")]
		public float SellbackMultiplier;

		private string[] _styles;

		[JsonProperty("styles")]
		public string [] Styles;

		[JsonProperty("slots")]
		public int NumSlots;

		[JsonProperty("maxSlots")]
		public int MaxSlots;

		[JsonProperty("num_outfits")]
		public int NumOutfits;

		[JsonProperty("max_outfits")]
		public int MaxOutfits;

        [JsonProperty("max_accessories")]
        public int MaxAccessories;

        [JsonProperty("marketCapacity")]
		public int NumMarketSlots;

		[JsonProperty("style")]
		public string CurrentStyle
		{
			set {
				_currentStyle = value;
				AmbitionApp.SendMessage<string>(ItemConsts.STYLE, _currentStyle);
			}
			get { return _currentStyle; }
		}

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
        public int GossipSoldOrPeddled; //This is used in the mornings to determine if the player was caught selling gossip

        public Dictionary<string, ItemVO> Equipped = new Dictionary<string, ItemVO>();

		public List<ItemVO> Inventory = new List<ItemVO>();

        public ItemVO GetEquipped(string type)
        {
            ItemVO result;
            return Equipped.TryGetValue(type, out result) ? result : null;
        }

		// Inventories stored by ItemVO.Type (eg, Accessory, Gossip, Outfit)
		[JsonProperty("inventory", Order=1)]
		private Dictionary<string, int> _inventory
		{
			set
			{
				ItemVO item;
				foreach(KeyValuePair<string, int> kvp in value)
				{
					item = Array.Find(ItemDefinitions, i=>i.ID == kvp.Key);
					if (item != null)
					{
						item = new ItemVO(item);
						Inventory.Add(item);
					}
					item.Quantity = kvp.Value;
				}
			}
		}

		[JsonProperty("items", Order=0)]
		public ItemVO[] ItemDefinitions;

		public List<ItemVO> Market=new List<ItemVO>();
	}
}
