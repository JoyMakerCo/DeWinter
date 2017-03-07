using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace DeWinter
{
	public class SortAccessoryInventoryItems : MonoBehaviour {

	    public string inventoryType;

	    public GameObject typeButton;
	    public GameObject styleButton;

	    Image typeButtonImage;
	    Image styleButtonImage;

	    Image typeButtonTriangleImage;
	    Image styleButtonTriangleImage;

	    Text typeButtonText;
	    Text styleButtonText;

	    string sortedBy;
	    bool ascendingOrder;

	    private InventoryModel _model;

	    private List<ItemVO> _list;

	    // Use this for initialization
	    void Start()
	    {
			_model = DeWinterApp.GetModel<InventoryModel>();
	        SetUpButtons();

	        if (inventoryType == "personal")
		        _model.Inventory.TryGetValue(ItemConsts.ACCESSORY, out _list);
		    else
		    	_list = _model.Market.FindAll(i => i.Type == ItemConsts.ACCESSORY);

		    if (_list == null) _list = new List<ItemVO>();
	        SortByType();
	    }

	    void SetUpButtons()
	    {
	        //--Type--
	        typeButtonImage = typeButton.GetComponent<Image>();
	        typeButtonTriangleImage = typeButton.transform.Find("Image").GetComponent<Image>();
	        typeButtonText = typeButton.transform.Find("Text").GetComponent<Text>();

	        //--Style--
	        styleButtonImage = styleButton.GetComponent<Image>();
	        styleButtonTriangleImage = styleButton.transform.Find("Image").GetComponent<Image>();
	        styleButtonText = styleButton.transform.Find("Text").GetComponent<Text>();
	    }

	    public void SortByType()
	    {
			ascendingOrder = (sortedBy == "type") && !ascendingOrder;
			SelectedButton("type");
	        sortedBy = "type";
 			_list.Sort(sortByTypeComparer);
 		}

		private int sortByTypeComparer(ItemVO a, ItemVO b)
		{
			return ascendingOrder ? a.Name.CompareTo(b.Name) : b.Name.CompareTo(a.Name);
		}

	    public void SortByStyle()
	    {
			ascendingOrder = (sortedBy == "style") && !ascendingOrder;
			SelectedButton("style");
			sortedBy = "style";
 			_list.Sort(sortByStyleComparer);
 		}

		private int sortByStyleComparer(ItemVO a, ItemVO b)
		{
			object styleA, styleB;
			if (a.States == null || !a.States.TryGetValue(ItemConsts.STYLE, out styleA))
				styleA = null;
			if (b.States == null || !b.States.TryGetValue(ItemConsts.STYLE, out styleB))
				styleB = null;

			return ascendingOrder ? ((string)styleA).CompareTo((string)styleB) : ((string)styleB).CompareTo((string)styleA);
		}

	    void SelectedButton(string button)
	    {
	        switch (button)
	        {
	            case "type":
	                typeButtonImage.color = Color.black;
	                typeButtonText.alignment = TextAnchor.MiddleLeft;
	                typeButtonTriangleImage.color = Color.white;
	                if (ascendingOrder)
	                {
	                    typeButtonTriangleImage.transform.localScale = new Vector3(1, -1, 1);
	                }
	                else
	                {
	                    typeButtonTriangleImage.transform.localScale = new Vector3(1, 1, 1);
	                }
	                styleButtonImage.color = Color.white;
	                styleButtonText.alignment = TextAnchor.MiddleCenter;
	                styleButtonTriangleImage.color = Color.clear;
	                break;
	            case "style":
	                styleButtonImage.color = Color.black;
	                styleButtonText.alignment = TextAnchor.MiddleLeft;
	                styleButtonTriangleImage.color = Color.white;
	                if (ascendingOrder)
	                {
	                    styleButtonTriangleImage.transform.localScale = new Vector3(1, -1, 1);
	                }
	                else
	                {
	                    styleButtonTriangleImage.transform.localScale = new Vector3(1, 1, 1);
	                }
	                typeButtonImage.color = Color.white;
	                typeButtonText.alignment = TextAnchor.MiddleCenter;
	                typeButtonTriangleImage.color = Color.clear;
	                break;
	        }
	    }
	}
}