using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Ambition;

public class SeamstressAlterButton : MonoBehaviour {

    public OutfitInventoryList outfitInventoryList;
    public SceneFadeInOut screenFader;
	private Text _buttonText;
    private Button _button;
    private ServantModel _model;

    void Awake()
    {
        _button = this.GetComponent<Button>();
        _buttonText = this.transform.Find("Text").GetComponent<Text>();
		_model = AmbitionApp.GetModel<ServantModel>();
		HandleServant(null);
    }

    void OnEnable()
    {
    	AmbitionApp.Subscribe<ServantVO>(ServantMessages.SERVANT_HIRED, HandleServant);
		AmbitionApp.Subscribe<ServantVO>(ServantMessages.SERVANT_FIRED, HandleServant);
    }

	void OnDisable()
    {
		AmbitionApp.Unsubscribe<ServantVO>(ServantMessages.SERVANT_HIRED, HandleServant);
		AmbitionApp.Unsubscribe<ServantVO>(ServantMessages.SERVANT_FIRED, HandleServant);
    }

	private void HandleServant(ServantVO servant)
    {
		bool enabled = _model.Hired.TryGetValue(ServantConsts.CLOTHIER, out servant);
		_buttonText.enabled = enabled;
		_button.enabled = enabled;
    	if (enabled)
    	{
    		InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
    		switch (servant.Type)
    		{
    			case ServantConsts.SEAMSTRESS:
					_buttonText.text = "Seamstress - New Outfit";
					_button.enabled = (inventory.Inventory.Count < inventory.NumOutfits);
					break;
				case ServantConsts.TAILOR: 
					_buttonText.text = "Tailor - Alter (£20)";
					_button.enabled = (outfitInventoryList.selectedInventoryOutfit.Altered || GameData.moneyCount < 20);
					break;
    		}
	    }
    }
/// <summary>
// TODO: Make actual dialogs; Button functionality deduces type of clothier present
/// </summary>
/*
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
        if (_inventory.Inventory.Count < _inventory.NumOutfits) //As long as there is room for a new Outfit
        {
            object[] objectStorage = new object[1];
            objectStorage[0] = outfitInventoryList;
            screenFader.gameObject.SendMessage("CreateSewNewOutfitModal", objectStorage);
        } else 
        {
			Dictionary<string, string> subs = new Dictionary<string, string>()
				{{"$CAPACITY", _inventory.NumOutfits.ToString()}};
			AmbitionApp.OpenMessageDialog(DialogConsts.CANT_BUY_DIALOG, subs);
        }
    }
    */
}