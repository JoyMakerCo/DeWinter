using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

    // Use this for initialization
    void Start()
    {
        SetUpButtons();
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
        if (sortedBy == "type")
        {
            ascendingOrder = !ascendingOrder;
        }
        else
        {
            ascendingOrder = false;
        }
        SelectedButton("type");
        sortedBy = "type";
        bool swapped = true;
        while (swapped)
        {
            swapped = false;
            for (int i = 1; i < AccessoryInventory.accessoryInventories[inventoryType].Count; i++)
            {
                if (ascendingOrder)
                {
                    if (AccessoryInventory.accessoryInventories[inventoryType][i - 1].TypeNumber() > AccessoryInventory.accessoryInventories[inventoryType][i].TypeNumber())
                    {
                        Swap(i - 1, i);
                        swapped = true;
                    }
                }
                else
                {
                    if (AccessoryInventory.accessoryInventories[inventoryType][i - 1].TypeNumber() < AccessoryInventory.accessoryInventories[inventoryType][i].TypeNumber())
                    {
                        Swap(i - 1, i);
                        swapped = true;
                    }
                }
            }
        }
    }

    public void SortByStyle()
    {
        if (sortedBy == "style")
        {
            ascendingOrder = !ascendingOrder;
        }
        else
        {
            ascendingOrder = false;
        }
        SelectedButton("style");
        sortedBy = "style";
        bool swapped = true;
        while (swapped)
        {
            swapped = false;
            for (int i = 1; i < AccessoryInventory.accessoryInventories[inventoryType].Count; i++)
            {
                if (ascendingOrder)
                {
                    if (AccessoryInventory.accessoryInventories[inventoryType][i - 1].StyleNumber() > AccessoryInventory.accessoryInventories[inventoryType][i].StyleNumber())
                    {
                        Swap(i - 1, i);
                        swapped = true;
                    }
                }
                else
                {
                    if (AccessoryInventory.accessoryInventories[inventoryType][i - 1].StyleNumber() < AccessoryInventory.accessoryInventories[inventoryType][i].StyleNumber())
                    {
                        Swap(i - 1, i);
                        swapped = true;
                    }
                }
            }
        }
    }

    void Swap(int accessory1, int accessory2)
    {
        Accessory placeHolder1 = AccessoryInventory.accessoryInventories[inventoryType][accessory1];
        Accessory placeHolder2 = AccessoryInventory.accessoryInventories[inventoryType][accessory2];

        AccessoryInventory.accessoryInventories[inventoryType][accessory1] = placeHolder2;
        AccessoryInventory.accessoryInventories[inventoryType][accessory2] = placeHolder1;
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
