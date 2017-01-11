using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

    string sortedBy;
    bool ascendingOrder;

    // Use this for initialization
    void Start () {
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
        if(sortedBy == "novelty")
        {
            ascendingOrder = !ascendingOrder;
        } else
        {
            ascendingOrder = false;
        }
        SelectedButton("novelty");
        sortedBy = "novelty";
        bool swapped = true;
        while (swapped)
        {
            swapped = false;
            for (int i = 1; i < OutfitInventory.outfitInventories[inventoryType].Count; i++)
            {
                if (ascendingOrder)
                {
                    if (OutfitInventory.outfitInventories[inventoryType][i - 1].novelty > OutfitInventory.outfitInventories[inventoryType][i].novelty)
                    {
                        Swap(i - 1, i);
                        swapped = true;
                    }
                } else
                {
                    if (OutfitInventory.outfitInventories[inventoryType][i - 1].novelty < OutfitInventory.outfitInventories[inventoryType][i].novelty)
                    {
                        Swap(i - 1, i);
                        swapped = true;
                    }
                }                
            }
        }
    }

    public void SortByLuxury()
    {
        if (sortedBy == "luxury")
        {
            ascendingOrder = !ascendingOrder;
        }
        else
        {
            ascendingOrder = false;
        }
        SelectedButton("luxury");
        sortedBy = "luxury";
        bool swapped = true;
        while (swapped)
        {
            swapped = false;
            for (int i = 1; i < OutfitInventory.outfitInventories[inventoryType].Count; i++)
            {
                if (ascendingOrder)
                {
                    if (OutfitInventory.outfitInventories[inventoryType][i - 1].luxury > OutfitInventory.outfitInventories[inventoryType][i].luxury)
                    {
                        Swap(i - 1, i);
                        swapped = true;
                    }
                }
                else
                {
                    if (OutfitInventory.outfitInventories[inventoryType][i - 1].luxury < OutfitInventory.outfitInventories[inventoryType][i].luxury)
                    {
                        Swap(i - 1, i);
                        swapped = true;
                    }
                }
            }
        }
    }

    public void SortByModesty()
    {
        if (sortedBy == "modesty")
        {
            ascendingOrder = !ascendingOrder;
        }
        else
        {
            ascendingOrder = false;
        }
        SelectedButton("modesty");
        sortedBy = "modesty";
        bool swapped = true;
        while (swapped)
        {
            swapped = false;
            for (int i = 1; i < OutfitInventory.outfitInventories[inventoryType].Count; i++)
            {
                if (ascendingOrder)
                {
                    if (OutfitInventory.outfitInventories[inventoryType][i - 1].modesty > OutfitInventory.outfitInventories[inventoryType][i].modesty)
                    {
                        Swap(i - 1, i);
                        swapped = true;
                    }
                }
                else
                {
                    if (OutfitInventory.outfitInventories[inventoryType][i - 1].modesty < OutfitInventory.outfitInventories[inventoryType][i].modesty)
                    {
                        Swap(i - 1, i);
                        swapped = true;
                    }
                }
            }
        }
    }

    void Swap(int outfit1, int outfit2)
    {
        Outfit placeHolder1 = OutfitInventory.outfitInventories[inventoryType][outfit1];
        Outfit placeHolder2 = OutfitInventory.outfitInventories[inventoryType][outfit2];

        OutfitInventory.outfitInventories[inventoryType][outfit1] = placeHolder2;
        OutfitInventory.outfitInventories[inventoryType][outfit2] = placeHolder1;
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
