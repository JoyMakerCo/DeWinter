using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Util;
using UnityEngine;

namespace Ambition
{
    [Serializable]
    public class ItemVO
    {
        [JsonProperty("name")]
        public string Name; // Override Name (not used, but if the player is ever allowed to rename items, this is where it would go)

        [JsonProperty("id")]
        public string ID; // The id of the item's definition

        [JsonProperty("type")]
        public ItemType Type; // The category of item (outfit, gossip, etc)

        [JsonProperty("Price")]
        public int Price; // Price to buy from market OR the sellback price

        [JsonProperty("state")]
        public Dictionary<string, string> State = new Dictionary<string, string>(); // Unique state of this item instance

        [JsonProperty("equipped")]
        public bool Equipped = false;

        [JsonProperty("permanent")]
        public bool Permanent = false;

        [JsonProperty("market")]
        public bool Market = true;
        
        [JsonIgnore]
        public DateTime Created;

        [JsonProperty("created")]
        private long _created
        {
            set => Created = new DateTime(value);
            get => Created.Ticks;
        }

        [JsonProperty("asset")]
        public string AssetID;

        public ItemVO() {}
        public ItemVO(ItemVO item) : this(item, item.State) {}
        public ItemVO(ItemVO item, Dictionary<string, string> state)
        {
            Type = item.Type;
            Equipped = item.Equipped;
            State = new Dictionary<string, string>(state);
            Price = item.Price;
            Created = item.Created;
            ID = item.ID;
            AssetID = item.AssetID;
            Permanent = item.Permanent;
        }

        public override string ToString()
        {
            string lines = string.Format( "ItemVO {0} {1} £{2}", Name, Type, Price);
            if (State != null && State.Count > 0)
            {
                lines += "\n State:";
                foreach (KeyValuePair<string, string> state in State)
                {
                    lines += "\n  " + state.Key + ":" + state.Value;
                }
            }
            return lines;
        }
    }
}
