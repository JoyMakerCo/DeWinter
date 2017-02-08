using UnityEngine;
using System.Collections;
using Newtonsoft.Json;

public class ServantVO
{
	[JsonProperty("name")]
	public string name; //What is their given name?
	
	[JsonProperty("slot")]
    public string slot; //What role do they fill? Seamstress? Spymaster? Bodyguard?

	[JsonProperty("title")]
	public string title; //Title, just used in name stuff

	[JsonProperty("description")]
	public string description; //What this servant can do for the Player

	[JsonProperty("wage")]
	public int wage; //How much they cost per week

    public bool introduced=false; //Has this servant been introduced yet?
    public bool hired=false; //Are you employing this Servant right now?

    public string NameAndTitle
    {
    	get { return name + ", the " + title; }
    }

    public string RecordData
    {
		get { return description + "\n- Costs " + wage.ToString("£" + "#,##0") + "/Week"; }
    }
}