using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Ambition;

public class ServantButtonController : MonoBehaviour
{
    private Text buttonText;
    private Image buttonImage;
    public GameObject screenFader; // It's for the Can't Afford Pop-up
    public string servantType;
    private ServantModel _model;
    private ServantVO _servant;

    void Start()
    {
        buttonText = transform.GetChild(0).GetComponent<Text>();
        buttonImage = this.GetComponent<Image>();
		_model = AmbitionApp.GetModel<ServantModel>();
		HandleServant(null);
    }

    void OnEnable()
    {
		AmbitionApp.Subscribe<ServantVO>(ServantMessages.SERVANT_FIRED, HandleServant);
		AmbitionApp.Subscribe<ServantVO>(ServantMessages.SERVANT_HIRED, HandleServant);
    }

	void OnDisable()
    {
		AmbitionApp.Unsubscribe<ServantVO>(ServantMessages.SERVANT_FIRED, HandleServant);
		AmbitionApp.Unsubscribe<ServantVO>(ServantMessages.SERVANT_HIRED, HandleServant);
    }

    private void HandleServant(ServantVO servant)
    {
		bool enabled = _model.Introduced.Contains(servant);
		buttonText.enabled = enabled;
		buttonImage.enabled = enabled;
		if (enabled)
		{
			_servant = servant;
			buttonText.color = _servant.Hired ? Color.red : Color.white;
			if (_servant.Hired)
			{
				buttonText.text = "Fire " + _servant.Name;
			}
			else
			{
				// button.enabled = _servant.Wage < GameData.moneyCount;
				buttonText.text = "Hire " + _servant.Name + " for £" + _servant.Wage;
			}
        }
        else _servant = null;
    }

    // TODO: This lives in the dialog
    public void HireOrFire()
    {
        if (!_servant.Hired) //Can't hire them unless they've been Introduced
        {
            if (!_servant.Hired && GameData.moneyCount >= _servant.Wage) //If they are NOT Hired and you CAN afford them
            {
            	AmbitionApp.SendMessage<ServantVO>(ServantMessages.HIRE_SERVANT, _servant);
            }
            else if (!_servant.Hired && GameData.moneyCount < _servant.Wage) //If they are NOT Hired and you CAN'T afford them
            {
                object[] objectStorage = new object[1];
                objectStorage[0] = _servant.NameAndTitle + "'s wages";
                screenFader.gameObject.SendMessage("CreateCantAffordModal", objectStorage);
            }
            else // If they ARE Hired, then it doesn't really matter whether or not you can afford her
            {
				AmbitionApp.SendMessage<ServantVO>(ServantMessages.FIRE_SERVANT, _servant);
            }
        }
    }
}
