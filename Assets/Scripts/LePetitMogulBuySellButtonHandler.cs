using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LePetitMogulBuySellButtonHandler : MonoBehaviour {

    public Image BuyButtonImage;
    public Image SellButtonImage;

    public GameObject PlayerInventory;
    public GameObject ShopInventory;

    void Start()
    {
        ClickBuyButon();
    }

    public void ClickBuyButon()
    {
        BuyButtonImage.color = Color.white;
        SellButtonImage.color = Color.clear;
        ShopInventory.SetActive(true);
        ShopInventory.transform.SetAsLastSibling();
        PlayerInventory.SetActive(false);
    }

    public void ClickSellButton()
    {
        BuyButtonImage.color = Color.clear;
        SellButtonImage.color = Color.white;
        ShopInventory.SetActive(false);
        PlayerInventory.SetActive(true);
        PlayerInventory.transform.SetAsLastSibling();
    }
}
