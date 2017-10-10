using System;
using System.Collections.Generic;
using Newtonsoft.Json;
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

		[JsonProperty("states")]
		public Dictionary<string, object> States = new Dictionary<string, object>();

		[JsonProperty("Price")]
		public int Price;

		[JsonProperty("salePrice")]
		public int SellPrice;

		[JsonProperty("asset")]
		public string Asset;

		public int Quantity;

		public string PriceString
		{
			get { return Price.ToString("£" + "#,##0"); }
		}

		public string SellPriceString
		{
			get { return SellPrice.ToString("£" + "#,##0"); }
		}

		public ItemVO() {}
		public ItemVO(ItemVO item)
		{
			Name = item.Name;
			Description = item.Description;
			Type = item.Type;
			Tags = item.Tags;
			States = new Dictionary<string, object>(this.States);
			Price = item.Price;
			SellPrice = item.SellPrice;
			Asset = item.Asset;
			Quantity = item.Quantity;
		}
	}
}