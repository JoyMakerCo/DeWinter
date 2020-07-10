using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Ambition;

public class SortOutfitInventoryItems : MonoBehaviour {

    public string inventoryType;

    public GameObject noveltyButton;
    public GameObject luxuryButton;
    public GameObject modestyButton;

    Image noveltyButtonImage;
    Image luxuryButtonImage;
    Image modestyButtonImage;

    Image noveltyButtonTriangleImage;
    Image luxuryButtonTriangleImage;
    Image modestyButtonTriangleImage;

    Text noveltyButtonText;
    Text luxuryButtonText;
    Text modestyButtonText;

	private InventoryModel _model;
	private List<ItemVO> _list;

    string sortedBy;
    bool ascendingOrder;

    // Use this for initialization
    void Start ()
    {
		_model = AmbitionApp.GetModel<InventoryModel>();
        SetUpButtons();
        SortByNovelty();
	}

    void SetUpButtons()
    {
        //--Novelty--
        noveltyButtonImage = noveltyButton.GetComponent<Image>();
        noveltyButtonTriangleImage = noveltyButton.transform.Find("Image").GetComponent<Image>();
        noveltyButtonText = noveltyButton.transform.Find("Text").GetComponent<Text>();

        //--Luxury--
        luxuryButtonImage = luxuryButton.GetComponent<Image>();
        luxuryButtonTriangleImage = luxuryButton.transform.Find("Image").GetComponent<Image>();
        luxuryButtonText = luxuryButton.transform.Find("Text").GetComponent<Text>();

        //--Modesty--
        modestyButtonImage = modestyButton.GetComponent<Image>();
        modestyButtonTriangleImage = modestyButton.transform.Find("Image").GetComponent<Image>();
        modestyButtonText = modestyButton.transform.Find("Text").GetComponent<Text>();
    }

	public void SortByNovelty()
    {
		ascendingOrder = (sortedBy == "novelty") && !ascendingOrder;
		SelectedButton("novelty");
		sortedBy = "novelty";
		(inventoryType == ItemConsts.PERSONAL ? _model.Inventory : _model.Market).Sort(sortByNoveltyComparer);
	}

	public void SortByLuxury()
    {
		ascendingOrder = (sortedBy == "luxury") && !ascendingOrder;
		SelectedButton("luxury");
		sortedBy = "luxury";
		(inventoryType == ItemConsts.PERSONAL ? _model.Inventory : _model.Market).Sort(sortByLuxuryComparer);
	}

    private int sortByNoveltyComparer(ItemVO a, ItemVO b) => Compare(a, b, ItemConsts.NOVELTY);
    private int sortByLuxuryComparer(ItemVO a, ItemVO b) => Compare(a, b, ItemConsts.LUXURY);
    private int sortByModestyComparer(ItemVO a, ItemVO b) => Compare(a, b, ItemConsts.MODESTY);
    int Compare(ItemVO a, ItemVO b, string state)
    {
        string str = null;
        int ia = (a.State?.TryGetValue(state, out str) ?? false) ? int.Parse(str) : 0;
        int ib = (b.State?.TryGetValue(state, out str) ?? false) ? int.Parse(str) : 0;
        return ia == ib ? 0 : (ascendingOrder && ia > ib) ? 1 : -1;
    }


    public void SortByModesty()
    {
		ascendingOrder = (sortedBy == "modesty") && !ascendingOrder;
		SelectedButton("modesty");
		sortedBy = "modesty";
		(inventoryType == ItemConsts.PERSONAL ? _model.Inventory : _model.Market).Sort(sortByModestyComparer);
	}

    void SelectedButton(string button)
    {
        switch (button)
        {
            case "novelty":
                noveltyButtonImage.color = Color.black;
                noveltyButtonText.alignment = TextAnchor.MiddleLeft;
                noveltyButtonTriangleImage.color = Color.white;
                if (ascendingOrder)
                {
                    noveltyButtonTriangleImage.transform.localScale = new Vector3(1, -1, 1);
                } else
                {
                    noveltyButtonTriangleImage.transform.localScale = new Vector3(1, 1, 1);
                }
                luxuryButtonImage.color = Color.white;
                luxuryButtonText.alignment = TextAnchor.MiddleCenter;
                luxuryButtonTriangleImage.color = Color.clear;
                modestyButtonImage.color = Color.white;
                modestyButtonText.alignment = TextAnchor.MiddleCenter;
                modestyButtonTriangleImage.color = Color.clear;
                break;
            case "luxury":
                luxuryButtonImage.color = Color.black;
                luxuryButtonText.alignment = TextAnchor.MiddleLeft;
                luxuryButtonTriangleImage.color = Color.white;
                if (ascendingOrder)
                {
                    luxuryButtonTriangleImage.transform.localScale = new Vector3(1, -1, 1);
                }
                else
                {
                    luxuryButtonTriangleImage.transform.localScale = new Vector3(1, 1, 1);
                }
                noveltyButtonImage.color = Color.white;
                noveltyButtonText.alignment = TextAnchor.MiddleCenter;
                noveltyButtonTriangleImage.color = Color.clear;
                modestyButtonImage.color = Color.white;
                modestyButtonText.alignment = TextAnchor.MiddleCenter;
                modestyButtonTriangleImage.color = Color.clear;
                break;
            case "modesty":
                modestyButtonImage.color = Color.black;
                modestyButtonText.alignment = TextAnchor.MiddleLeft;
                modestyButtonTriangleImage.color = Color.white;
                if (ascendingOrder)
                {
                    modestyButtonTriangleImage.transform.localScale = new Vector3(1, -1, 1);
                }
                else
                {
                    modestyButtonTriangleImage.transform.localScale = new Vector3(1, 1, 1);
                }
                luxuryButtonImage.color = Color.white;
                luxuryButtonText.alignment = TextAnchor.MiddleCenter;
                luxuryButtonTriangleImage.color = Color.clear;
                noveltyButtonImage.color = Color.white;
                noveltyButtonText.alignment = TextAnchor.MiddleCenter;
                noveltyButtonTriangleImage.color = Color.clear;
                break;
        }
    }
}
