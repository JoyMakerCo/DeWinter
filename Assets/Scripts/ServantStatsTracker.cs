using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DeWinter;

public class ServantStatsTracker : MonoBehaviour {

    public GameObject screenFader; // It's for the Pop-ups

    public Text nameText;
    public Text descriptionText;

    public GameObject hireOrFireButton;
    Image hireOrFireButtonImage;
    Text hireOrFireButtonText;
    public ServantInventoryList servantList;

    static bool attemptedCamilleFiring = false;
	
	void Start()
    {
        hireOrFireButtonImage = hireOrFireButton.GetComponent<Image>();
        hireOrFireButtonText = hireOrFireButton.transform.Find("Text").GetComponent<Text>();
    }

    // Update is called once per frame
	void Update () {
	if (servantList.selectedServant != null)
        {
            nameText.text = servantList.selectedServant.NameAndTitle;
            descriptionText.text = servantList.selectedServant.description;
            if(servantList.inventoryType == ServantInventoryList.InventoryType.Personal)
            {
                if (servantList.selectedServant.Name != "Camille" || !attemptedCamilleFiring)
                {
                    hireOrFireButtonImage.color = Color.white;
                    hireOrFireButtonText.text = "Fire " + servantList.selectedServant.Name;
                } else
                {
                    hireOrFireButtonImage.color = Color.gray;
                    hireOrFireButtonText.text = "Fire " + servantList.selectedServant.Name;
                }

            } else
            {
                hireOrFireButtonImage.color = Color.white;
                hireOrFireButtonText.text = "Hire " + servantList.selectedServant.Name + " for " + servantList.selectedServant.Wage.ToString("£" + "#,##0");
            }
            
        } else
        {
            nameText.text = "";
            descriptionText.text = "";
            hireOrFireButtonImage.color = Color.clear;
            hireOrFireButtonText.text = "";
        }
	}

    public void HireServant()
    {
		DeWinterApp.SendCommand<HireServantCmd, ServantVO>(servantList.selectedServant);
        servantList.ClearInventoryButtons();
        servantList.GenerateInventoryButtons();
    }

    public void FireServant()
    {
		ServantVO servant = servantList.selectedServant;
		if(servant != null)
        {
			if (servant.Name != "Camille")
            {
				DeWinterApp.SendCommand<FireServantCmd, ServantVO>(servantList.selectedServant);
		        servantList.ClearInventoryButtons();
		        servantList.GenerateInventoryButtons();
            } else if (!attemptedCamilleFiring) //If the Player hasn't attempted to fire Camille yet then throw up this message bubble
            {
                attemptedCamilleFiring = true;
                screenFader.gameObject.SendMessage("CreateCamilleFiringModal");
            }
        }
    }
}
