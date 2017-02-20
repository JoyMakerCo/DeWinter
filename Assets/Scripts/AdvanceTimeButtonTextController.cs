﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DeWinter;

public class AdvanceTimeButtonTextController : MonoBehaviour {

    private Text myText;
    private Outline myOutline; // This is for highlighting buttons
    private PartyModel _model;

    void Start ()
    {
    	DeWinterApp.GetModel<InventoryModel>().partyOutfitID = -1;
    	_model = DeWinterApp.GetModel<PartyModel>();
        myText = this.GetComponentInChildren<Text>();
        myOutline = this.GetComponent<Outline>();
    }

    void Update ()
    {
    	
		if (_model.Party.faction == null || _model.Party.RSVP == -1 || _model.Party.RSVP == 0)
        {
            myOutline.effectColor = Color.clear;
            myText.text = "Next Day";
        } else
        {
            myOutline.effectColor = Color.yellow;
            myText.text = "Go to the Party!";
        }
	}
}
