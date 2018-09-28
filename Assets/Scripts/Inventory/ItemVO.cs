using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Util;

namespace Ambition
{
	public class ItemVO
	{
		[JsonProperty("id")]
		public string ID;

		[JsonProperty("name")]
		public string Name;

		[JsonProperty("description")]
		public string Description;

		[JsonProperty("type")]
		public string Type;

		[JsonProperty("tags")]
		public List<string> Tags;

		[JsonProperty("state")]
		public Dictionary<string,object> State = new Dictionary<string, object>();

		[JsonProperty("Price")]
		public int Price;

		[JsonProperty("asset")]
		public string Asset;

		public int Quantity;

		public bool Equipped;

		public string PriceString
		{
			get { return Price.ToString("£" + "#,##0"); }
		}

		public ItemVO() {}
		public ItemVO(ItemVO item)
		{
			ID = item.ID;
			Name = item.Name;
			Description = item.Description;
			Type = item.Type;
			Tags = item.Tags;
			State = new Dictionary<string, object>(item.State);
			Price = item.Price;
			Asset = item.Asset;
			Quantity = item.Quantity;
		}
	}
}