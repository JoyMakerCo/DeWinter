using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Dialog;

namespace Ambition
{
	public class TwoPartyRSVPdPopUpController : MonoBehaviour, Util.IInitializable<PartyVO []>
	{
		private PartyVO[] _parties;

	    public void Initialize(PartyVO[] parties)
	    {
			//Title Text
	        Text titleText = this.transform.Find("TitleText").GetComponent<Text>();
	        titleText.text = "Which Party?";

	        //Body Text
	        Text bodyText = this.transform.Find("BodyText").GetComponent<Text>();
	        bodyText.text = "Madamme, you're currently scheduled to go to two Parties tonight." +
	                "\nWhich one will you be going to?";

	    	_parties = parties;
	        Text party1ButtonText = this.transform.Find("Party1Button").Find("Text").GetComponent<Text>();
	        party1ButtonText.text = _parties[0].Name();
	        Text party2ButtonText = this.transform.Find("Party2Button").Find("Text").GetComponent<Text>();
			party2ButtonText.text = _parties[1].Name();
	    }

	    public void SelectParty(int partyNumber)
	    {
	    	AmbitionApp.GetModel<PartyModel>().Party = _parties[partyNumber + 1];
	    }
	}
}