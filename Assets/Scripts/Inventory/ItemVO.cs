using System;
using System.Collections.Generic;
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
        public string Name;

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

        [JsonIgnore]
        public DateTime Created;

        [JsonProperty("created")]
        private long _created
        {
            set => Created = new DateTime(value);
            get => Created.Ticks;
        }

        [JsonProperty("Asset")]
        public Sprite Asset;

        public ItemVO() {}
        public ItemVO(ItemVO item) : this(item, item.State) {}
        public ItemVO(ItemVO item, Dictionary<string, string> state)
        {
            Name = item.Name;
            Type = item.Type;
            Equipped = item.Equipped;
            State = new Dictionary<string, string>(state);
            Price = item.Price;
            Created = item.Created;
        }

        public override string ToString()
        {
            return string.Format( "ItemVO {0} {1} {2} £{3}", Name, Type, Equipped?"equipped":"not equipped", Price);
        }
    }
}
