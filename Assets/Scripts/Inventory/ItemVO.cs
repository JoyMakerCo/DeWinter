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

        [JsonProperty("config")]
        public string Config;

        [JsonProperty("asset")]
        public string AssetID;

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
            ID = item.ID;
            Config = item.Config;
            AssetID = item.AssetID;
        }

        public override string ToString()
        {
            // yuck
            if (Type == ItemType.Gossip)
            {
                return GossipWrapperVO.ToString(this);
            }
            return string.Format( "ItemVO {0} {1} £{2}", Name, Type, Price);
        }

        public string[] Dump()
        {
            if (Type == ItemType.Gossip)
            {
                return new string[] { GossipWrapperVO.ToString(this) };
            }

            var lines = new List<string>();
            lines.Add( string.Format( "ItemVO {0} {1} £{2}", Name, Type, Price) );

            if (State.Count > 0)
            {
                lines.Add("|"+string.Join( ", ", State.Select(x => x.Key + ":" + x.Value).ToArray() ) );
            }
            if (Type == ItemType.Outfit)
            {
                lines.Add("|"+OutfitWrapperVO.GetDescription(this));
            }
            return lines.ToArray();

        }
    }
}
