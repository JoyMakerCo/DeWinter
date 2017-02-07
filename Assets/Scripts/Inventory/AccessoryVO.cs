using System;
using System.Collections;
using Newtonsoft.Json;

public class AccessoryVO
{
	[JsonProperty("type")]
	public string Type;

	[JsonProperty("name")]
	private string _name;
	public string Name
    {
    	get { return style + " " + _name; }
    }

	[JsonProperty("description")]
	private string _description;
	public string Description
    {
		get { return "This " + Name + " " + _description; }
    }

    public string style; // Styles: Frankish, Venezian and Catalan

	[JsonProperty("priceMin")]
	public int PriceMin;

	[JsonProperty("priceMax")]
	public int PriceMax;

    public int Price;

    // Generate a random Accessory
    public AccessoryVO() {}


        public int Price(string inventory)
    {
        if (inventory == "personal")
        {

            return (int)(price * 0.5);
        }
        else
        {
            return price;
        }
    }
}