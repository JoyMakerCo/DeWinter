using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
	    	List<ItemVO> items;
			myText.text = (_model.Inventory.TryGetValue(ItemConsts.ACCESSORY, out items) ? items.Count : 0).ToString() + "/" + _model.MaxSlots;
	    }
	}
}