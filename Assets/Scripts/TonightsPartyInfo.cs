using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Ambition;
using Core;

namespace Ambition
{
	public class TonightsPartyInfo : MonoBehaviour
	{
		public Text PartyNameText;
		public Text PartyDescriptionText;
		public Text StyleText;
		public Text ObjectivesText;

	    // Use this for initialization
	    void Awake ()
	    {
	    	PartyVO party = AmbitionApp.GetModel<PartyModel>().Party;
	    	PartyNameText.text = party == null ? "" : party.Name;
	    	PartyDescriptionText.text = party == null ? "" : party.Description;
			StyleText.text = AmbitionApp.GetModel<InventoryModel>().CurrentStyle;
	    }
	}
}
