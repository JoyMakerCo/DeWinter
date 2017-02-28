using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DeWinter
{
	public class ItemVO : ICloneable
	{
		[JsonProperty("name")]
		public string Name;

		[JsonProperty("description")]
		public string Description;

		[JsonProperty("type")]
		public string Type;

		[JsonProperty("tags")]
		public string[] Tags;

		[JsonProperty("states")]
		public Dictionary<string, object> States;

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

		public object Clone()
		{
			ItemVO result = new ItemVO();
			result.Name = this.Name;
			result.Description = this.Description;
			result.Type = this.Type;
			result.Tags = this.Tags;
			result.States = new Dictionary<string, object>(this.States);
			result.Price = this.Price;
			result.SellPrice = this.SellPrice;
			result.Asset = this.Asset;
			result.Quantity = this.Quantity;
			return result;
		}
	}
}