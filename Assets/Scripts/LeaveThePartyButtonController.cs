using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaveThePartyButtonController : MonoBehaviour {

    public Image buttonImage;
    public Text buttonText;

    void Update()
    {
        if (!GameData.tonightsParty.tutorial || (GameData.tonightsParty.tutorial && GameData.tonightsParty.host.lockedInState != 0))
        {
            buttonImage.color = Color.white;
            buttonText.color = Color.white;
        } else
        {
            buttonImage.color = Color.clear;
            buttonText.color = Color.clear;
        }
    }
}
