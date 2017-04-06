using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TwoPartyChoicePopUpController : MonoBehaviour {

    public GameObject screenFader;
    Party Party1;
    Party Party2;
    public bool isToday;

    // Use this for initialization
    void Start () {
    	
        Text party1ButtonText = this.transform.Find("Party1Button").Find("Text").GetComponent<Text>();
        party1ButtonText.text = Party1.Name();
        Text party2ButtonText = this.transform.Find("Party2Button").Find("Text").GetComponent<Text>();
        party2ButtonText.text = Party2.Name();
    }

    public void SelectParty(int partyNumber)
    {
        switch (partyNumber)
        {
            case 1:
                if (Party1.RSVP == 1)
                {
                    object[] objectStorage = new object[2];
                    objectStorage[0] = Party1;
					objectStorage[1] = isToday;
                    screenFader.gameObject.SendMessage("CreateCancellationPopUp", objectStorage);
                    break;
                }
                else
                {
                    screenFader.gameObject.SendMessage("CreateRSVPPopUp", Party1);
                    break;
                }
            case 2:
                if (Party2.RSVP == 1)
                {
                    object[] objectStorage = new object[2];
                    objectStorage[0] = Party2;
					objectStorage[1] = isToday;
                    screenFader.gameObject.SendMessage("CreateCancellationPopUp", objectStorage);
                    break;
                }
                else
                {
                    screenFader.gameObject.SendMessage("CreateRSVPPopUp", Party2);
                    break;
                }
        }
    }
}
