using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace DeWinter
{
	public class AccessoryInventorySizeTracker : MonoBehaviour {
	    private Text myText;
	    private InventoryModel _model;

	    // Use this for initialization
	    void Start()
	    {
	    	_model = DeWinterApp.GetModel<InventoryModel>();
	        myText = GetComponent<Text>();
	    }

	    // Update is called once per frame
	    void Update()
	    {
	    // TODO: Count ALL Items
			myText.text = _model.Inventory[ItemConsts.ACCESSORY].Count.ToString() + "/" + _model.MaxSlots;
	    }
	}
}