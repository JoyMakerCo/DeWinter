using System;
using System.Collections.Generic;
using Core;
using UnityEngine;
using Newtonsoft.Json;

namespace Ambition
{
    [Saveable]
	public class InventoryModel : DocumentModel, IConsoleEntity, IResettable
	{
        // CONSTRUCTORS //////////////

		public InventoryModel() : base ("InventoryData") 
        {
            Gossip = Resources.Load("Gossip Config") as GossipConfig;
            if (Gossip == null)
            {
                Debug.LogError("Missing Gossip Config file, using defaults");
                Gossip = ScriptableObject.CreateInstance<GossipConfig>();
            }

            ItemConfig[] configs = Resources.LoadAll<ItemConfig>("Items");
            Items = new ItemVO[configs.Length];
            for (int i=configs.Length-1; i>=0; --i)
            {
                Items[i] = Import(configs[i]);
            }
        }

        // PUBLIC DATA //////////////

        public GossipConfig Gossip;

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

        [JsonIgnore]
        public ItemVO[] Items; // Library of item definitions

        [JsonProperty("gossip_activity")]
        public int GossipActivity;

        [JsonProperty("inventory")]
        // Items owned by the player. Contains customized instances from the Items list.
        public List<ItemVO> Inventory = new List<ItemVO>();

        [JsonProperty("equipped")]
        public Dictionary<ItemType, ItemVO> Equipped = new Dictionary<ItemType, ItemVO>();

        [JsonProperty("market")]
        public List<ItemVO> Market = null; // Created On-Demand and persists throughout the day.

        public ItemVO Import(ItemConfig config)
        {
            ItemVO item = new ItemVO()
            {
                Name = config.name,
                Type = config.Type,
                Price = config.Price,
                State = new Dictionary<string, string>(),
                AssetID = config.Asset ? config.Asset.name : null,
                Config = config.name
            };
            if (!string.IsNullOrEmpty(item.AssetID) && !_assets.ContainsKey(item.AssetID))
            {
                _assets.Add(item.AssetID, config.Asset);
            }
            if (config.State != null)
            {
                Array.ForEach(config.State, s => item.State[s.Key] = s.Value);
            }
            return item;
        }

        public bool Remove(ItemVO item)
        {
            if (Inventory.Remove(item)) return true;
            if (!Equipped.TryGetValue(item.Type, out ItemVO equipped)) return false;
            if (equipped != item) return false;
            Equipped.Remove(item.Type);
            return true;
        }

        public ItemVO[] GetItems(ItemType type, bool equipped = false)
        {
            if (equipped)
            {
                return Equipped.ContainsKey(type)
                    ? new ItemVO[] { Equipped[type] }
                    : new ItemVO[0];
            }
            else
            {
               return Inventory.FindAll(i => i.Type == type).ToArray();
            }
        }

        public void Reset()
        {
            Equipped.Clear();
            Inventory.Clear();
        }

        // PRIVATE / PROTECTED METHODS ///////////////

        [JsonIgnore]
        private Dictionary<string, Sprite> _assets = new Dictionary<string, Sprite>();

        public void GossipShared(ItemVO gossip)
        {
            GossipActivity++;
            Debug.Log("Gossip activity increased");

            // does it match Pierre's quest?

            var _quest = AmbitionApp.GetModel<QuestModel>();
            if ((_quest.CurrentQuest != null) && (_quest.ActiveQuestState == QuestModel.QuestActive))
            {
                if (GossipWrapperVO.GetFaction(gossip) == _quest.CurrentQuest.Faction)
                {
                    Debug.Log("Gossip quest fulfilled");

                    // pop the dialog
                    AmbitionApp.OpenDialog("REDEEM_QUEST_DIALOG");
                }
            }
        }

        public bool CheckCaughtTrading()
        {
            // These counts are expressed as the activity score 
            // -after- the sale, whereas the table in the spec is 
            // expressed as the score -before- the sale, so it 
            // appears to be off by one.
            var result = false; 

            switch (GossipActivity)
            {
                case 0:
                case 1:     result = false;                         break;
                case 2:     result = Util.RNG.Chance(0.05f);        break;
                case 3:     result = Util.RNG.Chance(0.10f);        break;
                case 4:     result = Util.RNG.Chance(0.25f);        break;
                case 5:     result = Util.RNG.Chance(0.33f);        break;
                case 6:     result = Util.RNG.Chance(0.50f);        break;
                case 7:     result = Util.RNG.Chance(0.67f);        break;
                case 8:     result = Util.RNG.Chance(0.75f);        break;
                case 9:     result = Util.RNG.Chance(0.80f);        break;
                case 10:     
                default:    result = Util.RNG.Chance(0.95f);        break;
            }

            // Reset the activity count
            GossipActivity = 0;


            Debug.LogFormat( "checking gossip trading activity - {0}", result ? "incident will trigger" : "incident will not trigger");
            return result;
        }

        public ItemVO GetEquippedItem(ItemType slot)
        {
            Equipped.TryGetValue(slot, out ItemVO item);
            if (item != null) item.Equipped = true;
            return item;
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

            // -200 to +200
            int sum = 200 - (Math.Abs(fvo.Modesty - modesty) + Math.Abs(fvo.Luxury - luxury));

            // max range +/- 20 cred for outfit
            const int credibility_shift_scale = 20;     // TODO expose this tuning variable
            return (int)(sum * novelty * credibility_shift_scale * 0.00005f);
        }

        // rename or move this...
        public int GetCredibilityShift(ItemVO outfit, FactionType faction)
        {
            return GetCredibilityBonus(outfit,faction) + AmbitionApp.GetModel<GameModel>().ExhaustionPenalty;
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
                "Gossip Activity: " + GossipActivity.ToString(),
            };

            // base items
            if (Items == null)
            {
                lines.Add("Base Items (null) ");
            }
            else
            {
                lines.Add("Base Items: ");

                foreach (var ivo in Items)
                {
                    lines.Add("  " + ivo.ToString());
                }
            }

            // player inventory
            if (Inventory == null)
            {
                lines.Add("Inventory Items (null) ");
            }
            else
            {
                lines.Add("Inventory Items: ");

                foreach (ItemVO item in Inventory)
                {
                    lines.Add(string.Format("[{0}]", item));
                }
            }
            if (Equipped != null)
            {
                lines.Add("Equipped Items: ");

                foreach (ItemVO item in Equipped.Values)
                {
                    lines.Add(string.Format("[{0} > {1}]", item.Type, item.ID));
                }
            }
            else lines.Add("Equipped Items  (null)");


            // market inventory
            if (Market != null)
            {
                lines.Add("Market Items: ");

                foreach (ItemVO item in Market)
                {
                    lines.Add(string.Format("[{0}]", item));
                }
            }
            else lines.Add("Market Items (null) ");

            return lines.ToArray();
        }

        public void Invoke( string[] args )
        {
            ConsoleModel.warn("InventoryModel has no invocation.");
        }
    }
}
