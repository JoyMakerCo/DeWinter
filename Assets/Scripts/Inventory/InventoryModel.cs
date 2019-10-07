using System;
using System.Linq;
using System.Collections.Generic;
using Core;
using UnityEngine;
using Newtonsoft.Json;

namespace Ambition
{
    [Saveable]
	public class InventoryModel : DocumentModel, IConsoleEntity
	{
		public InventoryModel() : base ("InventoryData") {}

		[JsonProperty("noveltyDamage")]
		public int NoveltyDamage;

		[JsonProperty("outOfStyleMultiplier")]
		public float OutOfStyleMultiplier;

		[JsonProperty("sellbackMultiplier")]
		public float SellbackMultiplier;

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

		[JsonProperty("luxury")]
		public Dictionary<int, string> Luxury;

		[JsonProperty("modesty")]
		public Dictionary<int, string> Modesty;

        [JsonProperty("items")]
        public ItemVO[] Items; // Base Item Definitions, used for instantiation.

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

        public string[] Dump()
        {
            var lines = new List<string>
            {
                "InventoryModel:",
                "Novelty Damage: " + NoveltyDamage.ToString(),
                "Out of Style Mult: " + OutOfStyleMultiplier.ToString("0.00"),
                "Sellback Mult: " + SellbackMultiplier.ToString("0.00"),
                string.Format( "Slots: {0}/{1}", NumSlots, MaxSlots ),
                string.Format( "Outfits: {0}/{1}", NumOutfits, MaxOutfits ),
                "Max Accessories: " + MaxAccessories.ToString(),
                "Market Slots: " + NumMarketSlots.ToString(),
            };

			// base items
            if (Items == null)
            {
                lines.Add( "Base Items (null) ");
            }
            else
            {
                lines.Add( "Base Items: ");

                foreach (var ivo in Items)
                {
                    lines.Add( "  " +ivo.ToString() );
                }
            }

			// player inventory
            if (Inventory == null)
            {
                lines.Add( "Inventory Items (null) ");
			}
			else
			{
				lines.Add( "Inventory Items: ");

				foreach (var kv in Inventory)
				{
					lines.Add( string.Format( "[{0}]", kv.Key ) );
					for ( int i = 0; i < kv.Value.Count; i++)
					{
						string equipt = "  ";
						if (Equipped != null)
						{
							if (Equipped[kv.Key] == i)
							{
								equipt = " >";
							}
						}
						lines.Add( equipt + kv.Value[i].ToString() );
					}
				}
			}

			// market inventory
            if (Market == null)
            {
                lines.Add( "Market Items (null) ");
			}
			else
			{
				lines.Add( "Market Items: ");

				foreach (var kv in Market)
				{
					lines.Add( string.Format( "[{0}]", kv.Key ) );
					for ( int i = 0; i < kv.Value.Count; i++)
					{
						lines.Add( "  " + kv.Value[i].ToString() );
					}
				}
			}

            return lines.ToArray();
        }

        public void Invoke( string[] args )
        {
            ConsoleModel.warn("InventoryModel has no invocation.");
        }
    }
}
