using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SewNewOutfitPopUpController : MonoBehaviour {

    int outfitCost;
    string outfitStyle;
    int outfitLuxury;
    int outfitModesty;

    Slider luxurySlider;
    Slider modestySlider;

    public GameObject catalanStyleButton;
    public GameObject frankishStyleButton;
    public GameObject venezianStyleButton;

    public Image outfitCostButtonImage;
    public Text outfitCostText;

    public OutfitInventoryList personalInventoryList;

    // Use this for initialization
    void Start () {
	    luxurySlider = this.transform.Find("LuxuryText").Find("Slider").GetComponent<Slider>();
        modestySlider = this.transform.Find("ModestyText").Find("Slider").GetComponent<Slider>();
        FrankishSelected();
    }
	
	// Update is called once per frame
	void Update () {
        outfitLuxury = (int)luxurySlider.value;
        outfitModesty = (int)modestySlider.value;
        outfitCost = CalculatePrice();
        outfitCostText.text = "Create(" + outfitCost.ToString("£" + "#,##0") + ")";
        if (GameData.moneyCount >= outfitCost)
        {
            outfitCostButtonImage.color = Color.white;
        } else
        {
            outfitCostButtonImage.color = Color.gray;
        }
    }

    public void SewNewOutfit()
    {
        if(GameData.moneyCount >= outfitCost)
        {
            OutfitInventory.personalInventory.Add(new Outfit(110, outfitModesty, outfitLuxury, outfitStyle));
            GameData.moneyCount -= outfitCost;
            personalInventoryList.ClearInventoryButtons();
            personalInventoryList.GenerateInventoryButtons();
            Destroy(transform.parent.gameObject);
            GameData.activeModals--;
        } else
        {
            Debug.Log("Can't Afford to make that Outfit :(");
        }
    }

    int CalculatePrice()
    {
        int calcPrice = (int)(Mathf.Abs(outfitModesty) + Mathf.Abs(outfitLuxury));
        if (outfitStyle != GameData.currentStyle) //Check to see if this Outfit matches what's in Style
        {
            calcPrice = (int)(calcPrice * GameData.outOfStylePriceMultiplier);
        }
        if (calcPrice < 10) // If the Price is less than 10 make it 10. Will Sell for 5 at most (Sell price is 50% of Buy Price)
        {
            calcPrice = 10;
        }
        calcPrice = (int)(calcPrice * 1.5);
        return calcPrice;
    }

    public void CatalanSelected()
    {
        outfitStyle = "Catalan";
        catalanStyleButton.GetComponent<Image>().color = Color.black;
        frankishStyleButton.GetComponent<Image>().color = Color.white;
        venezianStyleButton.GetComponent<Image>().color = Color.white;
    }

    public void FrankishSelected()
    {
        outfitStyle = "Frankish";
        catalanStyleButton.GetComponent<Image>().color = Color.white;
        frankishStyleButton.GetComponent<Image>().color = Color.black;
        venezianStyleButton.GetComponent<Image>().color = Color.white;
    }

    public void VenezianSelected()
    {
        outfitStyle = "Venezian";
        catalanStyleButton.GetComponent<Image>().color = Color.white;
        frankishStyleButton.GetComponent<Image>().color = Color.white;
        venezianStyleButton.GetComponent<Image>().color = Color.black;
    }
}
