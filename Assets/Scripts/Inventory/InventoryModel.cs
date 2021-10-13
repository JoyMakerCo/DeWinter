using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Core;
using UnityEngine;
using Newtonsoft.Json;

namespace Ambition
{
    [Saveable]
	public class InventoryModel : ObservableModel<InventoryModel>, IResettable
	{
        // PUBLIC DATA //////////////

        [JsonIgnore]
        public float OutOfStyleMultiplier = 0.75f;

        [JsonIgnore]
        public float SellbackMultiplier = 0.5f;

        [JsonProperty("marketCapacity")]
        public int NumMarketSlots = 5;

        [JsonProperty("inventory")]
        // Items owned by the player. Contains customized instances from the Items list.
        public List<ItemVO> Inventory = new List<ItemVO>();

        [JsonProperty("market")]
        public List<ItemVO> Market = null; // Created On-Demand and persists throughout the day.

        [JsonIgnore]
        private Dictionary<string, Sprite> _assets = new Dictionary<string, Sprite>();

        [JsonIgnore]
        public int HumbleLimit;

        [JsonIgnore]
        public int RisqueLimit;

        [JsonIgnore]
        public int ModestLimit;

        [JsonIgnore]
        public int LuxuryLimit;

        [JsonIgnore]
        public readonly Dictionary<string, ItemVO> Items = new Dictionary<string, ItemVO>();

        // PRIVATE/PROTECTED DATA //////////////

        public ItemVO Instantiate(string itemID) => Items.TryGetValue(itemID, out ItemVO item)
           ? Instantiate(item)
           : null;

        public void Import(ItemConfig config)
        {
            if (config != null)
            {
                ItemVO item = new ItemVO()
                {
                    ID = config.name,
                    Type = config.Type,
                    Price = config.Price,
                    State = new Dictionary<string, string>(),
                    AssetID = config.Asset == null ? null : config.Asset.name,
                    Permanent = config.Permanent,
                    Market = config.Market
                };
                if (config.State != null)
                {
                    Array.ForEach(config.State, s => item.State[s.Key] = s.Value);
                }
                Items[config.name] = item;
                if (!string.IsNullOrEmpty(item.AssetID))
                {
                    _assets[item.AssetID] = config.Asset;
                }
            }
        }

        public bool Remove(ItemVO item) => Inventory.Remove(item);

        public ItemVO[] GetItems(ItemType type)
        {
            List<ItemVO> result = new List<ItemVO>();
            foreach (ItemVO item in Inventory)
                if (item.Type == type)
                    result.Add(item);
            return result.ToArray();
        }

        public void Reset()
        {
            Inventory.Clear();
            Market?.Clear();
            Market = null;
        }

        // PRIVATE / PROTECTED METHODS ///////////////

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            ItemVO item;
            ItemConfig config;
            for (int i=Inventory.Count-1; i>=0; --i)
            {
                item = Inventory[i];
                if (item != null && !Items.ContainsKey(item.ID))
                {
                    config = Resources.Load<ItemConfig>(Filepath.Items + Inventory[i].ID);
                    Import(config);
                }
                Inventory[i] = Copy(item);
            }
            if (Market != null)
            {
                for(int i=Market.Count-1; i>=0; --i)
                {
                    Market[i] = Instantiate(Market[i]);
                }
            }
        }

        private ItemVO Instantiate(ItemVO item)
        {
            if (item == null) return null;
            switch (item.Type) // Add to this if new item types are introduced
            {
                case ItemType.Outfit:
                    return new OutfitVO(item);
            }
            return new ItemVO(item);
        }

        private ItemVO Copy(ItemVO item)
        {
            if (item == null) return null;
            ItemVO result = Instantiate(item);
            result.Price = item.Price;
            result.State = new Dictionary<string, string>(item.State);
            result.Created = item.Created;
            return result;
        }

        public Sprite GetAsset(ItemVO item)
        {
            if (string.IsNullOrEmpty(item?.AssetID)) return null;
            _assets.TryGetValue(item.AssetID, out Sprite asset);
            return asset;
        }

        public ItemVO GetEquippedItem(ItemType slot) => Inventory.Find(i => i.Equipped && i.Type == slot);

        public int GetFactionBonus(ItemVO item, FactionType faction)
        {
            OutfitVO outfit = item as OutfitVO;
            if (outfit == null) return 0;
            FactionVO fvo = AmbitionApp.GetModel<FactionModel>()[faction];
            
            if (fvo == null) return 0;

            if (outfit.State == null) outfit.State = new Dictionary<string, string>();
            int modesty = outfit.Modesty;
            int luxury = outfit.Luxury;
            int novelty = outfit.Novelty;

            // -200 to +200
            int sum = 200 - (Math.Abs(fvo.Modesty - modesty) + Math.Abs(fvo.Luxury - luxury));

            // max range +/- 20 cred for outfit
            const int credibility_shift_scale = 20;     // TODO expose this tuning variable
            return (int)(sum * novelty * credibility_shift_scale * 0.00005f);
        }

        public override string ToString()
        {
            string lines = "InventoryModel:" +
                "\n Out of Style Mult: " + OutOfStyleMultiplier.ToString("0.00") +
                "\n Sellback Mult: " + SellbackMultiplier.ToString("0.00") +
                "\n Market Slots: " + NumMarketSlots.ToString();

            // base items
            if (Items == null)
            {
                lines += "\n Base Items (null) ";
            }
            else
            {
                lines += "\n Base Items: ";

                foreach (var ivo in Items)
                {
                    lines += "\n  " + ivo.ToString();
                }
            }

            // player inventory
            if (Inventory == null)
            {
                lines += "\n  Inventory Items (null) ";
            }
            else
            {
                lines += "\n  Inventory Items: ";

                foreach (ItemVO item in Inventory)
                {
                    lines += "\n   " + string.Format("[{0}]", item.ToString());
                }
            }

            // market inventory
            if (Market != null)
            {
                lines += "\n  Market Items: ";

                foreach (ItemVO item in Market)
                {
                    lines += "\n   " + string.Format("[{0}]", item.ToString());
                }
            }
            else lines += "\n  Market Items (null) ";

            return lines;
        }
    }
}
