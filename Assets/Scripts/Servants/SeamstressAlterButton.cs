using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Ambition;

public class SeamstressAlterButton : MonoBehaviour {

    public OutfitInventoryList outfitInventoryList;
    public SceneFadeInOut screenFader;
    Image buttonImage;
    Text buttonText;
    ServantModel _model;
    InventoryModel _outfits;

    void Start()
    {
        buttonImage = this.GetComponent<Image>();
        buttonText = this.transform.Find("Text").GetComponent<Text>();
		_model = AmbitionApp.GetModel<ServantModel>();
		_outfits = AmbitionApp.GetModel<InventoryModel>();
    }

    void Update()
    {
		if (_model.Hired.ContainsKey("Seamstress")) //If the Seamstress has been hired, enable the New Outfit Ability
        {
            buttonText.text = "Seamstress - New Outfit";
            if (_outfits.Inventory.Count < _outfits.NumOutfits) //As long as there is room to fit the new Outfit
            {
                buttonImage.color = Color.white;
                buttonText.color = Color.white;
            }
            else
            {
                buttonImage.color = Color.gray;
                buttonText.color = Color.white;
            }
		} else if (_model.Hired.ContainsKey("Tailor")) //If the Tailor has been hired, enable the Alter Outfit Ability
        {
            buttonText.text = "Tailor - Alter (£20)";
            if (outfitInventoryList.selectedInventoryOutfit != null) //As long as an Outfit has been selected
            {
                if (outfitInventoryList.selectedInventoryOutfit.Altered || GameData.moneyCount < 20) //As long as they have the money to pay for it
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
		if (_model.Hired.ContainsKey("Tailor"))
        {
            AlterationWindow();
		} else if (_model.Hired.ContainsKey("Seamstress"))
        {
            CreateNewOutfitWindow();
        }
    }

    void AlterationWindow()
    {
        if (!outfitInventoryList.selectedInventoryOutfit.Altered && GameData.moneyCount > 20) //If the Seamstress has been Hired and the Outfit hasn't been Altered AND you can afford it
        {
            object[] objectStorage = new object[1];
            objectStorage[0] = outfitInventoryList.selectedInventoryOutfit;
            screenFader.gameObject.SendMessage("CreateAlterOutfitModal", objectStorage);
        }
    }

    void CreateNewOutfitWindow()
    {
        if (_outfits.Inventory.Count < _outfits.NumOutfits) //As long as there is room for a new Outfit
        {
            object[] objectStorage = new object[1];
            objectStorage[0] = outfitInventoryList;
            screenFader.gameObject.SendMessage("CreateSewNewOutfitModal", objectStorage);
        } else 
        {
			Dictionary<string, string> subs = new Dictionary<string, string>()
				{{"$CAPACITY", _outfits.NumOutfits.ToString()}};
			AmbitionApp.OpenMessageDialog(DialogConsts.CANT_BUY_DIALOG, subs);
        }
    }
}