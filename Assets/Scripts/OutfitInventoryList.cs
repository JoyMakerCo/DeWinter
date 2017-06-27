using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Ambition
{
	public class OutfitInventoryList : MonoBehaviour {

	    public string inventoryType;
	    public GameObject outfitInventoryButtonPrefab;
	    public WardrobeImageController imageController;

	    private OutfitInventoryModel _model;

	    // Use this for initialization
	    void Start () {
	    	_model = AmbitionApp.GetModel<OutfitInventoryModel>();
	        GenerateInventoryButtons();
	    }

	    public void GenerateInventoryButtons()
	    {
			List<Outfit> outfits = (inventoryType == ItemConsts.PERSONAL ? _model.Inventory : _model.Merchant);
	        foreach (Outfit o in outfits)
	        {
	            GameObject button = GameObject.Instantiate(outfitInventoryButtonPrefab);
	            OutfitInventoryButton buttonStats = button.GetComponent<OutfitInventoryButton>();
	            buttonStats.outfit = o;
	            buttonStats.inventoryType = inventoryType;
	            button.transform.SetParent(this.transform, false);
	            buttonStats.imageController = imageController;
	        }
	    }

		public Outfit selectedInventoryOutfit
		{
			get { return _model.PartyOutfit; }
			set {
				_model.PartyOutfit = value;
			}
		}
	    public void ClearInventoryButtons()
	    {
	        foreach (Transform child in this.transform)
	        {
	            GameObject.Destroy(child.gameObject);
	        }
	    }
	}
}
