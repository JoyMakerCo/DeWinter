using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AccessoryTextTracker : MonoBehaviour {

    Text myText;

    // Use this for initialization
    void Start () {
        myText = this.GetComponent<Text>();
        if(GameData.partyAccessoryID != -1)
        {
            myText.text = "Acessory: " + AccessoryInventory.personalInventory[GameData.partyAccessoryID].Name();
        } else
        {
            myText.text = "";
        }
        
    }
}
