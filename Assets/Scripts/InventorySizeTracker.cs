using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Ambition
{
	public class InventorySizeTracker : MonoBehaviour {
	    private Text myText;
	    private OutfitInventoryModel model;

	    // Use this for initialization
	    void Start () {
			model = AmbitionApp.GetModel<OutfitInventoryModel>();
	        myText = GetComponent<Text>();
	    }
		
		// Update is called once per frame
		void Update () {
			myText.text = model.Inventory.Count.ToString() + "/" + model.Capacity.ToString();
		}
	}
}
