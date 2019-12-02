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
		public InventoryModel() : base ("InventoryData") 
        {
            Gossip = Resources.Load("Gossip Config") as GossipConfig;
            if (Gossip == null)
            {
                Debug.LogError("Missing Gossip Config file, using defaults");
                Gossip = ScriptableObject.CreateInstance<GossipConfig>();
            }

            Debug.LogWarning("MIKE, WHERE ARE THE ITEMS");
            Items = new ItemVO[] {};


            var itemTemplates = Resources.Load("Item Templates") as ItemTemplates;
            if (itemTemplates == null)
            {
                Debug.LogError("Missing Item Templates file, you need to create and populate it");
                itemTemplates = ScriptableObject.CreateInstance<ItemTemplates>();
            }

            Items = itemTemplates.Items.Select( ic => ic.GetItem() ).ToArray(); 
        }

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

        [JsonProperty("items")]
        public ItemVO[] Items; // Base Item Definitions, used for instantiation.

        [JsonProperty("gossip_activity")]
        public int GossipActivity;

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

        public ItemVO GetItem( ItemType itemType, string itemID )
        {
            if (Inventory.ContainsKey(itemType))
            {
                foreach (var ivo in Inventory[itemType])
                {
                    if (ivo.ID == itemID)
                    {
                        return ivo;
                    }
                }
            }

            return null;
        }

        public ItemVO GetItem( string itemID )
        {
            foreach (var kv in Inventory)
            {
                foreach (var ivo in kv.Value)
                {
                    if (ivo.ID == itemID)
                    {
                        return ivo;
                    }
                }
            }
            return null;
        }
        public bool HasItem( ItemType itemType, string itemID )
        {
            return (GetItem(itemType,itemID) != null);
        }

        public bool HasItem( string itemID )
        {
            return (GetItem(itemID) != null);
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
            if (!Equipped.TryGetValue(slot, out int index)) return null;
            if (!Inventory.TryGetValue(slot, out List<ItemVO> items)) return null;
            var result = index < items.Count ? items[index] : null;

            if (result != null)
            {
                if (!result.Equipped)
                {
                    Debug.LogWarningFormat( "item in equipped list but not marked equipped: {0}",result.ToString());
                }
            }

            return result;
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
                "Gossip Activity: " + GossipActivity.ToString(),
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
                            if (Equipped.ContainsKey(kv.Key))
                            {
							    if (Equipped[kv.Key] == i)
							    {
								    equipt = " >";
							    }
                            }
						}
                        foreach (var line in kv.Value[i].Dump() )
                        {
    						lines.Add( equipt + line );
                        }
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
                        foreach (var line in kv.Value[i].Dump() )
                        {
    						lines.Add( "  " + line );
                        }
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
