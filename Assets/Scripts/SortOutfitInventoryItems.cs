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

	private OutfitInventoryModel _model;
	private List<Outfit> _list;

    string sortedBy;
    bool ascendingOrder;

    // Use this for initialization
    void Start ()
    {
		_model = AmbitionApp.GetModel<OutfitInventoryModel>();
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
		_list = (inventoryType == ItemConsts.PERSONAL) ? _model.Inventory : _model.Merchant;
		_list.Sort(sortByNoveltyComparer);
	}

	private int sortByNoveltyComparer(Outfit a, Outfit b)
	{
		return a.novelty.CompareTo(b.novelty);
	}

	public void SortByLuxury()
    {
		ascendingOrder = (sortedBy == "luxury") && !ascendingOrder;
		SelectedButton("luxury");
		sortedBy = "luxury";
		_list = (inventoryType == ItemConsts.PERSONAL) ? _model.Inventory : _model.Merchant;
		_list.Sort(sortByLuxuryComparer);
	}

	private int sortByLuxuryComparer(Outfit a, Outfit b)
	{
		return a.luxury.CompareTo(b.luxury);
	}

    public void SortByModesty()
    {
		ascendingOrder = (sortedBy == "modesty") && !ascendingOrder;
		SelectedButton("modesty");
		sortedBy = "modesty";
		_list = (inventoryType == ItemConsts.PERSONAL) ? _model.Inventory : _model.Merchant;
		_list.Sort(sortByModestyComparer);
	}

	private int sortByModestyComparer(Outfit a, Outfit b)
	{
		return a.modesty.CompareTo(b.modesty);
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
