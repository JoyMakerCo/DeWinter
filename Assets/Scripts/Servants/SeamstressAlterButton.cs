using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Ambition;

public class SeamstressAlterButton : MonoBehaviour
{
    public OutfitInventoryList outfitInventoryList;
    public SceneFadeInOut screenFader;
	private Text _buttonText;
    private Button _button;
    private ServantModel _model;
	private InventoryModel _inventory;
    private ServantVO _servant;

    void Awake()
    {
        _button = this.GetComponent<Button>();
        _buttonText = this.transform.Find("Text").GetComponent<Text>();
		_model = AmbitionApp.GetModel<ServantModel>();
		AmbitionApp.Subscribe<ServantVO>(ServantMessages.SERVANT_HIRED, HandleServant);
		AmbitionApp.Subscribe<ServantVO>(ServantMessages.SERVANT_FIRED, HandleServant);
		HandleServant(null);
    }

	void OnDestroy()
    {
		AmbitionApp.Unsubscribe<ServantVO>(ServantMessages.SERVANT_HIRED, HandleServant);
		AmbitionApp.Unsubscribe<ServantVO>(ServantMessages.SERVANT_FIRED, HandleServant);
    }

	private void HandleServant(ServantVO servant)
    {
		bool enabled = _model.Servants.TryGetValue(ServantConsts.CLOTHIER, out _servant);
    	this.gameObject.SetActive(enabled);
		if (enabled)
    	{
    		switch (_servant.Type)
    		{
    			case ServantConsts.SEAMSTRESS:
					_buttonText.text = "Seamstress - New Outfit";
					enabled = (_inventory.NumOutfits < _inventory.NumSlots);
					break;
				case ServantConsts.TAILOR: 
					_buttonText.text = "Tailor - Alter (£20)";
					enabled = (outfitInventoryList.selectedInventoryOutfit.Altered || GameData.moneyCount < 20);
					break;
    		}
	    }
		_button.enabled = enabled;
    }

    // These dialogs don't exist yet.
    public void HandleAlter()
    {
    	if (_servant != null)
    	{
    		switch (_servant.Type)
    		{
    			case ServantConsts.SEAMSTRESS:
    				AmbitionApp.OpenDialog(DialogConsts.CREATE_OUTFIT_DIALOG);
    				break;
				case ServantConsts.TAILOR:
					AmbitionApp.OpenDialog(DialogConsts.ALTER_OUTFIT_DIALOG);
					break;
    		}
    	}
    }
}
