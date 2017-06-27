using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Ambition
{
	public class AccessoryInventorySizeTracker : MonoBehaviour {
	    private Text myText;
	    private InventoryModel _model;

	    // Use this for initialization
	    void Start()
	    {
	    	_model = AmbitionApp.GetModel<InventoryModel>();
	        myText = GetComponent<Text>();
	    }

	    // Update is called once per frame
	    // TODO: This has to not be updated every frame
	    void Update()
	    {
			myText.text = _model.Inventory.Count.ToString() + "/" + _model.MaxSlots;
	    }
	}
}