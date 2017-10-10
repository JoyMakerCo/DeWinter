using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Ambition
{
	public class OutfitInventoryList : MonoBehaviour {

	    public string inventoryType;
	    public GameObject outfitInventoryButtonPrefab;
	    public WardrobeImageController imageController;

	    private InventoryModel _model;
	    private GameModel _gameModel;

	    void Awake ()
	    {
			_model = AmbitionApp.GetModel<InventoryModel>();
			_gameModel = AmbitionApp.GetModel<GameModel>();
	        GenerateInventoryButtons();
	    }

	    public void GenerateInventoryButtons()
	    {
			List<ItemVO> outfits = (inventoryType == ItemConsts.PERSONAL ? _model.Inventory : _model.Market).FindAll(i=>i.Type == ItemConsts.OUTFIT);
	        foreach (ItemVO o in outfits)
	        {
	            GameObject button = GameObject.Instantiate(outfitInventoryButtonPrefab);
	            OutfitInventoryButton buttonStats = button.GetComponent<OutfitInventoryButton>();
	            buttonStats.outfit = new OutfitVO(o);
	            buttonStats.inventoryType = inventoryType;
	            button.transform.SetParent(this.transform, false);
	            buttonStats.imageController = imageController;
	        }
	    }

		public OutfitVO selectedInventoryOutfit
		{
			get { return _gameModel.Outfit; }
			set {
				_gameModel.Outfit = value;
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
