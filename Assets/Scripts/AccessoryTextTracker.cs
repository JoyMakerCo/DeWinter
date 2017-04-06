﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DeWinter;

public class AccessoryTextTracker : MonoBehaviour
{
    Text myText;

    // Use this for initialization
    void Start ()
    {
		ItemVO accessory = DeWinterApp.GetModel<InventoryModel>().SelectedItem;
        myText = this.GetComponent<Text>();
        myText.text = (accessory != null)
			? "Acessory: " + accessory.Name
			: "";
    }
}