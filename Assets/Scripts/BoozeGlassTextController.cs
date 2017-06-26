using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Ambition;

public class BoozeGlassTextController : MonoBehaviour {

    Text myText;
    private PartyModel _model;

    // Use this for initialization
    void Awake () {
    	_model = DeWinterApp.GetModel<PartyModel>();
        myText = this.GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update ()
	{
        myText.text = "Booze Glass: " + _model.Party.currentPlayerDrinkAmount + "/" + _model.Party.maxPlayerDrinkAmount;
    }
}
