using System;
using System.Linq;
using System.Collections.Generic;
using Core;
using UnityEngine;
using Newtonsoft.Json;

namespace Ambition
{
    [Saveable]
	public class InventoryModel : DocumentModel
	{
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

		[JsonProperty("num_outfits")]
		public int NumOutfits;

		[JsonProperty("max_outfits")]
		public int MaxOutfits;

        [JsonProperty("max_accessories")]
        public int MaxAccessories;

        [JsonProperty("marketCapacity")]
		public int NumMarketSlots;

        [JsonProperty("style")]
        private string _style
        {
            set => Style.Value = value;
        }
        public Observable<string> Style;

		[JsonProperty("luxury")]
		public Dictionary<int, string> Luxury;

		[JsonProperty("modesty")]
		public Dictionary<int, string> Modesty;

        [JsonProperty("items")]
        public ItemVO[] Items; // Base Item Definitions, used for instantiation.

        [JsonProperty("next_style")]
        public string NextStyle;

        [JsonIgnore]
        // Items owned by the player. Contains customized instances from the Items list.
        public Dictionary<ItemType, List<ItemVO>> Inventory = new Dictionary<ItemType, List<ItemVO>>();

        [JsonIgnore]
        // Items currently equipped. Equipped items and Inventory are mutually exclusive.
        public Dictionary<ItemType, int> Equipped = new Dictionary<ItemType, int>();

        [JsonProperty("market")]
        public Dictionary<ItemType, List<ItemVO>> Market; // Created On-Demand and persists day-to-day.

        // List of item instances based on item list and customized in the game context.
        [JsonProperty("inventory")]
		private List<ItemVO> _inventory
		{
			set
			{
                Inventory.Clear();
                Equipped.Clear();
                foreach (ItemVO item in value)
                {
                    Add(item);
                    if (item.Equipped) Equip(item);
                }
            }
            get
            {
                List<ItemVO> result = new List<ItemVO>();
                foreach(List<ItemVO> items in Inventory.Values)
                {
                    result.AddRange(items);
                }
                return result;
            }
        }

        public void Add(ItemVO item)
        {
            if (!Inventory.TryGetValue(item.Type, out List<ItemVO> items))
            {
                Inventory[item.Type] = items = new List<ItemVO>() { item };
            }
            else if (!items.Contains(item))
            {
                items.Add(item);
            }
            Broadcast();
        }

        public bool Remove(ItemVO item)
        {
            if (item == null || !Inventory.TryGetValue(item.Type, out List<ItemVO> items))
                return false;

            int index = items.IndexOf(item);
            if (index < 0) return false;

            if (Equipped.TryGetValue(item.Type, out int equipped) && equipped > index)
            {
                Equipped[item.Type]--;
            }
            else if (equipped == index) Equipped.Remove(item.Type);
            Broadcast();
            return true;
        }

        public void Equip(ItemVO item)
        {
            Add(item);
            HelpUnequip(item.Type);
            Equipped[item.Type] = GetItemIndex(item);
            item.Equipped = true;
            Broadcast();
        }

        public bool Unequip(ItemVO item)
        {
            if (GetEquippedItem(item.Type) != item) return false;
            Equipped.Remove(item.Type);
            item.Equipped = false;
            Broadcast();
            return true;
        }

        private bool HelpUnequip(ItemType type) // Unequip helper to reduce redundant Broadcast calls
        {
            ItemVO item = GetEquippedItem(type);
            if (item == null) return false;
            item.Equipped = false;
            return Equipped.Remove(type);
        }

        public bool Unequip(ItemType type)
        {
            if (!HelpUnequip(type)) return false;
            Broadcast();
            return true;
        }

        public ItemVO GetEquippedItem(ItemType slot)
        {
            if (!Equipped.TryGetValue(slot, out int index)) return null;
            if (!Inventory.TryGetValue(slot, out List<ItemVO> items)) return null;
            return index < items.Count ? items[index] : null;
        }

        public int GetCredibilityBonus(ItemVO outfit, FactionType faction)
        {
            if (outfit == null) return 0;
            FactionVO fvo = AmbitionApp.GetModel<FactionModel>()[faction];
            if (fvo == null) return 0;

            if (outfit.State == null) outfit.State = new Dictionary<string, string>();
            int modesty = OutfitWrapperVO.GetModesty(outfit);
            int luxury = OutfitWrapperVO.GetLuxury(outfit);
            int novelty = OutfitWrapperVO.GetNovelty(outfit);
            int sum = Math.Abs(fvo.Modesty - modesty) + Math.Abs(fvo.Luxury - luxury);

            // ExhaustionPenalty magically handles the well-rested bonus
            return (int)(sum * novelty * 0.002f) + AmbitionApp.GetModel<GameModel>().ExhaustionPenalty;
        }

        private int GetItemIndex(ItemVO item)
        {
            return Inventory.TryGetValue(item.Type, out List<ItemVO> items)
                ? items.IndexOf(item)
                : -1;
        }

        private void BroadcastStyle(string style) => AmbitionApp.SendMessage<string>(ItemConsts.STYLE, Style.Value);
        protected override void OnLoadComplete()
        {
            Style.Observe(BroadcastStyle);
        }
    }
}
