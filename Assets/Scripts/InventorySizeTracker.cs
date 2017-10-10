using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Ambition
{
	public class InventorySizeTracker : MonoBehaviour {
	    private Text myText;
	    private InventoryModel model;

	    // Use this for initialization
	    void Start () {
			model = AmbitionApp.GetModel<InventoryModel>();
	        myText = GetComponent<Text>();
	    }
		
		// Update is called once per frame
		void Update () {
			myText.text = model.Inventory.FindAll(i=>i.Type == ItemConsts.OUTFIT).Count.ToString() + "/" + model.NumOutfits.ToString();
		}
	}
}
