using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ambition;

public class LeaveThePartyButtonController : MonoBehaviour {

    public Image buttonImage;
    public Text buttonText;

    void Update()
    {
		bool enable = (!GameData.tonightsParty.tutorial || GameData.tonightsParty.Host.IsLockedIn);
        buttonImage.enabled = enable;
        buttonText.enabled = enable;
    }
}