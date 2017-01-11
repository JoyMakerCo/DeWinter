using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ServantButtonController : MonoBehaviour
{
    private Text buttonText;
    private Image buttonImage;
    public GameObject screenFader; // It's for the Can't Afford Pop-up
    public string servantType;

    void Start()
    {
        buttonText = transform.GetChild(0).GetComponent<Text>();
        buttonImage = this.GetComponent<Image>();
    }

    void Update()
    {
        if (GameData.servantDictionary[servantType].Introduced()) //Don't even show the button unless they've been Introduced
        {
            buttonImage.color = Color.white;
            if (!GameData.servantDictionary[servantType].Hired())
            {
                if (GameData.servantDictionary[servantType].Wage() < GameData.moneyCount)
                {
                    buttonText.color = Color.white;
                }
                else
                {
                    buttonText.color = Color.red;
                }
                buttonText.text = "Hire " + GameData.servantDictionary[servantType].Name() + " for £" + GameData.servantDictionary[servantType].Wage();
            }
            else
            {
                buttonText.color = Color.white;
                buttonText.text = "Fire " + GameData.servantDictionary[servantType].Name();
            }
        } else
        {
            buttonText.color = Color.clear;
            buttonImage.color = Color.clear;
        }
        
    }

    public void HireOrFire()
    {
        if (GameData.servantDictionary[servantType].Introduced()) //Can't hire them unless they've been Introduced
        {
            if (!GameData.servantDictionary[servantType].Hired() && GameData.moneyCount >= GameData.servantDictionary[servantType].Wage()) //If they are NOT Hired and you CAN afford them
            {
                GameData.servantDictionary[servantType].Hire();
                GameData.moneyCount -= GameData.servantDictionary[servantType].Wage(); //Everyone's gotta get paid!
            }
            else if (!GameData.servantDictionary[servantType].Hired() && GameData.moneyCount < GameData.servantDictionary[servantType].Wage()) //If they are NOT Hired and you CAN'T afford them
            {
                object[] objectStorage = new object[1];
                objectStorage[0] = GameData.servantDictionary[servantType].NameAndTitle() + "'s wages";
                screenFader.gameObject.SendMessage("CreateCantAffordModal", objectStorage);
            }
            else // If they ARE Hired, then it doesn't really matter whether or not you can afford her
            {
                GameData.servantDictionary[servantType].Fire();
            }
        } else
        {
            Debug.Log("No introduction? Then no hiring!");
        }
        
    }
}