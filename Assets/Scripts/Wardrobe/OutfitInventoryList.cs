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

	    void Awake ()
	    {
			_model = AmbitionApp.GetModel<InventoryModel>();
	        GenerateInventoryButtons();
	    }

	    public void GenerateInventoryButtons()
	    {
			List<ItemVO> outfits = (inventoryType == ItemConsts.PERSONAL ? _model.Inventory : _model.Market).FindAll(i=>i.Type == ItemConsts.OUTFIT);
	        foreach (ItemVO o in outfits)
	        {
	            GameObject button = Instantiate(outfitInventoryButtonPrefab);
	            OutfitInventoryButton buttonStats = button.GetComponent<OutfitInventoryButton>();
	            buttonStats.outfit = new OutfitVO(o);
	            buttonStats.inventoryType = inventoryType;
	            button.transform.SetParent(this.transform, false);
	            buttonStats.imageController = imageController;
	        }
	    }

		public OutfitVO selectedInventoryOutfit
		{
            get {
                ItemVO item;
                return _model.Equipped.TryGetValue(ItemConsts.OUTFIT, out item) ? item as OutfitVO : null;
            }
			set {
                AmbitionApp.SendMessage(InventoryMessages.EQUIP, value);
			}
		}
	    public void ClearInventoryButtons()
	    {
	        foreach (Transform child in this.transform)
	        {
	            Destroy(child.gameObject);
	        }
	    }
	}
}
