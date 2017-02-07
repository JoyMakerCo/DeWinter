using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneralReputationBenefitsListController : MonoBehaviour {

    Text text;

    // Use this for initialization
    void Start()
    {
        text = this.GetComponent<Text>();
        DisplayBenefits();
    }

    public void DisplayBenefits()
    {
        text.text = "Reputation Level Benefits";
        for (int i = 0; i < GameData.playerReputationLevel + 1; i++)
        {
            text.text += "\n" + GameData.reputationLevels[i].BenefitString();
            Debug.Log("General Reputation Level " + i + " Benefits!");
        }
    }
}
