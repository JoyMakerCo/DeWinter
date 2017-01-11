using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class WardrobeImageController : MonoBehaviour {

    public Sprite noOutfit;
    public Sprite frankishOutfit;
    public Sprite venezianOutfit;
    public Sprite catalanOutfit;

    public Image displayImage;
    
    //Eventually this will need to be a multi-dimensional list (3 dimensions) for Styles, Modesty and Luxury
    public List<Sprite> outfitList;
    public int displayID;

	// Use this for initialization
	void Start () {
        StockOutfitList();
	}
	
	// Update is called once per frame
	void Update () {
        displayImage.sprite = outfitList[displayID];
	}

    void StockOutfitList()
    {
        outfitList.Add(noOutfit);
        outfitList.Add(frankishOutfit);
        outfitList.Add(venezianOutfit);
        outfitList.Add(catalanOutfit);
    }
}
