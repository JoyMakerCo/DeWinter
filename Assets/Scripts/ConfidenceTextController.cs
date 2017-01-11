using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ConfidenceTextController : MonoBehaviour {
    Text myText;

    // Use this for initialization
    void Start()
    {
        myText = this.GetComponent<Text>();
    }

    void Update()
    {
        myText.text = "Confidence: " + GameData.tonightsParty.currentPlayerConfidence + "/" + GameData.tonightsParty.maxPlayerConfidence;
    }
}
