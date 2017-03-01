using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TwoPartyRSVPdPopUpController : MonoBehaviour {

    public GameObject screenFader;
    public Party party1;
    public Party party2;

    // Use this for initialization
    void Start()
    {
        Text party1ButtonText = this.transform.Find("Party1Button").Find("Text").GetComponent<Text>();
        party1ButtonText.text = party1.Name();
        Text party2ButtonText = this.transform.Find("Party2Button").Find("Text").GetComponent<Text>();
        party2ButtonText.text = party2.Name();
    }

    public void SelectParty(int partyNumber)
    {
		GameData.tonightsParty = (partyNumber == 1) ? party1 : party2;
    }
}