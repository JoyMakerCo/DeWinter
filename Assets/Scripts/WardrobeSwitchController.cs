using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WardrobeSwitchController : MonoBehaviour {

    public GameObject outfitsTab;
    public GameObject outfitsButton;
    public GameObject accessoriesTab;
    public GameObject accessoriesButton;

    // Use this for initialization
    void Start()
    {
        OutfitsSelected();
    }

    public void OutfitsSelected()
    {
        outfitsTab.transform.SetAsLastSibling();
        outfitsButton.GetComponent<Image>().color = Color.black;
        accessoriesButton.GetComponent<Image>().color = Color.white;
    }

    public void AccessoriesSelected()
    {
        accessoriesTab.transform.SetAsLastSibling();
        accessoriesButton.GetComponent<Image>().color = Color.black;
        outfitsButton.GetComponent<Image>().color = Color.white;
    }
}
