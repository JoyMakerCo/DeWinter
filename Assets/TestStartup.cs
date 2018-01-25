using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ambition;

public class TestStartup : MonoBehaviour {

	// Use this for initialization
	void Awake ()
	{
		AmbitionApp.RegisterModel<FactionModel>();
		AmbitionApp.RegisterModel<MapModel>();
		AmbitionApp.RegisterModel<PartyModel>();
		AmbitionApp.RegisterCommand<GenerateMapCmd, PartyVO>(MapMessage.GENERATE_MAP);

		AmbitionApp.GetModel<PartyModel>().Party = new PartyVO();
		AmbitionApp.GetModel<PartyModel>().Party.Faction = "church";
	}
}
