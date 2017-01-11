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
        if (GameData.servantDictionary["Seamstress"].Hired()) //If the Seamstress has been hired, enable the New Outfit Ability
        {
            buttonText.text = "Seamstress - New Outfit";
            if (OutfitInventory.personalInventory.Count < OutfitInventory.personalInventoryMaxSize) //As long as there is room to fit the new Outfit
            {
                buttonImage.color = Color.white;
                buttonText.color = Color.white;
            }
            else
            {
                buttonImage.color = Color.gray;
                buttonText.color = Color.white;
            }
        } else if (GameData.servantDictionary["Tailor"].Hired()) //If the Tailor has been hired, enable the Alter Outfit Ability
        {
            buttonText.text = "Tailor - Alter (£20)";
            if (outfitInventoryList.selectedInventoryOutfit != -1) //As long as an Outfit has been selected
            {
                if (OutfitInventory.personalInventory[outfitInventoryList.selectedInventoryOutfit].altered || GameData.moneyCount < 20) //As long as they have the money to pay for it
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

        }
        else
        {
            buttonImage.color = Color.clear;
            buttonText.color = Color.clear;
        }        
    }

    public void ClothierServantAbility()
    {
        if (GameData.servantDictionary["Tailor"].Hired())
        {
            AlterationWindow();
        } else if (GameData.servantDictionary["Seamstress"].Hired())
        {
            CreateNewOutfitWindow();
        }
    }

    void AlterationWindow()
    {
        if (!OutfitInventory.personalInventory[outfitInventoryList.selectedInventoryOutfit].altered && GameData.moneyCount > 20) //If the Seamstress has been Hired and the Outfit hasn't been Altered AND you can afford it
        {
            object[] objectStorage = new object[1];
            objectStorage[0] = outfitInventoryList.selectedInventoryOutfit;
            screenFader.gameObject.SendMessage("CreateAlterOutfitModal", objectStorage);
        }
    }

    void CreateNewOutfitWindow()
    {
        if (OutfitInventory.personalInventory.Count < OutfitInventory.personalInventoryMaxSize) //As long as there is room for a new Outfit
        {
            object[] objectStorage = new object[1];
            objectStorage[0] = outfitInventoryList;
            screenFader.gameObject.SendMessage("CreateSewNewOutfitModal", objectStorage);
        } else 
        {
            //Error Message telling the Player to make Room
            screenFader.gameObject.SendMessage("CreateCantFitNewOutfitModal");
        }
    }
}
