using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ServantStatsTracker : MonoBehaviour {

    public Text nameText;
    public Text descriptionText;

    public GameObject hireOrFireButton;
    Image hireOrFireButtonImage;
    Text hireOrFireButtonText;
    public ServantInventoryList servantList;
	
	void Start()
    {
        hireOrFireButtonImage = hireOrFireButton.GetComponent<Image>();
        hireOrFireButtonText = hireOrFireButton.transform.Find("Text").GetComponent<Text>();
    }

    // Update is called once per frame
	void Update () {
	if (servantList.selectedServant != null)
        {
            nameText.text = servantList.selectedServant.NameAndTitle();
            descriptionText.text = servantList.selectedServant.Description();
            if(servantList.inventoryType == ServantInventoryList.InventoryType.Personal)
            {
                hireOrFireButtonImage.color = Color.white;
                hireOrFireButtonText.text = "Fire " + servantList.selectedServant.Name();
            } else
            {
                hireOrFireButtonImage.color = Color.white;
                hireOrFireButtonText.text = "Hire " + servantList.selectedServant.Name() + " for " + servantList.selectedServant.Wage().ToString("£" + "#,##0");
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
        if (servantList.selectedServant != null)
        {
            if (servantList.selectedServant.Introduced())
            {
                servantList.selectedServant.Hire();
                servantList.ClearInventoryButtons();
                servantList.GenerateInventoryButtons();
            }
        }
    }

    public void FireServant()
    {
        if(servantList.selectedServant != null)
        {
            if (servantList.selectedServant.Hired())
            {
                if(servantList.selectedServant.Name() != "Camille")
                {
                    servantList.selectedServant.Fire();
                    servantList.ClearInventoryButtons();
                    servantList.GenerateInventoryButtons();
                }
            }
        }
    }
}
