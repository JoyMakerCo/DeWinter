using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DeWinter;

public class JournalTabController : MonoBehaviour {
    public GameObject personalTab;
    public GameObject personalButton;
    public GameObject factionsTab;
    public GameObject factionsButton;
    public GameObject notablesTab;
    public GameObject notablesButton;
    public GameObject enemiesTab;
    public GameObject enemiesButton;

    public GeneralReputationBenefitsListController benefitsController;

    // Use this for initialization
    void Start()
    {
        PersonalSelected();
    }

    public void PersonalSelected()
    {
        personalTab.transform.SetAsLastSibling();
        personalButton.GetComponent<Image>().color = Color.black;
        factionsButton.GetComponent<Image>().color = Color.white;
        notablesButton.GetComponent<Image>().color = Color.white;
        enemiesButton.GetComponent<Image>().color = Color.white;
    }

    public void FactionsSelected()
    {
        factionsTab.transform.SetAsLastSibling();
        factionsButton.GetComponent<Image>().color = Color.black;
        personalButton.GetComponent<Image>().color = Color.white;
        notablesButton.GetComponent<Image>().color = Color.white;
        enemiesButton.GetComponent<Image>().color = Color.white;
    }

    public void NotablesSelected()
    {
        notablesTab.transform.SetAsLastSibling();
        notablesButton.GetComponent<Image>().color = Color.black;
        personalButton.GetComponent<Image>().color = Color.white;
        factionsButton.GetComponent<Image>().color = Color.white;
        enemiesButton.GetComponent<Image>().color = Color.white;
    }

    public void EnemiesSelected()
    {
        enemiesTab.transform.SetAsLastSibling();
        enemiesButton.GetComponent<Image>().color = Color.black;
        personalButton.GetComponent<Image>().color = Color.white;
        factionsButton.GetComponent<Image>().color = Color.white;
        notablesButton.GetComponent<Image>().color = Color.white;
    }
}
