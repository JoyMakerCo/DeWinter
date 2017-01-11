using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TwoPartyChoicePopUpController : MonoBehaviour {

    public GameObject screenFader;
    public Day affectedDay;

    // Use this for initialization
    void Start () {
        Text party1ButtonText = this.transform.Find("Party1Button").Find("Text").GetComponent<Text>();
        party1ButtonText.text = affectedDay.party1.Name();
        Text party2ButtonText = this.transform.Find("Party2Button").Find("Text").GetComponent<Text>();
        party2ButtonText.text = affectedDay.party2.Name();
    }

    public void SelectParty(int partyNumber)
    {
        switch (partyNumber)
        {
            case 1:
                if (affectedDay.party1.RSVP == 1)
                {
                    object[] objectStorage = new object[2];
                    objectStorage[0] =affectedDay.party1;
                    if (GameData.currentMonth == affectedDay.month && GameData.currentDay == affectedDay.day)
                    {
                        objectStorage[1] = true;
                    }
                    else
                    {
                        objectStorage[1] = false;
                    }
                    screenFader.gameObject.SendMessage("CreateCancellationPopUp", objectStorage);
                    break;
                }
                else
                {
                    screenFader.gameObject.SendMessage("CreateRSVPPopUp", affectedDay.party1);
                    break;
                }
            case 2:
                if (affectedDay.party2.RSVP == 1)
                {
                    object[] objectStorage = new object[2];
                    objectStorage[0] = affectedDay.party2;
                    if (GameData.currentMonth == affectedDay.month && GameData.currentDay == affectedDay.day)
                    {
                        objectStorage[1] = true;
                    }
                    else
                    {
                        objectStorage[1] = false;
                    }
                    screenFader.gameObject.SendMessage("CreateCancellationPopUp", objectStorage);
                    break;
                }
                else
                {
                    screenFader.gameObject.SendMessage("CreateRSVPPopUp", affectedDay.party2);
                    break;
                }
        }
    }
}
