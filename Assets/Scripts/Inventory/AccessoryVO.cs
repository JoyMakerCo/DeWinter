using System;
using Newtonsoft.Json;

public class AccessoryVO
{
	private string _name;
	private string _description;
	
	[JsonProperty("type")]
	public string Type;

	[JsonProperty("name")]
	public string Name
    {
    	get { return Style + " " + _name; }
    	private set { _name = value; }
    }

	[JsonProperty("description")]
	public string Description
    {
		get { return "This " + Name + " " + _description; }
		private set { _description = value; }
    }

    public string Style; // Styles: Frankish, Venezian and Catalan

	[JsonProperty("priceMin")]
	public int PriceMin;

	[JsonProperty("priceMax")]
	public int PriceMax;

    public int Price;
}