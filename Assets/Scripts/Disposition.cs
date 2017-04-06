using UnityEngine;
using System.Collections;
using Newtonsoft.Json;

public class Disposition
{
	[JsonProperty("name")]
    public string name;

	[JsonProperty("like")]
    public string like;

	[JsonProperty("dislike")]
    public string dislike;

	[JsonProperty("color")]
	private int _color
	{
		set {
	    	byte R = (byte)((value >> 16) & 0xFF);
			byte G = (byte)((value >> 8) & 0xFF);
			byte B = (byte)((value) & 0xFF);
			color = new Color32(R, G, B, 255);
		}
	}

	[JsonIgnore]
    public Color color;
}