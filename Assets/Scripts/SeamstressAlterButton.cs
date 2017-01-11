using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SeamstressAlterButton : MonoBehaviour {

    public OutfitInventoryList outfitInventoryList;
    public SceneFadeInOut screenFader;
    Image buttonImage;
    Text buttonText;

    void Start()
    {
        buttonImage = this.GetComponent<Image>();
        buttonText = this.transform.Find("Text").GetComponent<Text>();
    }

    void Update()
    {
        if (GameData.servantDictionary["Seamstress"].Hired())
        {
            if (outfitInventoryList.selectedInventoryOutfit != -1)
            {
                if (OutfitInventory.personalInventory[outfitInventoryList.selectedInventoryOutfit].altered || GameData.moneyCount < 20)
                {
                    buttonImage.color = Color.gray;
                    buttonText.color = Color.white;
                }
                else
                {
                    buttonImage.color = Color.white;
                    buttonText.color = Color.white;
                }
            }
            else
            {
                buttonImage.color = Color.gray;
                buttonText.color = Color.white;
            }
        } else
        {
            buttonImage.color = Color.clear;
            buttonText.color = Color.clear;
        }        
    }

    public void AlterationWindow()
    {
        if (GameData.servantDictionary["Seamstress"].Hired() && !OutfitInventory.personalInventory[outfitInventoryList.selectedInventoryOutfit].altered && GameData.moneyCount > 20) //If the Seamstress has been Hired and the Outfit hasn't been Altered AND you can afford it
        {
            object[] objectStorage = new object[1];
            objectStorage[0] = outfitInventoryList.selectedInventoryOutfit;
            screenFader.gameObject.SendMessage("CreateAlterOutfitModal", objectStorage);
        }
    }
}
