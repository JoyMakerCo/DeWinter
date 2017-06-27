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
    }

    void Update()
    {
		List<ServantVO> servants;
		_servant = (_model.Introduced.TryGetValue(servantType, out servants) && servants.Count > 0) ? servants[0] : null;
		if (_servant != null) //Don't even show the button unless they've been Introduced
        {
            buttonImage.color = Color.white;
            if (!_servant.Hired)
            {
                if (_servant.Wage < GameData.moneyCount)
                {
                    buttonText.color = Color.white;
                }
                else
                {
                    buttonText.color = Color.red;
                }
                buttonText.text = "Hire " + _servant.Name + " for £" + _servant.Wage;
            }
            else
            {
                buttonText.color = Color.white;
                buttonText.text = "Fire " + _servant.Name;
            }
        } else
        {
            buttonText.color = Color.clear;
            buttonImage.color = Color.clear;
        }
        
    }

    public void HireOrFire()
    {
        if (_servant.Introduced) //Can't hire them unless they've been Introduced
        {
            if (!_servant.Hired && GameData.moneyCount >= _servant.Wage) //If they are NOT Hired and you CAN afford them
            {
            	AmbitionApp.SendMessage<ServantVO>(ServantConsts.HIRE_SERVANT, _servant);
            }
            else if (!_servant.Hired && GameData.moneyCount < _servant.Wage) //If they are NOT Hired and you CAN'T afford them
            {
                object[] objectStorage = new object[1];
                objectStorage[0] = _servant.NameAndTitle + "'s wages";
                screenFader.gameObject.SendMessage("CreateCantAffordModal", objectStorage);
            }
            else // If they ARE Hired, then it doesn't really matter whether or not you can afford her
            {
				AmbitionApp.SendMessage<ServantVO>(ServantConsts.FIRE_SERVANT, _servant);
            }
        } else
        {
            Debug.Log("No introduction? Then no hiring!");
        }
        
    }
}